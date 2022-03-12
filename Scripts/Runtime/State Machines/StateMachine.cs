using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.StateMachines
{
    public class State
    {
        #region  State Properties

        public string Name { get; }

        public bool IsFistFrame { get; private set; }

        private float _activationTime;
        public float GetTimeActive() => Time.time - _activationTime;
        #endregion

        #region Constructor
        public State(StateMachine pList, string pName, StateDelegate pUpdate, StateDelegate pActivation = null, StateDelegate pDeactivation = null)
        {
            _stateList = pList;
            Name = pName;
            _updateFunction = pUpdate;
            _activateFunction = pActivation;
            _deactivateFunction = pDeactivation;
        }
        //Constructor Variations

        #endregion

        #region Update Functions


        public delegate int StateDelegate(State pCallerState, float pTime);
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

        private readonly StateMachine _stateList;

        /// <summary>
        /// Navigates to the requested state (if it exists)
        /// </summary>
        /// <param name="pNameStr">Name of the state to navigate to</param>
        /// <returns>Was navigating to the given state possible</returns>
        public bool GotoState(string pNameStr) =>
            _stateList?.GotoState(pNameStr) ?? false;

        /// <summary>
        /// Requests the previous state of the parent state list
        /// </summary>
        /// <returns>The previous state found in the state list</returns>
        public State GetPreviousState() =>
            _stateList?.PreviousState;

        /// <summary>
        /// Does the parent have a state with the given name
        /// </summary>
        /// <param name="pName">Name of the state to look for</param>
        /// <returns>Does the requested state exist</returns>
        public bool HasState(string pName) =>
            _stateList?.HasState(pName) ?? false;
        #endregion
    }

    [Serializable]
    public class StateMachine
    {
        private Hashtable _stateTable;
        public int Count => _stateTable?.Count ?? 0;

        public State CurrentState { get; private set; }
        public State PreviousState { get; private set; }

        private string _listName;

        #region Constructor
        public StateMachine(string pName)
        {
            _listName = pName;
            _stateTable = new Hashtable();
        }
        #endregion

        #region State Access
        /// <summary>
        /// Is the current state the same as the given one
        /// </summary>
        /// <param name="pName">The state to check against the current state</param>
        public bool IsCurrentState(string pName) =>
            (!string.IsNullOrEmpty(pName) && CurrentState?.Name == pName);

        /// <summary>
        /// Does the state machine contain a state with the given name
        /// </summary>
        /// <param name="pName">The name of the state to check for</param>
        public bool HasState(string pName) => _stateTable.ContainsKey(pName);
        #endregion

        #region State Registration
        public bool Add(string pName, State.StateDelegate pOnUpdate, State.StateDelegate pOnActivate, State.StateDelegate pOnDeactivate)
        {
            _stateTable ??= new Hashtable();

            if (_stateTable.ContainsKey(pName))
            {
                Debug.LogError("StateList: re-adding state :" + pName);
                return false;
            }
            State lState = new State(this, pName, pOnUpdate, pOnActivate, pOnDeactivate);
            _stateTable.Add(pName, lState);
            return true;
        }
        public bool Add(string pNameStr, State.StateDelegate pUpdate) => Add(pNameStr, pUpdate, null, null);

        public bool Add(State pState)
        {
            if (_stateTable == null) _stateTable = new Hashtable();

            if (pState == null)
            {
                Debug.LogError("StateList: Adding null-state");
                return false;
            }
            if (_stateTable.ContainsKey(pState.Name))
            {
                Debug.LogError("StateList: re-adding state :" + pState.Name);
                return false;
            }

            _stateTable.Add(pState.Name, pState);
            return true;
        }
        #endregion

        #region State Machine Navigation
        /// <summary>
        /// Gets the state within the state list with the given name
        /// </summary>
        /// <param name="pName">Name of the state to return</param>
        public State GetState(string pName)
        {
            if (_stateTable.ContainsKey(pName))
                return (State)_stateTable[pName];
            return null;
        }
        /// <summary>
        /// Returns the state within the state list if it exists
        /// </summary>
        /// <param name="pName">Name of the state to return</param>
        /// <param name="pState">Output of the state if found</param>
        /// <returns>Whether the state exists or not</returns>
        public bool TryGetState(string pName, out State pState)
        {
            if (_stateTable.ContainsKey(pName))
            {
                pState = (State)_stateTable[pName];
                return true;
            }
            pState = null;
            return false;
        }

        /// <summary>
        /// Sets the state of this state machine to the given state
        /// </summary>
        /// <param name="pState">The state to set the state-machine to</param>
        public void GotoState(State pState)
        {
            if (CurrentState == pState) // This is already the current state
                return;

            float currentTime = Time.time;
            if(CurrentState != null)
            {
                PreviousState = CurrentState;
                PreviousState.Deactivate(currentTime);
            }
            CurrentState = pState;
            CurrentState.Activate(currentTime);
        }
        /// <summary>
        /// Sets the state of this state machine to the state with the given name
        /// </summary>
        /// <param name="pName">Name of the state to set the state machine to</param>
        /// <returns>Was the setting of the state successful</returns>
        public bool GotoState(string pName)
        {
            State state = GetState(pName);
            if (HasState(pName))
            {
                GotoState(state);
                return true;
            }
            Debug.LogError("Unknown State:" + pName);
            return false;
        }
        #endregion

        /// <summary>
        /// Updates the current state that is active within this State Machine
        /// </summary>
        /// <param name="pTime">Current time to register</param>
        /// <returns>Return Code of the function</returns>
        public int Update(float pTime)
        {
            if (CurrentState == null) return -2;
            int returnValue = 0;
            try
            {
                returnValue = CurrentState.Update(pTime);
            }
            catch (Exception lException)
            {
                string lMessageStr = "State: " + _listName + "; Exception in Update state:" + CurrentState.Name + ", " + lException;
                Debug.LogError(lMessageStr);
            }
            return returnValue;
        }

        public override string ToString()
        {
            string lStr = $"StateList {_listName} - {_stateTable} - {CurrentState?.Name ?? "null"}\n";
            foreach (KeyValuePair<string, object> e in _stateTable)
            {
                if (e.Value is State s)
                {
                    lStr += $"{s.Name}\n";
                }
            }
            return lStr;
        }
    }
}