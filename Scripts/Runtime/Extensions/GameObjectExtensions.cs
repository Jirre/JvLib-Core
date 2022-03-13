namespace UnityEngine
{
    public static partial class GameObjectExtensions
    {
        /// <summary>
        /// Check if an object is a prefab.
        /// </summary>
        public static bool IsPrefab(this GameObject gameObject)
        {
            return !gameObject.scene.IsValid();
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject)
            where T : Component
        {
            T c = gameObject.GetComponent<T>();
            if (c == null)
                c = gameObject.AddComponent<T>();
            return c;
        }
    }
}
