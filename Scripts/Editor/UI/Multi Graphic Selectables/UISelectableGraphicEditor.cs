using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvLib.UI.Editor
{
    using UnityEditor;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UISelectableGraphic))]
    public class UISelectableGraphicEditor : Editor
    {
        private UISelectableGraphic _myTarget { get { return target as UISelectableGraphic; } }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_style"), new GUIContent("Transition"));
            EditorGUI.indentLevel++;
            if(_myTarget.Style == UISelectableGraphic.TransitionStyle.ColorTint)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_targetGraphic"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_colors"), GUIContent.none);
            }
            else if (_myTarget.Style == UISelectableGraphic.TransitionStyle.SpriteSwap)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_targetGraphic"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_sprites"), GUIContent.none);
            }
            else if (_myTarget.Style == UISelectableGraphic.TransitionStyle.Animation)
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_animations"), GUIContent.none);
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }
    }
}
