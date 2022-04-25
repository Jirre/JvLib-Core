using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Animations.Linear2D
{
    [AddComponentMenu("JvLib/Animations/2D/Rotate")]
    public class Linear2DRotateComponent : MonoBehaviour
    {
        [Header("Rotation += [deltaTime * Mult]")]

        [SerializeField] private float _Multiplier;
        public float Multiplier
        {
            get => _Multiplier;
            set => _Multiplier = value;
        }

        private void Update()
        {
            if (FloatUtility.Equals(_Multiplier, 0f, 0.02f))
            {
                return;
            }
            transform.localEulerAngles += Vector3.forward * (Time.deltaTime * _Multiplier);
        }
    }
}
