using UnityEngine;

namespace JvLib.Events.Editor
{
    using UnityEditor;
    [CustomEditor(typeof(AStateBehaviour), true, isFallback = true)]
    public abstract class AStateMachineBehaviourEditor : Editor
    {
        private bool _foldoutState = false;

        protected GUIStyle _HeaderStyle;

        public sealed override void OnInspectorGUI()
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.Space();
            if (_HeaderStyle == null) _HeaderStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold
            };
            if (_foldoutState = EditorGUILayout.Foldout(_foldoutState, "State Behaviour Settings", _HeaderStyle))
            {
                EditorGUI.indentLevel++;
                GUI.enabled = false;
                EditorGUILayout.TextField(new GUIContent("Current State"), (target as AStateBehaviour).GetCurrentStateName());
                EditorGUILayout.FloatField(new GUIContent("Active Time"), (target as AStateBehaviour).GetCurrentStateTime());
                GUI.enabled = true;
                EditorGUI.indentLevel--;
            }
            OnGUI();
            EditorGUI.indentLevel--;
        }

        protected abstract void OnGUI();
    }
}
