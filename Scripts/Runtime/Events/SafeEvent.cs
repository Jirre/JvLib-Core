using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Object = UnityEngine.Object;

namespace JvLib.Events
{
    public abstract class SafeEventBase
    {
        protected abstract IList<CallbackData> Listeners { get; }

        public struct CallbackData
        {
            public readonly object Target;
            public MethodInfo MethodInfo;

            public CallbackData(object target, MethodInfo methodInfo)
            {
                this.Target = target;
                this.MethodInfo = methodInfo;
            }

            public bool IsTargetDestroyed
            {
                get
                {
                    Object asObject = Target as Object;

                    return (object)asObject != null && asObject == null;
                }
            }
        }

        public abstract bool HasListeners { get; }

        public bool HasDestroyedListeners
        {
            get
            {
                IList<CallbackData> listeners = Listeners;
                foreach (CallbackData c in listeners)
                {
                    if (c.IsTargetDestroyed)
                        return true;
                }

                return false;
            }
        }

        public abstract void Clear();
    }

    public class SafeEvent : SafeEventBase
    {
        private readonly LinkedList<Action> _listeners = new LinkedList<Action>();
        protected override IList<CallbackData> Listeners
        {
            get
            {
                List<CallbackData> result = new List<CallbackData>(_listeners.Count);
                result.AddRange(_listeners.Select(l => new CallbackData(l.Target, l.Method)));
                return result;
            }
        }

        public override bool HasListeners => 
            _listeners.Count > 0;

        public void Subscribe(Action callback) => _listeners.AddLast(callback);
        public void Unsubscribe(Action callback) => _listeners.Remove(callback);
        public override void Clear() => _listeners.Clear();

        public bool ContainsCallback(Action callback)
        {
            using LinkedList<Action>.Enumerator enumerator = _listeners.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Action listener = enumerator.Current;
                if (listener != null && 
                    listener.Method == callback.Method && 
                    listener.Target == callback.Target)
                {
                    return true;
                }
            }

            return false;
        }

        public virtual void Dispatch()
        {
            List<Action> dispatchingListeners = new List<Action>();
            dispatchingListeners.AddRange(_listeners);

            foreach (Action listener in dispatchingListeners)
            {
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
        private readonly LinkedList<Action<T>> _listeners = new LinkedList<Action<T>>();
        protected override IList<CallbackData> Listeners
        {
            get
            {
                List<CallbackData> result = new List<CallbackData>(_listeners.Count);
                result.AddRange(_listeners.Select(l => new CallbackData(l.Target, l.Method)));

                return result;
            }
        }

        public override bool HasListeners => 
            _listeners.Count > 0;

        public void Subscribe(Action<T> callback) => _listeners.AddLast(callback);
        public void Unsubscribe(Action<T> callback) => _listeners.Remove(callback);
        public override void Clear() => _listeners.Clear();

        public bool ContainsCallback(Action<T> callback)
        {
            using LinkedList<Action<T>>.Enumerator enumerator = _listeners.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Action<T> listener = enumerator.Current;
                if (listener != null && 
                    listener.Method == callback.Method && 
                    listener.Target == callback.Target)
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispatch(T t)
        {
            List<Action<T>> dispatchingListeners = new List<Action<T>>();
            dispatchingListeners.AddRange(_listeners);

            foreach (Action<T> listener in dispatchingListeners)
            {
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
    
    public class SafeEvent<T1, T2> : SafeEventBase
    {
        private readonly LinkedList<Action<T1, T2>> _listeners = new LinkedList<Action<T1, T2>>();
        protected override IList<CallbackData> Listeners
        {
            get
            {
                List<CallbackData> result = new List<CallbackData>(_listeners.Count);
                result.AddRange(_listeners.Select(l => new CallbackData(l.Target, l.Method)));

                return result;
            }
        }

        public override bool HasListeners => 
            _listeners.Count > 0;

        public void Subscribe(Action<T1, T2> callback) => _listeners.AddLast(callback);
        public void Unsubscribe(Action<T1, T2> callback) => _listeners.Remove(callback);
        public override void Clear() => _listeners.Clear();

        public bool ContainsCallback(Action<T1, T2> callback)
        {
            using LinkedList<Action<T1, T2>>.Enumerator enumerator = _listeners.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Action<T1, T2> listener = enumerator.Current;
                if (listener != null && 
                    listener.Method == callback.Method && 
                    listener.Target == callback.Target)
                {
                    return true;
                }
            }

            return false;
        }

        public void Dispatch(T1 x, T2 y)
        {
            List<Action<T1, T2>> dispatchingListeners = new List<Action<T1, T2>>();
            dispatchingListeners.AddRange(_listeners);

            foreach (Action<T1, T2> listener in dispatchingListeners)
            {
                listener(x, y);
            }

            dispatchingListeners.Clear();
        }

        public static SafeEvent<T1, T2> operator +(SafeEvent<T1, T2> e, Action<T1, T2> a)
        {
            e.Subscribe(a);
            return e;
        }

        public static SafeEvent<T1, T2> operator -(SafeEvent<T1, T2> e, Action<T1, T2> a)
        {
            e.Unsubscribe(a);
            return e;
        }
    }
}
