using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JvLib.UI
{
    [AddComponentMenu("JvLib/UI/Buttons/Repeating Button")]
    [DisallowMultipleComponent]
    public class UIRepeatingButton : AUISelectable, IPointerDownHandler, IPointerUpHandler
    {
        private bool _pressed = false;

        [SerializeField] private float _InitialTimeout = 0.5f;
        [SerializeField] private float _TickTimeout = 0.125f;
        private float _timer;

        #region Event Triggers
        public override void OnPointerDown(PointerEventData data)
        {
            base.OnPointerDown(data);
            //Designate that on release and no drag to still invoke the event
            _pressed = true;
            _timer = _InitialTimeout;

            Press();
        }
        public override void OnPointerUp(PointerEventData data)
        {
            base.OnPointerUp(data);
            //Reset all pressing values
            _pressed = false;
        }

        private void Update()
        {
            if (_pressed)
            {
                if (!interactable)
                {
#if UNITY_EDITOR
                    RedrawTargetGraphics();
#endif
                    _pressed = false;
                }
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _timer += _TickTimeout;
                    Press();
                }
            }
        }
        #endregion

        #region On Click Event
        [Serializable] public class ButtonClickEvent : UnityEvent { }

        [SerializeField] private ButtonClickEvent _OnClick = new ButtonClickEvent();

        public ButtonClickEvent OnClick
        {
            get { return _OnClick; }
            set { _OnClick = value; }
        }
        private void Press()
        {
            if (!IsActive() || !IsInteractable()) return;
            _OnClick.Invoke();
        }
        #endregion
    }
}