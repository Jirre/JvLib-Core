using System;
using System.Collections;
using System.Collections.Generic;
using JvLib.Events;
using Unity.Profiling;
using UnityEngine;
using JvLib.Routines;

namespace JvLib.Routines
{
    public class Routine : IRoutineYield
    {
        /// <summary>
        /// When the routine is forcibly stopped, this event will be called. If the Routine has
        /// already been stopped and ended, the callback will be called immediately
        /// </summary>
        private SafeEvent _onStoppedEvent;
        public event Action OnStoppedEvent
        {
            add
            {
                if (Stopped && Ended)
                    value();
                _onStoppedEvent += value;
            }
            remove => _onStoppedEvent -= value;
        }

        /// <summary>
        /// When the routine completes, this event will be called. If the Routine has completed, the
        /// callback will be called immediately
        /// </summary>
        private SafeEvent _onFinishedEvent;
        public event Action OnFinishedEvent
        {
            add
            {
                if (!Stopped && Ended)
                    value();
                _onFinishedEvent += value;
            }
            remove => _onFinishedEvent -= value;
        }

        /// <summary>
        /// When the routine ends, either by being stopped or finishing naturally, this event will be
        /// called. If the Routine has already ended, the callback will be called immediately
        /// </summary>
        private SafeEvent _onEndedEvent;
        public event Action OnEndedEvent
        {
            add
            {
                if (Ended)
                    value();
                _onEndedEvent += value;
            }
            remove => _onEndedEvent -= value;
        }

        public bool Running => !Ended;
        public bool Ended { get; private set; }
        public bool Stopped { get; private set; }
        public bool Paused { get; private set; }

        private MonoBehaviour _coroutineHolder;
        public MonoBehaviour CoroutineHolder
        {
            set => _coroutineHolder = value;
        }

        protected readonly IEnumerator Enumerator;

        private IEnumerator _extendedEnumerator;
        protected internal IEnumerator ExtendedEnumerator => _extendedEnumerator ??= ExtendEnumerator(Enumerator);

        private readonly List<Routine> _linkedRoutines = new List<Routine>();
        private static readonly WaitForFixedUpdate WAIT_FOR_FIXED_UPDATE = new WaitForFixedUpdate();
        private static readonly WaitForEndOfFrame WAIT_FOR_END_OF_FRAME = new WaitForEndOfFrame();

        private event Action<Routine> OnRoutineEndedEvent;

        public Routine()
        {
        }

        public Routine(IEnumerator enumerator)
        {
            this.Enumerator = enumerator;
        }

        public Routine(IEnumerator enumerator, MonoBehaviour monoBehaviour)
        {
            this.Enumerator = enumerator;
            _coroutineHolder = monoBehaviour;
        }

        public virtual void Start()
        {
            if (Enumerator == null) return;
            if (_coroutineHolder == null)
                RoutineHolder.StartRoutine(this);
            else
                _coroutineHolder.StartCoroutine(ExtendedEnumerator);
        }

        public void Stop()
        {
            Stopped = true;

            foreach (Routine routine in _linkedRoutines)
            {
                if (routine != null)
                {
                    routine.Stop();
                    routine.OnRoutineEndedEvent -= OnLinkedRoutineEnded;
                }
            }
        }

        public void Pause()
        {
            Paused = true;
        }

        public void Resume()
        {
            Paused = false;
        }
        
