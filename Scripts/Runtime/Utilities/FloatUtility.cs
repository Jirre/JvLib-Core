using UnityEngine;

namespace JvLib.Utilities
{
    public static class FloatUtility
    {
        public static bool Equals(float pSource, float pTarget, float pThreshold)
        {
            return Mathf.Abs(pSource - pTarget) <= pThreshold;
        }
    }
}
