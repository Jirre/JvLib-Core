using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace JvLib.UI
{
    [AddComponentMenu("JvLib/UI/Button")]
    public class UIButton : AUISelectable, IPointerClickHandler
    {
        [Serializable] public class ButtonClickEvent : UnityEvent { }

        [SerializeField] protected ButtonClickEvent _OnClick = new ButtonClickEvent();
        
        public ButtonClickEvent OnClick
        {
            get { return _OnClick; }
            set { _OnClick = value; }
        }

        private void Press()
        {
            if (!IsActive() || !IsInteractable()) return;
            if (_OnClick != null) _OnClick.Invoke();
        }

        // Trigger all registered callbacks.
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }
    }
}
