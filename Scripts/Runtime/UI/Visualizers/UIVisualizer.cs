using System;
using JvLib.Events;
using UnityEngine;

namespace JvLib.UI.Visualizers
{
    /// <typeparam name="C">Context Type</typeparam>
    public abstract class UIVisualizer<C> : MonoBehaviour
    {
        protected C Context { get; private set; }

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
            Context = pContext;
            _onContextChange.Dispatch();
        }

        protected abstract void OnContextUpdate(C pContext);
    }
}
