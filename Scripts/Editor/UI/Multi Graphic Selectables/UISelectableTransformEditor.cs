using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace JvLib.UI.Editor
{
    using UnityEditor;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(UISelectableTransform))]
    public class UISelectableTransformEditor : Editor
    {
        private UISelectableTransform _myTarget { get { return target as UISelectableTransform; } }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_targetTransform"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_rotation"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_scale"));

            serializedObject.ApplyModifiedProperties();
        }

        [CustomPropertyDrawer(typeof(UISelectableTransform.Vector3Container))]
        public class Vector3ContainerPropertyDrawer : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                position.height = EditorGUIUtility.singleLineHeight;
                property.FindPropertyRelative("IsAnimated").boolValue = EditorGUI.ToggleLeft(position, label, property.FindPropertyRelative("IsAnimated").boolValue, EditorStyles.boldLabel);
                if(property.FindPropertyRelative("IsAnimated").boolValue)
                {
                    EditorGUI.indentLevel++;
                    position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("Normal"));
                    position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("Highlighted"));
                    position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("Pressed"));
                    position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(position, property.FindPropertyRelative("Disabled"));
                    EditorGUI.indentLevel--;
                }
                
            }
            public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            {
                return (float)((property.FindPropertyRelative("IsAnimated").boolValue ? 5.0 : 1.0) * (double)EditorGUIUtility.singleLineHeight + (property.FindPropertyRelative("IsAnimated").boolValue ? 4.0 : 0.0) * (double)EditorGUIUtility.standardVerticalSpacing);
            }
        }
    }
}