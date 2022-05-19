namespace UnityEngine
{
    public static class LayerMaskExtensions
    {
        /// <summary>
        ///   <para>Is this game object tagged with layer mask ?</para>
        /// </summary>
        /// <param name="layerMask">The layer mask to compare.</param>
        public static bool CompareLayer(this GameObject gameObject, LayerMask layerMask)
        {
            return (layerMask.value & (1 << gameObject.layer)) > 0;
        }

        /// <summary>
        ///   <para>Is this game object tagged with layer mask ?</para>
        /// </summary>
        /// <param name="layerMask">The layer mask to compare.</param>
        public static bool CompareLayer(this Transform transform, LayerMask layerMask) =>
            transform.gameObject.CompareLayer(layerMask);
        
        /// <summary>
        ///   <para>Is this game object tagged with layer mask ?</para>
        /// </summary>
        /// <param name="layerMask">The layer mask to compare.</param>
        public static bool CompareLayer(this Collider collider, LayerMask layerMask) =>
            collider.gameObject.CompareLayer(layerMask);
        
        /// <summary>
        ///   <para>Is this game object tagged with layer mask ?</para>
        /// </summary>
        /// <param name="layerMask">The layer mask to compare.</param>
        public static bool CompareLayer(this Collider2D collider2D, LayerMask layerMask) =>
            collider2D.gameObject.CompareLayer(layerMask);
    }
}

