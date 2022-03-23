using System;
using UnityEditor;

namespace JvLib.Utilities
{
    public enum PlayModeState
    {
        ExitingPlayMode = 0,
        EditMode = 1,
        ExitingEditMode = 2,
        PlayMode = 3,
    }

    public static class PlayModeUtility
    {
        public static PlayModeState PlayModeState { get; private set; } = PlayModeState.PlayMode;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            PlayModeState = PlayModeState.EditMode;

            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange pStateChange)
        {
            PlayModeState = pStateChange switch
            {
                PlayModeStateChange.EnteredEditMode => PlayModeState.EditMode,
                PlayModeStateChange.ExitingEditMode => PlayModeState.ExitingEditMode,
                PlayModeStateChange.EnteredPlayMode => PlayModeState.PlayMode,
                PlayModeStateChange.ExitingPlayMode => PlayModeState.ExitingPlayMode,
                _ => throw new ArgumentOutOfRangeException(nameof(pStateChange), pStateChange, null)
            };
        }
    }
}