        private IEnumerator ExtendEnumerator(IEnumerator inputEnumerator)
        {
            RecursiveEnumerator currentEnumerator = new RecursiveEnumerator(inputEnumerator);
            while (!Stopped)
            {
                if (Paused)
                {
                    yield return null;
                    continue;
                }
                if (!currentEnumerator.MoveNext())
                        break;

                object current = currentEnumerator.Current;
                if (current is IRoutineYield routineYield)
                {
                    if (routineYield.Stopped)
                    {
                        Stopped = true;
                        break;
                    }

                    while (routineYield.Running && !Stopped)
                    {
                        if (routineYield.Stopped)
                        {
                            Stopped = true;
                            break;
                        }
                        yield return null;
                    }

                    if (Stopped)
                        routineYield.Stop();
                }
                else if (current is LinkToStopRoutine linkRoutine)
                {
                    _linkedRoutines.Add(linkRoutine.routine);
                    linkRoutine.routine.OnRoutineEndedEvent += OnLinkedRoutineEnded;
                }
                else
                {
                    yield return current;
                }
            }

            Ended = true;

            if (Stopped)
                DispatchOnStoppedEvent();
            else
                DispatchOnFinishedEvent();

            DispatchOnEndedEvent();
            DispatchOnRoutineEndedEvent();
        }

        private void OnLinkedRoutineEnded(Routine routine)
        {
            routine.OnRoutineEndedEvent -= OnLinkedRoutineEnded;
            _linkedRoutines.Remove(routine);
        }

        private void DispatchOnStoppedEvent()
        {
            _onStoppedEvent?.Dispatch();
        }

        private void DispatchOnFinishedEvent()
        {
            _onFinishedEvent?.Dispatch();
        }

        private void DispatchOnEndedEvent()
        {
            _onEndedEvent?.Dispatch();
        }

        private void DispatchOnRoutineEndedEvent()
        {
            OnRoutineEndedEvent?.Invoke(this);
        }

        public static Routine Start(IEnumerator enumerator)
        {
            Routine routine = new Routine(enumerator);
            routine.Start();
            return routine;
        }

        public static Routine Start(IEnumerator enumerator, MonoBehaviour monoBehaviour)
        {
            Routine routine = new Routine(enumerator, monoBehaviour);
            routine.Start();
            return routine;
        }

        public static void StopAndClear(ref Routine routine)
        {
            if (routine != null)
                routine.Stop();
            routine = null;
        }

        public static void StopAndStart(ref Routine routine, IEnumerator enumerator)
        {
            if (routine != null)
                routine.Stop();
            routine = Start(enumerator);
        }

        public static void StopAndStart(ref Routine routine, IEnumerator enumerator, MonoBehaviour monoBehaviour)
        {
            if (routine != null)
                routine.Stop();
            routine = new Routine(enumerator, monoBehaviour);
            routine.Start();
        }

        #region Simple Routines

        #region WaitForNumberOfFrames
        public static Routine WaitForNumberOfFrames(int nFrames, Action callback)
        {
            return Start(WaitForNumberOfFramesRoutine(nFrames, callback));
        }
        public static Routine WaitForNumberOfFrames(MonoBehaviour owner, int nFrames, Action callback)
        {
            return Start(WaitForNumberOfFramesRoutine(nFrames, callback), owner);
        }

        private static IEnumerator WaitForNumberOfFramesRoutine(int nFrames, Action callback)
        {
            for (int i = 0; i < nFrames; i++)
                yield return null;

            callback();
        }
        #endregion WaitForNumberOfFrames

        #region WaitForSeconds
        public static Routine WaitForSeconds(float seconds)
        {
            return Start(WaitForSecondsRoutine(seconds, false, null));
        }

        public static Routine WaitForSeconds(float seconds, Action callback)
        {
            return Start(WaitForSecondsRoutine(seconds, false, callback));
        }
        public static Routine WaitForSeconds(float seconds, bool useRealTime, Action callback)
        {
            return Start(WaitForSecondsRoutine(seconds, useRealTime, callback));
        }
        public static Routine WaitForSeconds(MonoBehaviour owner, float seconds, Action callback)
        {
            return Start(WaitForSecondsRoutine(seconds, false, callback), owner);
        }
        public static Routine WaitForSeconds(MonoBehaviour owner, float seconds, bool useRealTime, Action callback)
        {
            return Start(WaitForSecondsRoutine(seconds, useRealTime, callback), owner);
        }

