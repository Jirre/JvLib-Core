using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Animations.Linear2D
{
    [AddComponentMenu("JvLib/Animations/2D/Rotation")]
    public class Linear2DRotationComponent : MonoBehaviour
    {
        [Header("Rotation: [time * Mult + Offset]")]

        [SerializeField] private float _Multiplier;
        public float Multiplier
        {
            get => _Multiplier;
            set => _Multiplier = value;
        }

        [SerializeField] private float _Offset;
        public float Offset
        {
            get => _Offset;
            set => _Offset = value;
        }
        
        private void Update()
        {
            if (FloatUtility.Equals(_Multiplier, 0f, 0.02f))
            {
                transform.localEulerAngles = Vector3.zero;
                return;
            }
            transform.localEulerAngles = new Vector3(0, 0, Time.time * _Multiplier + _Offset);
        }
    }
}
