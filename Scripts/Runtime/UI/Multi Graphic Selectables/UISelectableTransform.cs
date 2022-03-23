using JvLib.Utilities;
using System;
using UnityEngine;

namespace JvLib.UI
{
    [AddComponentMenu("JvLib/UI/Selectable Transform")]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [Serializable]
    public class UISelectableTransform : AUISelectableTarget
    {
        [SerializeField] private Vector3Container _rotation = new Vector3Container(Vector3.zero);
        private Quaternion _originRotation;
        [SerializeField] private Vector3Container _scale = new Vector3Container(Vector3.one);
        private Vector3 _originScale;

        [SerializeField] private RectTransform _targetTransform;

        private AUISelectable.SelectionState _currentState;

        public Vector3Container Rotation { get { return _rotation; } set { if (PropertyUtility.SetStruct(ref _rotation, value)) OnSetProperty(); } }
        public Vector3Container Scale { get { return _scale; } set { if (PropertyUtility.SetStruct(ref _scale, value)) OnSetProperty(); } }

#if UNITY_EDITOR
        // Whether the OnEnable of this Instace has been run already.
        [NonSerialized]
        private bool m_HasEnableRun = false;
#endif

        private void Awake()
        {
            if (_targetTransform == null)
                _targetTransform = GetComponent<RectTransform>();
        }

        // Select on enable.
        private void OnEnable()
        {
            Clear();
            Transition(_currentState, true);
#if UNITY_EDITOR
            m_HasEnableRun = true;
#endif
        }

        private void OnDisable()
        {
            Clear();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // OnValidate can be called before OnEnable, this makes it unsafe to access other components
            // since they might not have been initialized yet.
            // OnSetProperty potentially access Animator or Graphics. (case 618186)
            if (m_HasEnableRun)
            {
                // Need to clear out the override image on the target...
                Clear();

                // And now go to the right state.
                Transition(_currentState, true);
            }
        }

        private void Reset()
        {
            _targetTransform = GetComponent<RectTransform>();
        }
#endif // if UNITY_EDITOR

        private void OnSetProperty()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                Transition(_currentState, true);
            else
#endif
                Transition(_currentState, true);
        }

        internal override void Transition(AUISelectable.SelectionState pState, bool pInstant)
        {
            if (gameObject.activeInHierarchy)
            {
                if (_rotation.IsAnimated)
                {
                    switch (pState)
                    {
                        case AUISelectable.SelectionState.Normal:
                            StartRotationTween(_rotation.Normal, pInstant);
                            break;
                        case AUISelectable.SelectionState.Highlighted:
                            StartRotationTween(_rotation.Highlighted, pInstant);
                            break;
                        case AUISelectable.SelectionState.Pressed:
                            StartRotationTween(_rotation.Pressed, pInstant);
                            break;
                        case AUISelectable.SelectionState.Disabled:
                            StartRotationTween(_rotation.Disabled, pInstant);
                            break;
                    }
                }
                if (_scale.IsAnimated)
                {
                    switch (pState)
                    {
                        case AUISelectable.SelectionState.Normal:
                            StartScaleTween(_scale.Normal, pInstant);
                            break;
                        case AUISelectable.SelectionState.Highlighted:
                            StartScaleTween(_scale.Highlighted, pInstant);
                            break;
                        case AUISelectable.SelectionState.Pressed:
                            StartScaleTween(_scale.Pressed, pInstant);
                            break;
                        case AUISelectable.SelectionState.Disabled:
                            StartScaleTween(_scale.Disabled, pInstant);
                            break;
                    }
                }
            }
        }

        internal override void Clear()
        {
            StartRotationTween(Vector3.zero, true);
            StartScaleTween(Vector3.one, true);
        }

        private void StartRotationTween(Vector3 targetRotation, bool instant)
        {
            if (_targetTransform == null)
                return;

            //TODO JV: Make Tween
            _targetTransform.rotation = Quaternion.Euler(targetRotation);
        }
        private void StartScaleTween(Vector3 targetScale, bool instant)
        {
            if (_targetTransform == null)
                return;

            //TODO JV: Make Tween
            _targetTransform.localScale = targetScale;
        }

        [Serializable]
        public struct Vector3Container
        {
            public bool IsAnimated;
            public Vector3 Normal;
            public Vector3 Highlighted;
            public Vector3 Pressed;
            public Vector3 Disabled;

            public Vector3Container(Vector3 pValue)
            {
                Normal = pValue;
                Highlighted = pValue;
                Pressed = pValue;
                Disabled = pValue;
                IsAnimated = false;
            }
            public Vector3Container(Vector3 pNormal, Vector3 pHighlighted, Vector3 pPressed, Vector3 pDisabled)
            {
                Normal = pNormal;
                Highlighted = pHighlighted;
                Pressed = pPressed;
                Disabled = pDisabled;
                IsAnimated = false;
            }
            public Vector3Container(Vector3 pNormal, Vector3 pHighlighted, Vector3 pPressed, Vector3 pDisabled, bool pIsAnimated)
            {
                Normal = pNormal;
                Highlighted = pHighlighted;
                Pressed = pPressed;
                Disabled = pDisabled;
                IsAnimated = pIsAnimated;
            }
        }
    }
}