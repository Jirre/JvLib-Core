namespace JvLib.UI.Editor
{
    using UnityEditor;
    [CustomEditor(typeof(UITimedButton))]
    public class RxUITimedButtonEditor : AUISelectableEditor
    {
        public override void InspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_Timeout"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnClick"));
        }
    }
}
