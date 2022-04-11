using System;
using UnityEngine;

namespace JvLib.Animations.Sway
{
    public class SwayRotationComponent : MonoBehaviour
    {
        [Serializable]
        public struct MovementStep
        {
            public Vector3 _Axis;
            public ESwayMethod _Method;
            public float _Multiplier;
            public float _Offset;
        }

        [Header("LocalEulerAngles = SUM( Axis * Method((Time * Multiplier) + Offset) )")]
        [SerializeField] private MovementStep[] _Steps;
        
        private void Update()
        {
            transform.localEulerAngles = Vector3.zero;
            if (_Steps == null || _Steps.Length == 0)
                return;
            
            foreach (MovementStep step in _Steps)
            {
                transform.localEulerAngles +=
                    step._Axis * SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
    }
}
