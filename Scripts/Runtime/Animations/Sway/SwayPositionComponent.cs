using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Animations.Sway
{
    [AddComponentMenu("JvLib/Animations/Sway/Position")]
    public class SwayPositionComponent : MonoBehaviour
    {

        [Header("LocalPosition = SUM( Axis * Method((Time * Multiplier) + Offset) )")]
        [SerializeField] private SwayStep<Vector3>[] _Steps;
        
        private void Update()
        {
            transform.localPosition = Vector3.zero;
            if (_Steps == null || _Steps.Length == 0)
                return;
            
            foreach (SwayStep<Vector3> step in _Steps)
            {
                if (FloatUtility.Equals(step._Multiplier, 0f, 0.02f))
                    continue;

                transform.localPosition +=
                    step._Value * SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
    }
}
