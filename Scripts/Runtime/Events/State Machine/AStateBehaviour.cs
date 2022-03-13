using UnityEngine;

namespace JvLib.Events
{
    public abstract class AStateBehaviour : MonoBehaviour
    {
        public EventState GetCurrentState() =>
            EventStates?.CurrentEventState;

        public string GetCurrentStateName() =>
            EventStates?.CurrentEventState?.Name ?? "NULL";

        public float GetCurrentStateTime() =>
            EventStates?.CurrentEventState?.GetTimeActive() ?? 0f;

        public virtual void GotoState(string pState) =>
            EventStates.GotoState(pState);

        public virtual void GotoState(EventState pEventState) =>
            EventStates.GotoState(pEventState);

        protected EventStateMachine EventStates;
        public virtual void InitStates()
        {
            EventStates = new EventStateMachine(GetType().Name);
            EventStates.Add("InitState", InitState);
            EventStates.GotoState("InitState");
        }

        protected abstract int InitState(EventState pEventState, float pTime);

        protected virtual void Start() => InitStates();
        protected virtual void Update() => EventStates.Update(Time.time);
    }
}