        private static IEnumerator WaitForSecondsRoutine(float seconds, bool useRealTime, Action callback)
        {
            if (!useRealTime)
                yield return new WaitForSeconds(seconds);
            else
                yield return new WaitForRealSeconds(seconds);

            if (callback != null)
                callback();
        }
        #endregion WaitForSeconds

        #region UpdateSeconds
        public static Routine UpdateSeconds(float seconds, Action<RoutineTimer> updateCallback)
        {
            return Start(UpdateRoutine(seconds, false, UpdateType.Normal, updateCallback, null));
        }
        public static Routine UpdateSeconds(float seconds, bool useRealTime, Action<RoutineTimer> updateCallback)
        {
            return Start(UpdateRoutine(seconds, useRealTime, UpdateType.Normal, updateCallback, null));
        }
        public static Routine UpdateSeconds(float seconds, UpdateType updateType, Action<RoutineTimer> updateCallback)
        {
            return Start(UpdateRoutine(seconds, false, updateType, updateCallback, null));
        }
        public static Routine UpdateSeconds(float seconds, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(seconds, false, UpdateType.Normal, updateCallback, finishCallback));
        }
        public static Routine UpdateSeconds(float seconds, bool useRealTime, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(seconds, useRealTime, UpdateType.Normal, updateCallback, finishCallback));
        }
        public static Routine UpdateSeconds(float seconds, UpdateType updateType, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(seconds, false, updateType, updateCallback, finishCallback));
        }
        public static Routine UpdateSeconds(float seconds, bool useRealTime, UpdateType updateType, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(seconds, useRealTime, updateType, updateCallback, finishCallback));
        }
        public static Routine UpdateSeconds(MonoBehaviour owner, float seconds, Action<RoutineTimer> updateCallback)
        {
            return Start(UpdateRoutine(seconds, false, UpdateType.Normal, updateCallback, null), owner);
        }
        public static Routine UpdateSeconds(MonoBehaviour owner, float seconds, bool useRealTime, Action<RoutineTimer> updateCallback)
        {
            return Start(UpdateRoutine(seconds, useRealTime, UpdateType.Normal, updateCallback, null), owner);
        }
        public static Routine UpdateSeconds(MonoBehaviour owner, float seconds, UpdateType updateType, Action<RoutineTimer> updateCallback)
        {
            return Start(UpdateRoutine(seconds, false, updateType, updateCallback, null), owner);
        }
        public static Routine UpdateSeconds(MonoBehaviour owner, float seconds, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(seconds, false, UpdateType.Normal, updateCallback, finishCallback), owner);
        }
        public static Routine UpdateSeconds(MonoBehaviour owner, float seconds, bool useRealTime, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(seconds, useRealTime, UpdateType.Normal, updateCallback, finishCallback), owner);
        }
        public static Routine UpdateSeconds(MonoBehaviour owner, float seconds, UpdateType updateType, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(seconds, false, updateType, updateCallback, finishCallback), owner);
        }
        public static Routine UpdateSeconds(MonoBehaviour owner, float seconds, bool useRealTime, UpdateType updateType, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(seconds, useRealTime, updateType, updateCallback, finishCallback), owner);
        }

        private static IEnumerator UpdateRoutine(float seconds, bool useRealTime, UpdateType updateType, Action<RoutineTimer> updateCallback, Action finishCallback)
        {
            RoutineTimer routineTimer = RoutineTimer.StartTimer(seconds, useRealTime);

            while (!routineTimer.IsFinished)
            {
                if (updateCallback != null)
                    updateCallback(routineTimer);

                switch (updateType)
                {
                    case UpdateType.Normal:
                        yield return null;
                        break;
                    case UpdateType.Fixed:
                        yield return WAIT_FOR_FIXED_UPDATE;
                        break;
                    case UpdateType.EndOfFrame:
                        yield return WAIT_FOR_END_OF_FRAME;
                        break;
                    default:
                        yield return null;
                        break;
                }
            }

            if (updateCallback != null)
                updateCallback(routineTimer);

            if (finishCallback != null)
                finishCallback();
        }
        #endregion UpdateSeconds

        #region UpdateFrames
        public static Routine UpdateFrames(int nFrames, Action<int> updateCallback)
        {
            return Start(UpdateRoutine(nFrames, UpdateType.Normal, updateCallback, null));
        }
        public static Routine UpdateFrames(int nFrames, UpdateType updateType, Action<int> updateCallback)
        {
            return Start(UpdateRoutine(nFrames, updateType, updateCallback, null));
        }
        public static Routine UpdateFrames(int nFrames, Action<int> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(nFrames, UpdateType.Normal, updateCallback, finishCallback));
        }
        public static Routine UpdateFrames(int nFrames, UpdateType updateType, Action<int> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(nFrames, updateType, updateCallback, finishCallback));
        }
        public static Routine UpdateFrames(MonoBehaviour owner, int nFrames, Action<int> updateCallback)
        {
            return Start(UpdateRoutine(nFrames, UpdateType.Normal, updateCallback, null), owner);
        }
        public static Routine UpdateFrames(MonoBehaviour owner, int nFrames, UpdateType updateType, Action<int> updateCallback)
        {
            return Start(UpdateRoutine(nFrames, updateType, updateCallback, null), owner);
        }
        public static Routine UpdateFrames(MonoBehaviour owner, int nFrames, Action<int> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(nFrames, UpdateType.Normal, updateCallback, finishCallback), owner);
        }
        public static Routine UpdateFrames(MonoBehaviour owner, int nFrames, UpdateType updateType, Action<int> updateCallback, Action finishCallback)
        {
            return Start(UpdateRoutine(nFrames, updateType, updateCallback, finishCallback), owner);
        }

        private static IEnumerator UpdateRoutine(int nFrames, UpdateType updateType, Action<int> updateCallback, Action finishCallback)
        {
            for (int i = 0; i < nFrames; i++)
            {
                if (updateCallback != null)
                    updateCallback(i);
                if (i < nFrames - 1)
                {
                    switch (updateType)
                    {
                        case UpdateType.Normal:
                            yield return null;
                            break;
                        case UpdateType.Fixed:
                            yield return WAIT_FOR_FIXED_UPDATE;
                            break;
                        case UpdateType.EndOfFrame:
                            yield return WAIT_FOR_END_OF_FRAME;
                            break;
                        default:
                            yield return null;
                            break;
                    }
                }
            }

            if (finishCallback != null)
                finishCallback();
        }
        #endregion UpdateFrames

        #region Update
        public static Routine Update(Action updateCallback)
        {
            return Start(UpdateRoutine(UpdateType.Normal, updateCallback));
        }
        public static Routine Update(UpdateType updateType, Action updateCallback)
        {
            return Start(UpdateRoutine(updateType, updateCallback));
        }
        public static Routine Update(MonoBehaviour owner, Action updateCallback)
        {
            return Start(UpdateRoutine(UpdateType.Normal, updateCallback), owner);
        }
        public static Routine Update(MonoBehaviour owner, UpdateType updateType, Action updateCallback)
        {
            return Start(UpdateRoutine(updateType, updateCallback), owner);
        }

        private static IEnumerator UpdateRoutine(UpdateType updateType, Action updateCallback)
        {
            while (true)
            {
                if (updateCallback != null)
                    updateCallback();

                switch (updateType)
                {
                    case UpdateType.Normal:
                        yield return null;
                        break;
                    case UpdateType.Fixed:
                        yield return WAIT_FOR_FIXED_UPDATE;
                        break;
                    case UpdateType.EndOfFrame:
                        yield return WAIT_FOR_END_OF_FRAME;
                        break;
                    default:
                        yield return null;
                        break;
                }
            }
        }
        #endregion Update

        #endregion

        public static class Yields
        {
            public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
            public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
        }
    }

    public enum UpdateType
    {
        Normal,
        Fixed,
        EndOfFrame
    }

    public static class RoutineExtensions
    {
        public static bool IsRunning(this Routine routine)
        {
            return routine is {Running: true};
        }
    }
}
