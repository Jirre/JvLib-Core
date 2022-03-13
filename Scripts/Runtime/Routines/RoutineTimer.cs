using System;
using System.Collections;
using UnityEngine;

namespace JvLib.Routines
{
    public class RoutineTimer : MonoBehaviour
    {
        private static GameObject _timerObject;

        #region Start Timer Functions

        /// <summary>
        /// Start a timer routine using the time scale.
        /// </summary>
        /// <param name="duration">The duration of the timer.</param>
        /// <returns>Returns an active timer.</returns>
        public static RoutineTimer StartTimer(float duration)
        {
            return StartTimer(duration, false, null);
        }

        /// <summary>
        /// Start a timer routine using the time scale.
        /// </summary>
        /// <param name="duration">The duration of the timer.</param>
        /// <param name="callback">The action which will be performed after the timer is finished.</param>
        /// <returns>Returns an active timer.</returns>
        public static RoutineTimer StartTimer(float duration, Action callback)
        {
            return StartTimer(duration, false, callback);
        }

        /// <summary>
        /// Start a timer.
        /// </summary>
        /// <param name="duration">The duration of the timer.</param>
        /// <param name="useRealTime">Indicates whether the timer uses time scale or real time.</param>
        /// <returns>Returns an active timer.</returns>
        public static RoutineTimer StartTimer(float duration, bool useRealTime)
        {
            return StartTimer(duration, useRealTime, null);
        }

        /// <summary>
        /// Start a timer.
        /// </summary>
        /// <param name="duration">The duration of the timer.</param>
        /// <param name="useRealTime">Indicates whether the timer uses time scale or real time.</param>
        /// <param name="callback">The action which will be performed after the timer is finished.</param>
        /// <returns>Returns an active timer.</returns>
        public static RoutineTimer StartTimer(float duration, bool useRealTime, Action callback)
        {
            return StartTimer(duration, 1, useRealTime, callback);
        }

        /// <summary>
        /// Start a timer routine using the time scale. The callback will be fired multiple times.
        /// </summary>
        /// <param name="loopDuration">The time of one iteration after which the callback will be performed.</param>
        /// <param name="totalDuration">The total duration of the timer.</param>
        /// <param name="callback">The action that will be performed after each iteration.</param>
        /// <returns>Returns an active timer.</returns>
        public static RoutineTimer StartLoopedTimer(float loopDuration, float totalDuration, Action callback)
        {
            return StartLoopedTimer(loopDuration, totalDuration, false, callback);
        }

        /// <summary>
        /// Start a timer routine using the time scale. The callback will be fired multiple times.
        /// </summary>
        /// <param name="loopDuration">The time of one iteration after which the callback will be performed.</param>
        /// <param name="totalDuration">The total duration of the timer.</param>
        /// <param name="useRealTime">Indicates whether the timer uses time scale or real time.</param>
        /// <param name="callback">The action that will be performed after each iteration.</param>
        /// <returns>Returns an active timer.</returns>
        public static RoutineTimer StartLoopedTimer(float loopDuration, float totalDuration, bool useRealTime,
            Action callback)
        {
            return StartLoopedTimer(loopDuration, Mathf.RoundToInt(totalDuration / loopDuration), useRealTime,
                callback);
        }

        /// <summary>
        /// Start a timer routine using the time scale. The callback will be fired multiple times.
        /// </summary>
        /// <param name="loopDuration">The time of one iteration after which the callback will be performed.</param>
        /// <param name="nIterations">The number of iterations of the timer.</param>
        /// <param name="callback">The action that will be performed after each iteration.</param>
        /// <returns>Returns an active timer.</returns>
        public static RoutineTimer StartLoopedTimer(float loopDuration, int nIterations, Action callback)
        {
            return StartLoopedTimer(loopDuration, nIterations, false, callback);
        }

        /// <summary>
        /// Start a timer routine using the time scale. The callback will be fired multiple times.
        /// </summary>
        /// <param name="loopDuration">The time of one iteration after which the callback will be performed.</param>
        /// <param name="nIterations">The number of iterations of the timer.</param>
        /// <param name="useRealTime">Indicates whether the timer uses time scale or real time.</param>
        /// <param name="callback">The action that will be performed after each iteration.</param>
        /// <returns>Returns an active timer.</returns>
        public static RoutineTimer StartLoopedTimer(float loopDuration, int nIterations, bool useRealTime,
            Action callback)
        {
            return StartTimer(loopDuration, nIterations, useRealTime, callback);
        }

        #endregion

        private static RoutineTimer StartTimer(
            float duration, int nIterations, bool useRealTime, Action callback)
        {
            if (_timerObject == null)
                _timerObject = new GameObject("TimerHolder");

            RoutineTimer routineTimer = _timerObject.AddComponent<RoutineTimer>();
            routineTimer.StartCoroutine(routineTimer.TimerRoutine(
                duration, nIterations, useRealTime, callback));

            return routineTimer;
        }

        /// <summary>
        /// The time in seconds during the current iteration.
        /// </summary>
        public float CurrentTime { get; private set; }

        /// <summary>
        /// The remaining time in seconds during the current iteration.
        /// </summary>
        public float RemainingTime { get; private set; }

        /// <summary>
        /// The time between 0 and 1.
        /// </summary>
        public float NormalizedTime => CurrentTime / Duration;

        /// <summary>
        /// The duration of one iteration in seconds.
        /// </summary>
        public float Duration { get; private set; }

        /// <summary>
        /// Represents how often the timer sends the callback and resets itself.
        /// </summary>
        public int Iteration { get; private set; }

        public bool IsPaused { get; private set; }

        public bool IsCanceled { get; private set; }

        public bool IsStopped { get; private set; }

        public bool IsFinished { get; private set; }


        public IEnumerator TimerRoutine(float duration, int nIterations,
            bool useRealTime, Action callback)
        {
            IsFinished = false;

            this.Duration = RemainingTime = duration;

            while (Iteration < nIterations)
            {
                while (RemainingTime > 0)
                {
                    if (!IsPaused)
                    {
                        if (useRealTime)
                        {
                            RemainingTime = Mathf.Max(0f,
                                RemainingTime - Time.unscaledDeltaTime);
                        }
                        else
                        {
                            RemainingTime = Mathf.Max(0f,
                                RemainingTime - Time.deltaTime);
                        }
                    }

                    CurrentTime = duration - RemainingTime;

                    if (IsCanceled || IsStopped)
                        yield break;

                    yield return null;
                }

                Iteration++;
            }

            if (!IsCanceled && callback != null)
                callback();

            IsFinished = true;
            yield return null;
            Destroy(this);
        }


        /// <summary>
        /// Pause the timer.
        /// </summary>
        public void Pause()
        {
            IsPaused = true;
        }

        /// <summary>
        /// Unpause the timer.
        /// </summary>
        public void Resume()
        {
            IsPaused = false;
        }


        /// <summary>
        /// Cancel the timer. This will not trigger the callback.
        /// </summary>
        public void Cancel()
        {
            IsCanceled = true;
        }

        /// <summary>
        /// Stops the timer. And triggers the callback
        /// </summary>
        public void Stop()
        {
            IsStopped = true;
        }

        /// <summary>
        /// Reset the timer's duration and iteration counter.
        /// </summary>
        public void Reset()
        {
            RemainingTime = Duration;
            Iteration = 0;
        }


        public void SetRemainingTimeAt(float time)
        {
            RemainingTime = time;
        }

        /// <summary>
        /// Add time to the current duration.
        /// </summary>
        public void IncreaseDurationBy(float time)
        {
            RemainingTime += time;
        }
    }
}
