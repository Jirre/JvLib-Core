using System;
using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Data
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector]
        private List<TKey> _Keys = new List<TKey>();

        [SerializeField, HideInInspector]
        private List<TValue> _Values = new List<TValue>();

        /// <summary>
        /// save the dictionary to lists
        /// </summary>
        public void OnBeforeSerialize()
        {
            _Keys.Clear();
            _Values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                _Keys.Add(pair.Key);
                _Values.Add(pair.Value);
            }
        }

        /// <summary>
        /// load dictionary from lists
        /// </summary>
        public void OnAfterDeserialize()
        {
            Clear();
            if (_Keys.Count != _Values.Count)
                throw new Exception($"there are {_Keys.Count} keys and {_Values.Count} values after deserialization. Make sure that both key and value types are serializable.");

            for (int i = 0; i < _Keys.Count && i < _Values.Count; i++)
                this[_Keys[i]] = _Values[i];
        }
    }
}