using System;
using UnityEngine;

namespace JvLib.Animations.Sway
{
    /// <typeparam name="V">Value Type</typeparam>
    [Serializable]
    public struct SwayStep<V>
    {
        public V _Value;
        public ESwayMethod _Method;
        public float _Multiplier;
        public float _Offset;
    }
}
