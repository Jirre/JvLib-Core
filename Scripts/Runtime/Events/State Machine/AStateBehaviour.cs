using System;
using UnityEngine;

namespace JvLib.Events
{
    /// <typeparam name="E">Enum Type</typeparam>
    public abstract class AStateBehaviour<E> : MonoBehaviour
        where E : Enum
    {
        public EventState<E> GetCurrentState() =>
            EventStates.CurrentEventState;

        public E GetCurrentStateID() =>
            EventStates.CurrentEventState.ID;

        public float GetCurrentStateTime() =>
            EventStates?.CurrentEventState?.GetRealLifeTime() ?? 0f;

        public virtual void GotoState(E pStateID) =>
            EventStates.GotoState(pStateID);

        public virtual void GotoState(EventState<E> pEventState) =>
            EventStates.GotoState(pEventState);

        protected EventStateMachine<E> EventStates;
        public abstract void InitStates();
        
        protected virtual void Start()
        {
            EventStates = new EventStateMachine<E>(GetType().Name);
            InitStates();
        }
            
        protected virtual void Update() => EventStates.Update();
    }
}
