using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.StateMachines
{
    public class State
    {
        #region  State Properties
        private string _name = "unknown";
        public string Name { get => _name; }

        private bool _firstFrame = false;
        public bool IsFistFrame { get => _firstFrame; }

        private float _activationTime = 0.0f;
        public float GetTimeActive() => Time.time - _activationTime;
        #endregion

        #region Constructor
        public State(StateMachine pList, string pName, StateDelegate pUpdate, StateDelegate pActivation, StateDelegate pDeactivation)
        {
            _stateList = pList;
            _name = pName;
            _updateFunction = pUpdate;
            _activateFunction = pActivation;
            _deactivateFunction = pDeactivation;
        }
        //Constructor Variations
        public State(StateMachine pList, string pName, StateDelegate pUpdate)
            : this(pList, pName, pUpdate, null, null) { }
        #endregion

        #region Update Functions


        public delegate int StateDelegate(State pCallerState, float pTime);
        private StateDelegate _activateFunction = null;
        private StateDelegate _updateFunction = null;
        private StateDelegate _deactivateFunction = null;

        /// <summary>
        /// Calls the internal Update Function related to this state
        /// </summary>
        /// <param name="pTime">Curent Time to ascertain lifetime</param>
        /// <returns>Return Code (-1 = internal Error)</returns>
        public virtual int Update(float pTime)
        {
            int result = -1;
            if (_updateFunction != null)
                result = _updateFunction(this, pTime);

            _firstFrame = false;
            return result;
        }

        /// <summary>
        /// Calls the internal Activation Function related to this state
        /// </summary>
        /// <param name="pTime">Curent Time to ascertain lifetime</param>
        /// <returns>Return Code (-1 = internal Error)</returns>
        public virtual int Activate(float pTime)
        {
            _firstFrame = true;
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
        /// <param name="pTime">Curent Time to ascertain lifetime</param>
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
        public StateMachine _stateList = null;
        public StateMachine Parent { get => _stateList; }

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
        public int Count { get => _stateTable?.Count ?? 0; }

        private State _currentState = null;
        public State CurrentState { get => _currentState; }

        private State _previousState = null;
        public State PreviousState { get => _previousState; }

        private string _listName = "unknown";

        #region Constructor
        public StateMachine(string pName)
        {
            _listName = pName;
            if (_stateTable != null)
                _stateTable.Clear();
            else _stateTable = new Hashtable();
        }
        #endregion

        #region State Access
        /// <summary>
        /// Is the current state the same as the given one
        /// </summary>
        /// <param name="pName">The state to check against the current state</param>
        public bool IsCurrentState(string pName) =>
            (!string.IsNullOrEmpty(pName) && _currentState?.Name == pName);

        /// <summary>
        /// Does the state machine contain a state with the given name
        /// </summary>
        /// <param name="pName">The name of the state to check for</param>
        public bool HasState(string pName) => _stateTable.ContainsKey(pName);
        #endregion

        #region State Registration
        public bool Add(string pName, State.StateDelegate pOnUpdate, State.StateDelegate pOnActivate, State.StateDelegate pOnDeactivate)
        {
            if (_stateTable == null) _stateTable = new Hashtable();

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
        /// <param name="pState">The state to set the statemachine to</param>
        public void GotoState(State pState)
        {
            if (_currentState == pState) // This is already the current state
                return;

            float currentTime = Time.time;
            if(_currentState != null)
            {
                _previousState = _currentState;
                _previousState.Deactivate(currentTime);
            }
            _currentState = pState;
            _currentState.Activate(currentTime);
        }
        /// <summary>
        /// Sets the state of this state machine to the state with the given name
        /// </summary>
        /// <param name="pName">Name of the state to set the state machine to</param>
        /// <returns>Was the setting of the state succesfull</returns>
        public bool GotoState(string pName)
        {
            State lState = GetState(pName);
            if (TryGetState(pName, out State state))
            {
                GotoState(lState);
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
            if (_currentState != null)
            {
                int returnValue = 0;
                try
                {
                    returnValue = _currentState.Update(pTime);
                }
                catch (Exception lException)
                {
                    string lMessageStr = "State: " + _listName + "; Exception in Update state:" + _currentState.Name + ", " + lException.ToString();
                    Debug.LogError(lMessageStr);
                }
                return returnValue;
            }
            return -2;
        }

        public override string ToString()
        {
            string lStr = string.Format("StateList {0} - {1} - {2}\n", _listName, _stateTable, _currentState?.Name ?? "null");
            foreach (KeyValuePair<string, object> e in _stateTable)
            {
                if (e.Value is State s)
                {
                    lStr += string.Format("{0}\n", s.Name);
                }
            }
            return lStr;
        }
    }
}