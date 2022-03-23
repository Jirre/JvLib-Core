using System.Collections;
using System.Collections.Generic;

namespace JvLib.Utilities
{
    public static class CollectionUtility
    {
        /// <summary>
        /// Convert an IDictionary to a Dictionary with the given generic type as value type
        /// </summary>
        public static Dictionary<TKey, TValue> ConvertToDictionary<TKey, TValue>(IDictionary pDictionary)
        {
            Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<object, object> e in pDictionary)
            {
                TKey lKey = (TKey) e.Key;
                TValue lValue = (TValue) e.Value;
                if (lKey == null || lValue == null) continue;

                result.Add(lKey, lValue);
            }

            return result;
        }
    }
}
