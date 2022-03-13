using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.Events
{
    [Serializable]
    public class EventStateMachine
    {
        private Hashtable _stateTable;
        public int Count => _stateTable?.Count ?? 0;

        public EventState CurrentEventState { get; private set; }
        public EventState PreviousEventState { get; private set; }

        private string _listName;

        #region Constructor

        public EventStateMachine(string pName)
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
            (!string.IsNullOrEmpty(pName) && CurrentEventState?.Name == pName);

        /// <summary>
        /// Does the state machine contain a state with the given name
        /// </summary>
        /// <param name="pName">The name of the state to check for</param>
        public bool HasState(string pName) => _stateTable.ContainsKey(pName);

        #endregion

        #region State Registration

        public bool Add(string pName, EventState.StateDelegate pOnUpdate, EventState.StateDelegate pOnActivate,
            EventState.StateDelegate pOnDeactivate)
        {
            _stateTable ??= new Hashtable();

            if (_stateTable.ContainsKey(pName))
            {
                Debug.LogError("StateList: re-adding state :" + pName);
                return false;
            }

            EventState lEventState = new EventState(this, pName, pOnUpdate, pOnActivate, pOnDeactivate);
            _stateTable.Add(pName, lEventState);
            return true;
        }

        public bool Add(string pNameStr, EventState.StateDelegate pUpdate) => Add(pNameStr, pUpdate, null, null);

        public bool Add(EventState pEventState)
        {
            if (_stateTable == null) _stateTable = new Hashtable();

            if (pEventState == null)
            {
                Debug.LogError("StateList: Adding null-state");
                return false;
            }

            if (_stateTable.ContainsKey(pEventState.Name))
            {
                Debug.LogError("StateList: re-adding state :" + pEventState.Name);
                return false;
            }

            _stateTable.Add(pEventState.Name, pEventState);
            return true;
        }

        #endregion

        #region State Machine Navigation

        /// <summary>
        /// Gets the state within the state list with the given name
        /// </summary>
        /// <param name="pName">Name of the state to return</param>
        public EventState GetState(string pName)
        {
            if (_stateTable.ContainsKey(pName))
                return (EventState) _stateTable[pName];
            return null;
        }

        /// <summary>
        /// Returns the state within the state list if it exists
        /// </summary>
        /// <param name="pName">Name of the state to return</param>
        /// <param name="pEventState">Output of the state if found</param>
        /// <returns>Whether the state exists or not</returns>
        public bool TryGetState(string pName, out EventState pEventState)
        {
            if (_stateTable.ContainsKey(pName))
            {
                pEventState = (EventState) _stateTable[pName];
                return true;
            }

            pEventState = null;
            return false;
        }

        /// <summary>
        /// Sets the state of this state machine to the given state
        /// </summary>
        /// <param name="pEventState">The state to set the state-machine to</param>
        public void GotoState(EventState pEventState)
        {
            if (CurrentEventState == pEventState) // This is already the current state
                return;

            float currentTime = Time.time;
            if (CurrentEventState != null)
            {
                PreviousEventState = CurrentEventState;
                PreviousEventState.Deactivate(currentTime);
            }

            CurrentEventState = pEventState;
            CurrentEventState.Activate(currentTime);
        }

        /// <summary>
        /// Sets the state of this state machine to the state with the given name
        /// </summary>
        /// <param name="pName">Name of the state to set the state machine to</param>
        /// <returns>Was the setting of the state successful</returns>
        public bool GotoState(string pName)
        {
            EventState eventState = GetState(pName);
            if (HasState(pName))
            {
                GotoState(eventState);
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
            if (CurrentEventState == null) return -2;
            int returnValue = 0;
            try
            {
                returnValue = CurrentEventState.Update(pTime);
            }
            catch (Exception lException)
            {
                string lMessageStr = "State: " + _listName + "; Exception in Update state:" + CurrentEventState.Name +
                                     ", " + lException;
                Debug.LogError(lMessageStr);
            }

            return returnValue;
        }

        public override string ToString()
        {
            string lStr = $"StateList {_listName} - {_stateTable} - {CurrentEventState?.Name ?? "null"}\n";
            foreach (KeyValuePair<string, object> e in _stateTable)
            {
                if (e.Value is EventState s)
                {
                    lStr += $"{s.Name}\n";
                }
            }

            return lStr;
        }
    }
}
