using UnityEngine;

namespace JvLib.Events
{
    public class EventState
    {
        #region State Properties

        public string Name { get; }

        public bool IsFistFrame { get; private set; }

        private float _activationTime;
        public float GetTimeActive() => Time.time - _activationTime;

        #endregion

        #region Constructor

        public EventState(EventStateMachine pList, string pName, StateDelegate pUpdate,
            StateDelegate pActivation = null, StateDelegate pDeactivation = null)
        {
            _eventStateList = pList;
            Name = pName;
            _updateFunction = pUpdate;
            _activateFunction = pActivation;
            _deactivateFunction = pDeactivation;
        }
        //Constructor Variations

        #endregion

        #region Update Functions

        public delegate int StateDelegate(EventState pCallerEventState, float pTime);

        private readonly StateDelegate _activateFunction;
        private readonly StateDelegate _updateFunction;
        private readonly StateDelegate _deactivateFunction;

        /// <summary>
        /// Calls the internal Update Function related to this state
        /// </summary>
        /// <param name="pTime">Current Time to ascertain lifetime</param>
        /// <returns>Return Code (-1 = internal Error)</returns>
        public virtual int Update(float pTime)
        {
            int result = -1;
            if (_updateFunction != null)
                result = _updateFunction(this, pTime);

            IsFistFrame = false;
            return result;
        }

        /// <summary>
        /// Calls the internal Activation Function related to this state
        /// </summary>
        /// <param name="pTime">Current Time to ascertain lifetime</param>
        /// <returns>Return Code (-1 = internal Error)</returns>
        public virtual int Activate(float pTime)
        {
            IsFistFrame = true;
            _activationTime = pTime;
            if (_activateFunction != null)
            {
                int result = _activateFunction(this, pTime);
                return result;
            }

            return -1;
        }

        /// <summary>
        /// Calls the internal Deactivation Function related to this state
        /// </summary>
        /// <param name="pTime">Current Time to ascertain lifetime</param>
        /// <returns>Return Code (-1 = internal Error)</returns>
        public virtual int Deactivate(float pTime)
        {
            if (_deactivateFunction != null)
            {
                int result = _deactivateFunction(this, pTime);
                return result;
            }

            return -1;
        }

        #endregion

        #region State Navigation

        private readonly EventStateMachine _eventStateList;

        /// <summary>
        /// Navigates to the requested state (if it exists)
        /// </summary>
        /// <param name="pNameStr">Name of the state to navigate to</param>
        /// <returns>Was navigating to the given state possible</returns>
        public bool GotoState(string pNameStr) =>
            _eventStateList?.GotoState(pNameStr) ?? false;

        /// <summary>
        /// Requests the previous state of the parent state list
        /// </summary>
        /// <returns>The previous state found in the state list</returns>
        public EventState GetPreviousState() =>
            _eventStateList?.PreviousEventState;

        /// <summary>
        /// Does the parent have a state with the given name
        /// </summary>
        /// <param name="pName">Name of the state to look for</param>
        /// <returns>Does the requested state exist</returns>
        public bool HasState(string pName) =>
            _eventStateList?.HasState(pName) ?? false;

        #endregion
    }
}

