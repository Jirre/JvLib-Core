namespace JvLib.UI.Editor
{
    using UnityEditor;
    [CustomEditor(typeof(UIButton))]
    public sealed class UIButtonEditor : AUISelectableEditor
    {
        public override void InspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_OnClick"));
        }
    }
}
