using UnityEngine;
using UnityEditorInternal;

namespace JvLib.UI.Editor
{
    using UnityEditor;
    [CustomEditor(typeof(AUISelectable),true)]
    public abstract class AUISelectableEditor : Editor
    {
        protected AUISelectable MyTarget { get { return target as AUISelectable; } }

        private ReorderableList _rList;

        protected virtual void OnEnable()
        {
            _rList = new ReorderableList(serializedObject, serializedObject.FindProperty("_selectableTargets"), true, true, true, true)
            {
                drawHeaderCallback = (Rect rect) =>
                {
                    EditorGUI.LabelField(rect, new GUIContent("Target Graphics"));
                },

                drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    EditorGUI.PropertyField(rect, serializedObject.FindProperty("_selectableTargets").GetArrayElementAtIndex(index), GUIContent.none);
                }
            };
        }
        protected virtual void OnDisable() { }
        public sealed override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_interactive"));
            _rList.DoLayoutList();

            InspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }
        public abstract void InspectorGUI();
    }
}
