using System;
using JvLib.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JvLib.UI.Visualizers
{
    /// <typeparam name="C">Context Type</typeparam>
    public abstract class UIVisualizer<C> : UIBehaviour
    {
        [SerializeField]
        private C _Context;

        protected override void Awake()
        {
            base.Awake();
            if (_Context != null)
                OnContextUpdate(_Context);
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if (_Context != null)
                OnContextUpdate(_Context);
        }
#endif
        protected virtual C Context
        {
            get => _Context;
            private set => SetContext(value);
        }

        private SafeEvent _onContextChange = new SafeEvent();
        public event Action OnContextChange
        {
            add => _onContextChange += value;
            remove => _onContextChange -= value;
        }

        public void SetContext(C pContext)
        {
            if (Context == null && pContext == null
                || Context != null && Context.Equals(pContext))
                return;
            _Context = pContext;
            OnContextUpdate(pContext);
            _onContextChange.Dispatch();
        }

        protected abstract void OnContextUpdate(C pContext);
    }
}
