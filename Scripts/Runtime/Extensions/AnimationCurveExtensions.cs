namespace UnityEngine
{
    public static class AnimationCurveExtensions
    {
        public static float GetDuration(this AnimationCurve pTarget)
        {
            return pTarget.keys.Length > 0 ? pTarget.keys[pTarget.keys.Length - 1].time : 0f;
        }

        public static float Evaluate01(this AnimationCurve pTarget, float pTime)
        {
            return pTarget.keys.Length > 0 ? 
                pTarget.Evaluate(Mathf.Clamp01(pTime) * pTarget.keys[pTarget.keys.Length - 1].time) : 0f;
        }
    }
}
