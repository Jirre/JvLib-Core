using UnityEngine;

namespace JvLib.StateMachines
{
    public abstract class AStateBehaviour : MonoBehaviour
    {
        public State GetCurrentState() =>
            States?.CurrentState;

        public string GetCurrentStateName() =>
            States?.CurrentState?.Name ?? "NULL";

        public float GetCurrentStateTime() =>
            States?.CurrentState?.GetTimeActive() ?? 0f;

        public virtual void GotoState(string pState) =>
            States.GotoState(pState);

        public virtual void GotoState(State pState) =>
            States.GotoState(pState);

        protected StateMachine States;
        public virtual void InitStates()
        {
            States = new StateMachine(GetType().Name);
            States.Add("InitState", InitState);
            States.GotoState("InitState");
        }

        protected abstract int InitState(State pState, float pTime);

        protected virtual void Start() => InitStates();
        protected virtual void Update() => States.Update(Time.time);
    }
}
