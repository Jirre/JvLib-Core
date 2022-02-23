namespace UnityEngine
{
    public static partial class ObjectExtensions
    {
        public static bool IsNull(this Object obj) =>
            ((object)obj) == null;

        public static bool IsNotNull(this Object obj) =>
            ((object)obj) != null;

        public static void SetDirty(this Object obj)
        {
#if  UNITY_EDITOR
            if (Application.isPlaying)
                return;

            if (obj is GameObject gameObject)
            {
                SetGameObjectDirty(gameObject);
                return;
            }

            if (obj is Component component)
            {
                SetComponentDirty(component);
                return;
            }

            UnityEditor.EditorUtility.SetDirty(obj);
#endif
        }

#if  UNITY_EDITOR
        public static void SetGameObjectDirty(GameObject gameObject)
        {
            if(Application.isPlaying)
                return;

            HandlePrefabInstance(gameObject);
            UnityEditor.EditorUtility.SetDirty(gameObject);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
        }

        public static void SetComponentDirty(Component component)
        {
            if(Application.isPlaying)
                return;

            HandlePrefabInstance(component.gameObject);
            UnityEditor.EditorUtility.SetDirty(component);
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(component.gameObject.scene);
        }

        // Some prefab overrides are not handled by Undo.RecordObject or EditorUtility.SetDirty.
        // eg. adding an item to an array/list on a prefab instance updates that the instance
        // has a different array count than the prefab, but not any data about the added thing
        private static void HandlePrefabInstance(GameObject gameObject)
        {
            UnityEditor.PrefabInstanceStatus status = UnityEditor.PrefabUtility.GetPrefabInstanceStatus(gameObject);
            if (status != UnityEditor.PrefabInstanceStatus.NotAPrefab)
            {
                UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(gameObject);
            }
        }
#endif
    }
}
