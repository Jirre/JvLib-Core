using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.Events
{
    /// <typeparam name="E">Enum Type</typeparam>
    [Serializable]
    public class EventStateMachine<E> 
        where E : Enum
    {
        private Hashtable _stateTable;
        public int Count => _stateTable?.Count ?? 0;

        public EventState<E> CurrentEventState { get; private set; }
        public EventState<E> PreviousEventState { get; private set; }

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
        /// <param name="pID">The state to check against the current state</param>
        public bool IsCurrentState(E pID) =>
            CurrentEventState.ID.Equals(pID);

        /// <summary>
        /// Does the state machine contain a state with the given name
        /// </summary>
        /// <param name="pStateID">The ID of the state to check for</param>
        public bool HasState(E pStateID) => _stateTable.ContainsKey(pStateID);

        #endregion

        #region State Registration

        public bool Add(E pStateID, EventState<E>.StateDelegate pOnUpdate, EventState<E>.StateDelegate pOnActivate,
            EventState<E>.StateDelegate pOnDeactivate)
        {
            _stateTable ??= new Hashtable();

            if (_stateTable.ContainsKey(pStateID))
            {
                Debug.LogError("StateList: re-adding state :" + pStateID);
                return false;
            }

            EventState<E> lEventState = new EventState<E>(this, pStateID, pOnUpdate, pOnActivate, pOnDeactivate);
            _stateTable.Add(pStateID, lEventState);
            return true;
        }

        public bool Add(E pStateID, EventState<E>.StateDelegate pUpdate) => Add(pStateID, pUpdate, null, null);

        public bool Add(EventState<E> pEventState)
        {
            if (_stateTable == null) _stateTable = new Hashtable();

            if (pEventState == null)
            {
                Debug.LogError("StateList: Adding null-state");
                return false;
            }

            if (_stateTable.ContainsKey(pEventState.ID))
            {
                Debug.LogError("StateList: re-adding state :" + pEventState.ID);
                return false;
            }

            _stateTable.Add(pEventState.ID, pEventState);
            return true;
        }

        #endregion

        #region State Machine Navigation

        /// <summary>
        /// Gets the state within the state list with the given name
        /// </summary>
        /// <param name="pStateID">ID of the state to return</param>
        public EventState<E> GetState(E pStateID)
        {
            if (_stateTable.ContainsKey(pStateID))
                return (EventState<E>) _stateTable[pStateID];
            return null;
        }

        /// <summary>
        /// Returns the state within the state list if it exists
        /// </summary>
        /// <param name="pName">Name of the state to return</param>
        /// <param name="pEventState">Output of the state if found</param>
        /// <returns>Whether the state exists or not</returns>
        public bool TryGetState(string pName, out EventState<E> pEventState)
        {
            if (_stateTable.ContainsKey(pName))
            {
                pEventState = (EventState<E>) _stateTable[pName];
                return true;
            }

            pEventState = null;
            return false;
        }

        /// <summary>
        /// Sets the state of this state machine to the given state
        /// </summary>
        /// <param name="pEventState">The state to set the state-machine to</param>
        public void GotoState(EventState<E> pEventState)
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
        /// <param name="pStateID">ID of the state to set the state machine to</param>
        /// <returns>Was the setting of the state successful</returns>
        public bool GotoState(E pStateID)
        {
            EventState<E> eventState = GetState(pStateID);
            if (HasState(pStateID))
            {
                GotoState(eventState);
                return true;
            }

            Debug.LogError("Unknown State:" + pStateID);
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
                string lMessageStr = "State: " + _listName + "; Exception in Update state:" + CurrentEventState.ID +
                                     ", " + lException;
                Debug.LogError(lMessageStr);
            }

            return returnValue;
        }

        public override string ToString()
        {
            string lStr = $"StateList {_listName} - {_stateTable} - {CurrentEventState.ID}\n";
            foreach (KeyValuePair<string, object> e in _stateTable)
            {
                if (e.Value is EventState<E> s)
                {
                    lStr += $"{s.ID}\n";
                }
            }

            return lStr;
        }
    }
}
