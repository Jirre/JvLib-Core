using System;
using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Animations.Sway
{
    [RequireComponent(typeof(RectTransform))]
    public class SwayRectOffsetComponent : MonoBehaviour
    {
        [SerializeField] private Vector2 _MinBase;
        [SerializeField] private Vector2 _MaxBase;
        
        [Serializable]
        public struct MovementStep
        {
            public Vector2 _Delta;
            public ESwayMethod _Method;
            public float _Multiplier;
            public float _Offset;
        }

        [SerializeField] private MovementStep[] _MinSteps;
        [SerializeField] private MovementStep[] _MaxSteps;

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
            
            foreach (MovementStep step in _MinSteps)
            {
                if (step._Delta == Vector2.zero ||
                    FloatUtility.Equals(step._Multiplier, 0f, 0.02f))
                    continue;

                ((RectTransform) transform).offsetMin +=
                    step._Delta * SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
        
        private void SwayMax()
        {
            ((RectTransform) transform).offsetMax = _MaxBase;
            if (_MaxSteps == null || _MaxSteps.Length == 0)
                return;
            
            foreach (MovementStep step in _MaxSteps)
            {
                if (step._Delta == Vector2.zero ||
                    FloatUtility.Equals(step._Multiplier, 0f, 0.02f))
                    continue;

                ((RectTransform) transform).offsetMax +=
                    step._Delta * SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
    }
}
