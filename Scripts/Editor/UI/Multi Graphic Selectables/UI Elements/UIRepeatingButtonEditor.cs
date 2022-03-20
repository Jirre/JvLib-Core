namespace JvLib.UI.Editor
{
    using UnityEditor;
    [CustomEditor(typeof(UIRepeatingButton))]
    public sealed class UIRepeatingButtonEditor : AUISelectableEditor
    {
        public override void InspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_InitialTimeout"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_TickTimeout"));
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnClick"));
        }
    }
}