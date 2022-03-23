using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Long
    {
        /// <summary>
        /// Attempts to parse a string to an long, using the default fallback upon a failure
        /// </summary>
        public static long LongParse(string pString, long pDefault = 0)
        {
            return long.TryParse(pString, out long result) ? result : pDefault;
        }
        /// <summary>
        /// Attempts to parse a string to an long, using the default fallback upon a failure
        /// </summary>
        public static long LongParse(string pString, System.Globalization.NumberStyles pStyle, long pDefault = 0)
        {
            return long.TryParse(pString, pStyle, null, out long result) ? result : pDefault;
        }
        /// <summary>
        /// Attempts to parse a string to an long, using the default fallback upon a failure
        /// </summary>
        public static long LongParse(string pString, System.Globalization.NumberStyles pStyle, IFormatProvider pFormat, long pDefault = 0)
        {
            return long.TryParse(pString, pStyle, pFormat, out long result) ? result : pDefault;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static long LongParse(List<long> pList, int pIndex, long pDefault = 0)
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return pList != null ? pList[pIndex] : pDefault;
        }

        /// <summary>
        /// Tries to split a string in longs and return the result
        /// </summary>
        public static List<long> LongParse(string pString, char pSeperator, long pDefault = 0)
        {
            List<long> lList = new List<long>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            string[] lStrArray = pString.Split(pSeperator);
            foreach (string lStr in lStrArray)
                lList.Add(LongParse(lStr, pDefault));
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static long LongParse(IDictionary pDictionary, string pKey, long pDefault = 0)
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            return !pDictionary.Contains(pKey) ? pDefault : LongParse((string)pDictionary[pKey], pDefault);
        }
    }
}