using System;
using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Animations.Sway
{
    public class SwayMovementComponent : MonoBehaviour
    {
        [Serializable]
        public struct MovementStep
        {
            public Vector3 _Axis;
            public ESwayMethod _Method;
            public float _Multiplier;
            public float _Offset;
        }

        [Header("LocalPosition = SUM( Axis * Method((Time * Multiplier) + Offset) )")]
        [SerializeField] private MovementStep[] _Steps;
        
        private void Update()
        {
            transform.localPosition = Vector3.zero;
            if (_Steps == null || _Steps.Length == 0)
                return;
            
            foreach (MovementStep step in _Steps)
            {
                if (FloatUtility.Equals(step._Multiplier, 0f, 0.02f))
                    continue;

                transform.localPosition +=
                    step._Axis * SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
    }
}
