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
        
        /// <summary>
        ///   <para>Is this game object tagged with layer mask ?</para>
        /// </summary>
        /// <param name="layerMask">The layer mask to compare.</param>
        public static bool CompareLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return (layerMask.value & (1 << gameObject.layer)) > 0;
        }
    }
}
