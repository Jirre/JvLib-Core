using System.Collections;
using System.Collections.Generic;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Char
    {
        /// <summary>
        /// Attempts to parse a string to a char, using the default fallback upon a failure
        /// </summary>
        public static char CharParse(string pString, char pDefault = ' ')
        {
            return char.TryParse(pString, out char result) ? result : pDefault;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static char CharParse(List<char> pList, int pIndex, char pDefault = ' ')
        {
            return (pList?.Count ?? 0) <= pIndex ? pDefault : pList[pIndex];
        }

        /// <summary>
        /// Tries to split a string in chars and return the result
        /// </summary>
        public static List<char> CharParse(string pString, char pSeperator, char pDefault = ' ')
        {
            List<char> lList = new List<char>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            string[] lStrArray = pString.Split(pSeperator);
            foreach (string lStr in lStrArray)
                lList.Add(CharParse(lStr, pDefault));
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static char CharParse(IDictionary pDictionary, string pKey, char pDefault = ' ')
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            return !pDictionary.Contains(pKey) ? pDefault : CharParse((string)pDictionary[pKey], pDefault);
        }
    }
}

