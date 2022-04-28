using System;
using UnityEngine;

namespace JvLib.Events
{
    /// <typeparam name="E">Enum Type</typeparam>
    public class EventState<E>
        where E : Enum
    {
        #region State Properties

        public E ID { get; }

        public bool IsFistFrame { get; private set; }

        private float _activationTime;
        public float GetTimeActive() => Time.time - _activationTime;

        #endregion

        #region Constructor

        public EventState(EventStateMachine<E> pList, E pID, StateDelegate pUpdate,
            StateDelegate pActivation = null, StateDelegate pDeactivation = null)
        {
            _eventStateList = pList;
            ID = pID;
            _updateFunction = pUpdate;
            _activateFunction = pActivation;
            _deactivateFunction = pDeactivation;
        }
        //Constructor Variations

        #endregion

        #region Update Functions

        public delegate int StateDelegate(EventState<E> pCallerEventState, float pTime);

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

        private readonly EventStateMachine<E> _eventStateList;

        /// <summary>
        /// Navigates to the requested state (if it exists)
        /// </summary>
        /// <param name="pStateID">ID of the state to navigate to</param>
        /// <returns>Was navigating to the given state possible</returns>
        public bool GotoState(E pStateID) =>
            _eventStateList?.GotoState(pStateID) ?? false;

        /// <summary>
        /// Requests the previous state of the parent state list
        /// </summary>
        /// <returns>The previous state found in the state list</returns>
        public EventState<E> GetPreviousState() =>
            _eventStateList?.PreviousEventState;

        /// <summary>
        /// Does the parent have a state with the given name
        /// </summary>
        /// <param name="pStateID">ID of the state to look for</param>
        /// <returns>Does the requested state exist</returns>
        public bool HasState(E pStateID) =>
            _eventStateList?.HasState(pStateID) ?? false;

        #endregion
    }
}

