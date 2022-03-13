using UnityEngine;

namespace JvLib.Events.Editor
{
    using UnityEditor;
    [CustomPropertyDrawer(typeof(EventStateMachine))]
    public class StateMachinePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, new GUIContent(label.text + "; State:"));
            EditorGUI.LabelField(position, property.FindPropertyRelative("_currentState")?.FindPropertyRelative("_name")?.stringValue ?? "Null");
            EditorGUI.EndProperty();
        }
    }
}
