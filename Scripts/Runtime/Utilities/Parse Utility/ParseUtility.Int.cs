using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Int
    {
        /// <summary>
        /// Attempts to parse a string to an int, using the default fallback upon a failure
        /// </summary>
        public static int IntParse(string pString, int pDefault = 0)
        {
            return int.TryParse(pString, out int result) ? result : pDefault;
        }

        /// <summary>
        /// Attempts to parse a string to an int, using the default fallback upon a failure
        /// </summary>
        public static int IntParse(string pString, System.Globalization.NumberStyles pStyle, int pDefault = 0)
        {
            return int.TryParse(pString, pStyle, null, out int result) ? result : pDefault;
        }

        /// <summary>
        /// Attempts to parse a string to an int, using the default fallback upon a failure
        /// </summary>
        public static int IntParse(string pString, System.Globalization.NumberStyles pStyle, IFormatProvider pFormat, int pDefault = 0)
        {
            return int.TryParse(pString, pStyle, pFormat, out int result) ? result : pDefault;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static int IntParse(List<int> pList, int pIndex, int pDefault = 0)
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return pList != null ? pList[pIndex] : pDefault;
        }

        /// <summary>
        /// Tries to split a string in ints and return the result
        /// </summary>
        public static List<int> IntParse(string pString, char pSeperator, int pDefault = 0)
        {
            List<int> lList = new List<int>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            string[] lStrArray = pString.Split(pSeperator);
            foreach (string lStr in lStrArray)
                lList.Add(IntParse(lStr, pDefault));
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static int IntParse(IDictionary pDictionary, string pKey, int pDefault = 0)
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            return !pDictionary.Contains(pKey) ? pDefault : IntParse((string)pDictionary[pKey], pDefault);
        }
    }
}