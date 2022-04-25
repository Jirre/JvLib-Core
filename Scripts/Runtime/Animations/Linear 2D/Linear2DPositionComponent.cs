using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Animations.Linear2D
{
    [AddComponentMenu("JvLib/Animations/2D/Position")]
    public class Linear2DPositionComponent : MonoBehaviour
    {
        [SerializeField, Tooltip("Direction to move the player in, in Degrees)")] private float _Direction;
        public float Direction
        {
            get => _Direction;
            set
            {
                if (PropertyUtility.SetStruct(ref _Direction, value))
                {
                    SetDirectionVector();
                }
            }
        }
        
        [SerializeField, Tooltip("Movement speed in units per second")] private float _Speed;
        public float Speed
        {
            get => _Speed;
            set => _Speed = value;
        }
        
        private Vector3 _dVector;

        private void Awake() => SetDirectionVector();
        
        private void Update()
        {
            transform.localPosition += _dVector * (_Speed * Time.deltaTime);
        }

        private void SetDirectionVector()
        {
            _dVector = new Vector3(MathUtility.DegCos(_Direction), MathUtility.DegSin(_Direction), 0f);
        }
    }
}
