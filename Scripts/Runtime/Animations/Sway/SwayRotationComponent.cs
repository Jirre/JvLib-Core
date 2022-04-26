using UnityEngine;

namespace JvLib.Animations.Sway
{
    [AddComponentMenu("JvLib/Animations/Sway/Rotation")]
    public class SwayRotationComponent : MonoBehaviour
    {
        [Header("LocalEulerAngles = SUM( Axis * Method((Time * Multiplier) + Offset) )")]
        [SerializeField] private SwayStep<Vector3>[] _Steps;
        
        private void Update()
        {
            transform.localEulerAngles = Vector3.zero;
            if (_Steps == null || _Steps.Length == 0)
                return;
            
            foreach (SwayStep<Vector3> step in _Steps)
            {
                transform.localEulerAngles +=
                    step._Value * SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
    }
}
