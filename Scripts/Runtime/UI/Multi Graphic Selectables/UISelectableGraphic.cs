using System;
using UnityEngine;
using UnityEngine.UI;
using JvLib.Utilities;

namespace JvLib.UI
{
    [AddComponentMenu("JvLib/UI/Selectable Graphic")]
    [ExecuteInEditMode]
    [RequireComponent(typeof(Graphic))]
    [DisallowMultipleComponent]
    [Serializable]
    public class UISelectableGraphic : AUISelectableTarget
    {
        // Highlighting Style
        public enum TransitionStyle
        {
            ColorTint,
            SpriteSwap,
            Animation
        }

        [SerializeField] private TransitionStyle _style = TransitionStyle.ColorTint;

        [SerializeField] private ColorBlock _colors = ColorBlock.defaultColorBlock;
        [SerializeField] private SpriteState _sprites;
        [SerializeField] private AnimationTriggers _animations = new AnimationTriggers();

        [SerializeField] private Graphic _targetGraphic;

        private AUISelectable.SelectionState _currentState;

#if UNITY_EDITOR
        // Whether the OnEnable of this Instace has been run already.
        [NonSerialized]
        private bool m_HasEnableRun = false;
#endif

        public TransitionStyle Style { get { return _style; } set { if (PropertyUtility.SetStruct(ref _style, value)) OnSetProperty(); } }
        public ColorBlock Colors { get { return _colors; } set { if (PropertyUtility.SetStruct(ref _colors, value)) OnSetProperty(); } }
        public SpriteState Sprites { get { return _sprites; } set { if (PropertyUtility.SetStruct(ref _sprites, value)) OnSetProperty(); } }
        public AnimationTriggers AnimationTriggers { get { return _animations; } set { if (PropertyUtility.SetClass(ref _animations, value)) OnSetProperty(); } }
        public Graphic targetGraphic { get { return _targetGraphic; } set { if (PropertyUtility.SetClass(ref _targetGraphic, value)) OnSetProperty(); } }

        // Convenience function that converts the Graphic to a Image, if possible
        public Image Image
        {
            get { return _targetGraphic as Image; }
            set { _targetGraphic = value; }
        }

        // Get the animator
        public Animator animator
        {
            get { return GetComponent<Animator>(); }
        }

        private void Awake()
        {
            if (_targetGraphic == null)
                _targetGraphic = GetComponent<Graphic>();
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
            _colors.fadeDuration = Mathf.Max(_colors.fadeDuration, 0.0f);

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
            _targetGraphic = GetComponent<Graphic>();
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
            Color tintColor;
            Sprite transitionSprite;
            string triggerName;

            switch (pState)
            {
                case AUISelectable.SelectionState.Normal:
                    tintColor = _colors.normalColor;
                    transitionSprite = null;
                    triggerName = _animations.normalTrigger;
                    break;
                case AUISelectable.SelectionState.Highlighted:
                    tintColor = _colors.highlightedColor;
                    transitionSprite = _sprites.highlightedSprite;
                    triggerName = _animations.highlightedTrigger;
                    break;
                case AUISelectable.SelectionState.Pressed:
                    tintColor = _colors.pressedColor;
                    transitionSprite = _sprites.pressedSprite;
                    triggerName = _animations.pressedTrigger;
                    break;
                case AUISelectable.SelectionState.Disabled:
                    tintColor = _colors.disabledColor;
                    transitionSprite = _sprites.disabledSprite; ;
                    triggerName = _animations.disabledTrigger;
                    break;
                default:
                    tintColor = Color.black;
                    transitionSprite = null;
                    triggerName = string.Empty;
                    break;
            }

            if (gameObject.activeInHierarchy)
            {
                switch (_style)
                {
                    case TransitionStyle.ColorTint:
                        StartColorTween(tintColor * _colors.colorMultiplier, pInstant);
                        break;
                    case TransitionStyle.SpriteSwap:
                        DoSpriteSwap(transitionSprite);
                        break;
                    case TransitionStyle.Animation:
                        TriggerAnimation(triggerName);
                        break;
                }
            }
        }
        internal override void Clear()
        {
            // Need to clear out the override image on the target...
            DoSpriteSwap(null);

            // If the transition mode got changed, we need to clear all the transitions, since we don't know what the old transition mode was.
            StartColorTween(Color.white, true);
            TriggerAnimation(_animations.normalTrigger);
        }
        private void StartColorTween(Color targetColor, bool instant)
        {
            if (_targetGraphic == null)
                return;

            _targetGraphic.CrossFadeColor(targetColor, instant ? 0f : _colors.fadeDuration, true, true);
        }
        private void DoSpriteSwap(Sprite newSprite)
        {
            if (Image == null)
                return;

            Image.overrideSprite = newSprite;
        }
        private void TriggerAnimation(string triggername)
        {
            if (animator == null || !animator.enabled || animator.runtimeAnimatorController == null || string.IsNullOrEmpty(triggername))
                return;

            animator.ResetTrigger(_animations.normalTrigger);
            animator.ResetTrigger(_animations.pressedTrigger);
            animator.ResetTrigger(_animations.highlightedTrigger);
            animator.ResetTrigger(_animations.disabledTrigger);
            animator.SetTrigger(triggername);
        }
    }
}