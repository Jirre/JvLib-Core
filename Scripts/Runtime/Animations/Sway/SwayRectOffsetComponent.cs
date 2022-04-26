using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Animations.Sway
{
    [AddComponentMenu("JvLib/Animations/Sway/Rect Offset")]
    [RequireComponent(typeof(RectTransform))]
    public class SwayRectOffsetComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 _MinBase;
        [SerializeField] private Vector2 _MaxBase;

        [SerializeField] private SwayStep<Vector2>[] _MinSteps;
        [SerializeField] private SwayStep<Vector2>[] _MaxSteps;

        private void Update()
        {
            SwayMin();
            SwayMax();
        }

        private void SwayMin()
        {
            ((RectTransform) transform).offsetMin = _MinBase;
            if (_MinSteps == null || _MinSteps.Length == 0)
                return;
            
            foreach (SwayStep<Vector2> step in _MinSteps)
            {
                if (step._Value == Vector2.zero ||
                    FloatUtility.Equals(step._Multiplier, 0f, 0.02f))
                    continue;

                ((RectTransform) transform).offsetMin +=
                    step._Value * SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
        
        private void SwayMax()
        {
            ((RectTransform) transform).offsetMax = _MaxBase;
            if (_MaxSteps == null || _MaxSteps.Length == 0)
                return;
            
            foreach (SwayStep<Vector2> step in _MaxSteps)
            {
                if (step._Value == Vector2.zero ||
                    FloatUtility.Equals(step._Multiplier, 0f, 0.02f))
                    continue;

                ((RectTransform) transform).offsetMax +=
                    step._Value * SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
    }
}
