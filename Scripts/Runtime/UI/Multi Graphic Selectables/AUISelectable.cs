using JvLib.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JvLib.UI
{
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    [DisallowMultipleComponent]
    [Serializable]
    public class AUISelectable : UIBehaviour,
        IPointerDownHandler, IPointerUpHandler,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler, IEventSystemHandler
    {
        #region Fields (Serialized)
        [Tooltip("Can the Selectable be interacted with?")]
        [SerializeField]
        private bool _interactive = true;
        public bool interactable { get { return _interactive; } set { if (PropertyUtility.SetStruct(ref _interactive, value)) OnSetProperty(); } }
        public virtual bool IsInteractable()
        {
            return _canvasGroupAllowsInteraction && _interactive;
        }

        [Tooltip("The list of SelectableTargets to influence")]
        [SerializeField]
        private List<AUISelectableTarget> _selectableTargets;
        // Get the animator
        public Animator Animator
        {
            get { return GetComponent<Animator>(); }
        }
        #endregion

        #region Properties (Input)
        private bool _isPointerInside { get; set; }
        private bool _isPointerDown { get; set; }
        private bool _hasSelection { get; set; }
        #endregion

        #region Enums
        public enum SelectionState
        {
            Normal,
            Highlighted,
            Pressed,
            Disabled
        }
        #endregion

        #region Methods (Unity and Validation)
        protected override void Awake()
        {
            if (_selectableTargets == null)
            {
                _selectableTargets = new List<AUISelectableTarget>();
                if (GetComponentsInChildren<AUISelectableTarget>() != null && GetComponentsInChildren<AUISelectableTarget>().Length > 0)
                    _selectableTargets.AddRange(GetComponentsInChildren<AUISelectableTarget>());
            }
        }

        /// <summary>
        /// Canvas Group Interaction
        /// </summary>
        private bool _canvasGroupAllowsInteraction = true;
        private readonly List<CanvasGroup> m_CanvasGroupCache = new List<CanvasGroup>();
        protected override void OnCanvasGroupChanged()
        {
            // Figure out if parent groups allow interaction
            // If no interaction is alowed... then we need
            // to not do that :)
            bool groupAllowInteraction = true;
            Transform t = transform;
            while (t != null)
            {
                t.GetComponents(m_CanvasGroupCache);
                for (int i = 0; i < m_CanvasGroupCache.Count; i++)
                {
                    if (!m_CanvasGroupCache[i].interactable)
                    {
                        groupAllowInteraction = false;
                        break;
                    }
                    if (m_CanvasGroupCache[i].ignoreParentGroups)
                        break;
                }
                t = t.parent;
            }

            if (groupAllowInteraction != _canvasGroupAllowsInteraction)
            {
                _canvasGroupAllowsInteraction = groupAllowInteraction;
                OnSetProperty();
            }
        }

        // Call from unity if animation properties have changed
        protected override void OnDidApplyAnimationProperties()
        {
            OnSetProperty();
        }

#if UNITY_EDITOR
        // Whether the OnEnable of this Instace has been run already.
        [NonSerialized]
        private bool m_HasEnableRun = false;
#endif
        // Select on enable.
        protected override void OnEnable()
        {
            base.OnEnable();

            SelectionState state = SelectionState.Normal;

            // The button will be highlighted even in some cases where it shouldn't.
            // For example: We only want to set the State as Highlighted if the StandaloneInputModule.m_CurrentInputMode == InputMode.Buttons
            // But we dont have access to this, and it might not apply to other InputModules.
            // TODO: figure out how to solve this. Case 617348.
            if (_hasSelection)
                state = SelectionState.Highlighted;

            _currentState = state;
            EvaluateAndTransition(true);

#if UNITY_EDITOR
            m_HasEnableRun = true;
#endif
        }

        protected override void OnDisable()
        {
            ClearGraphics();
            base.OnDisable();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            // OnValidate can be called before OnEnable, this makes it unsafe to access other components
            // since they might not have been initialized yet.
            // OnSetProperty potentially access Animator or Graphics. (case 618186)
            if (m_HasEnableRun)
            {
                // Need to clear out the override on the targets...
                ClearGraphics();

                // And now go to the right state.
                EvaluateAndTransition(true);
            }
        }

        public void RedrawTargetGraphics()
        {
            // OnValidate can be called before OnEnable, this makes it unsafe to access other components
            // since they might not have been initialized yet.
            // OnSetProperty potentially access Animator or Graphics. (case 618186)
            if (m_HasEnableRun)
            {
                // Need to clear out the override on the targets...
                ClearGraphics();

                // And now go to the right state.
                EvaluateAndTransition(true);
            }
        }
#endif // if UNITY_EDITOR

        private void OnSetProperty()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
                EvaluateAndTransition(true);
            else
#endif
                EvaluateAndTransition(false);
        }
        #endregion

        #region Methods (State Transitions)
        private SelectionState _currentState;
        public SelectionState CurrentSelectionState { get { return _currentState; } }

        /// <summary>
        /// Change the selectable to the correct state
        /// </summary>
        /// <param name="eventData">Information of the current Event</param>
        private void EvaluateAndTransition(BaseEventData eventData)
        {
            if (!IsActive())
                return;

            UpdateSelectionState(eventData);
            EvaluateAndTransition(false);
        }
        /// <summary>
        /// Change the selectable to the correct state
        /// </summary>
        /// <param name="instant">Should the event happen instantanious</param>
        private void EvaluateAndTransition(bool instant)
        {
            SelectionState transitionState = _currentState;
            if (IsActive() && !IsInteractable())
                transitionState = SelectionState.Disabled;
            SetGraphicState(transitionState, instant);
        }


        // The current visual state of the control.
        protected void UpdateSelectionState(BaseEventData eventData)
        {
            if (IsPressed())
                _currentState = SelectionState.Pressed;
            else if (IsHighlighted(eventData))
                _currentState = SelectionState.Highlighted;
            else _currentState = SelectionState.Normal;
        }

        // Whether the control should be 'selected'.
        protected bool IsHighlighted(BaseEventData eventData)
        {
            if (!IsActive())
                return false;

            if (IsPressed())
                return false;

            bool selected = _hasSelection;
            if (eventData is PointerEventData)
            {
                PointerEventData pointerData = eventData as PointerEventData;
                selected |=
                    (_isPointerDown && !_isPointerInside && pointerData.pointerPress == gameObject) // This object pressed, but pointer moved off
                    || (!_isPointerDown && _isPointerInside && pointerData.pointerPress == gameObject) // This object pressed, but pointer released over (PointerUp event)
                    || (!_isPointerDown && _isPointerInside && pointerData.pointerPress == null); // Nothing pressed, but pointer is over
            }
            else
            {
                selected |= _isPointerInside;
            }
            return selected;
        }

        [Obsolete("Is Pressed no longer requires eventData", false)]
        protected bool IsPressed(BaseEventData eventData)
        {
            return IsPressed();
        }

        // Whether the control should be pressed.
        protected bool IsPressed()
        {
            if (!IsActive())
                return false;

            return _isPointerInside && _isPointerDown;
        }
        #endregion
        
        #region Methods (Graphics)
        protected virtual void SetGraphicState(SelectionState pState, bool pInstant)
        {
            if ((_selectableTargets?.Count ?? 0) > 0) foreach (AUISelectableTarget g in _selectableTargets)
                    if (g != null) g.Transition(pState, pInstant);
        }
        protected void ClearGraphics()
        {
            if ((_selectableTargets?.Count ?? 0) > 0) foreach (AUISelectableTarget g in _selectableTargets)
                    if (g != null) g.Clear();
        }
        #endregion

        #region Interaction Methods (Interface)
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            // Selection tracking
            if (IsActive() && !IsInteractable())
                EventSystem.current.SetSelectedGameObject(gameObject, eventData);

            _isPointerDown = true;
            EvaluateAndTransition(eventData);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            _isPointerDown = false;
            EvaluateAndTransition(eventData);
        }

        public virtual void OnPointerEnter(PointerEventData eventData)
        {
            _isPointerInside = true;
            EvaluateAndTransition(eventData);
        }

        public virtual void OnPointerExit(PointerEventData eventData)
        {
            _isPointerInside = false;
            EvaluateAndTransition(eventData);
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            _hasSelection = true;
            EvaluateAndTransition(eventData);
        }

        public virtual void OnDeselect(BaseEventData eventData)
        {
            _hasSelection = false;
            EvaluateAndTransition(eventData);
        }

        public virtual void Select()
        {
            if (EventSystem.current.alreadySelecting)
                return;

            EventSystem.current.SetSelectedGameObject(gameObject);
        }
        #endregion
    }
}  