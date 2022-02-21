using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace JvLib.Collections
{
    public abstract class SafeEventBase
    {
        protected abstract IList<CallbackData> Listeners { get; }

        public struct CallbackData
        {
            public object target;
            public MethodInfo methodInfo;

            public CallbackData(object target, MethodInfo methodInfo)
            {
                this.target = target;
                this.methodInfo = methodInfo;
            }

            public bool IsTargetDestroyedOrDespawned
            {
                get
                {
                    Object asObject = target as Object;

                    if ((object)asObject != null && asObject == null)
                        return true;

                    return false;
                }
            }
        }

        public abstract bool HasListeners { get; }

        public bool HasDestroyedOrDespawnedListeners
        {
            get
            {
                IList<CallbackData> listeners = Listeners;
                for (int i = 0; i < listeners.Count; i++)
                {
                    if (listeners[i].IsTargetDestroyedOrDespawned)
                        return true;
                }

                return false;
            }
        }

        public abstract void Clear();
    }

    public class SafeEvent : SafeEventBase
    {
        private readonly LinkedList<Action> listeners = new LinkedList<Action>();
        protected override IList<CallbackData> Listeners
        {
            get
            {
                List<CallbackData> result = new List<CallbackData>(listeners.Count);
                result.AddRange(listeners.Select(l => new CallbackData(l.Target, l.Method)));
                return result;
            }
        }

        public override bool HasListeners
        {
            get => listeners.Count > 0;
        }

        public void Subscribe(Action callback) => listeners.AddLast(callback);
        public void Unsubscribe(Action callback) => listeners.Remove(callback);
        public override void Clear() => listeners.Clear();

        public bool ContainsCallback(Action callback)
        {
            using (LinkedList<Action>.Enumerator enumerator = listeners.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Action listener = enumerator.Current;
                    if (listener.Method == callback.Method && listener.Target == callback.Target)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public virtual void Dispatch()
        {
            List<Action> dispatchingListeners = new List<Action>();
            dispatchingListeners.AddRange(listeners);

            for (int i = 0; i < dispatchingListeners.Count; i++)
            {
                Action listener = dispatchingListeners[i];
                listener();
            }

            dispatchingListeners.Clear();
        }

        public static SafeEvent operator +(SafeEvent e, Action a)
        {
            e.Subscribe(a);
            return e;
        }

        public static SafeEvent operator -(SafeEvent e, Action a)
        {
            e.Unsubscribe(a);
            return e;
        }
    }

    public class SafeEvent<T> : SafeEventBase
    {
        private readonly LinkedList<Action<T>> listeners = new LinkedList<Action<T>>();
        protected override IList<CallbackData> Listeners
        {
            get
            {
                List<CallbackData> result = new List<CallbackData>(listeners.Count);
                result.AddRange(listeners.Select(l => new CallbackData(l.Target, l.Method)));

                return result;
            }
        }

        public override bool HasListeners
        {
            get => listeners.Count > 0;
        }

        public void Subscribe(Action<T> callback) => listeners.AddLast(callback);
        public void Unsubscribe(Action<T> callback) => listeners.Remove(callback);
        public override void Clear() => listeners.Clear();

        public bool ContainsCallback(Action<T> callback)
        {
            using (LinkedList<Action<T>>.Enumerator enumerator = listeners.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Action<T> listener = enumerator.Current;
                    if (listener.Method == callback.Method && listener.Target == callback.Target)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Dispatch(T t)
        {
            List<Action<T>> dispatchingListeners = new List<Action<T>>();
            dispatchingListeners.AddRange(listeners);

            for (int i = 0; i < dispatchingListeners.Count; i++)
            {
                Action<T> listener = dispatchingListeners[i];
                listener(t);
            }

            dispatchingListeners.Clear();
        }

        public static SafeEvent<T> operator +(SafeEvent<T> e, Action<T> a)
        {
            e.Subscribe(a);
            return e;
        }

        public static SafeEvent<T> operator -(SafeEvent<T> e, Action<T> a)
        {
            e.Unsubscribe(a);
            return e;
        }
    }
}
