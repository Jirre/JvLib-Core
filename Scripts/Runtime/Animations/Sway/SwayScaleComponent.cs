using UnityEngine;

namespace JvLib.Animations.Sway
{
    [AddComponentMenu("JvLib/Animations/Sway/Scale")]
    public class SwayScaleComponent : MonoBehaviour
    {
        [SerializeField] private Vector3 _BaseScale;

        public Vector3 BaseScale
        {
            get => _BaseScale;
            set => _BaseScale = value;
        }
        [SerializeField] private float _BaseMultiplier;

        public float BaseMultiplier
        {
            get => _BaseMultiplier;
            set => _BaseMultiplier = value;
        }

        [Header("LocalScale = BaseScale + BaseMultiplier * SUM( Axis * Method((Time * Multiplier) + Offset) )")]
        [SerializeField] private SwayStep<Vector3>[] _Steps;

        private void Update()
        {
            transform.localScale = _BaseScale;
            if (_Steps == null || _Steps.Length == 0 || _BaseMultiplier == 0f)
                return;
            
            foreach (SwayStep<Vector3> step in _Steps)
            {
                transform.localScale +=
                    _BaseMultiplier * 
                    step._Value * 
                    SwayMethod.Solve(step._Method, Time.time * step._Multiplier + step._Offset);
            }
        }
    }
}
