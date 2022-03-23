using System;
using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Routines
{
    internal class RoutineHolder : MonoBehaviour
    {
        private static RoutineHolder _holder;
        private static RoutineHolder Holder
        {
            get
            {
                if (_holder != null) return _holder;
                
                if (_holder.IsNotNull() && PlayModeUtility.PlayModeState == PlayModeState.ExitingPlayMode)
                {
                    // Prevent OnDestroy Errors
                    return null;
                }

                if (Application.isPlaying)
                {
                    GameObject go = new GameObject("RoutineHolder");
                    _holder = go.AddComponent<RoutineHolder>();
                    DontDestroyOnLoad(_holder);
                }
                else
                {
                    throw new InvalidOperationException("Cannot start a routine while not in play-mode");
                }
                return _holder;
            }
            set => _holder = value;
        }

        internal static void StartRoutine(Routine routine)
        {
            if (Holder == null)
                return;
            Holder.StartCoroutine(routine.ExtendedEnumerator);
        }
    }
}
