using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //String
    {
        /// <summary>
        /// Tries to split a string in sub-strings and return the result
        /// </summary>
        public static List<string> StringParse(string pString, char pSeperator, string pDefault = "")
        {
            List<string> lList = new List<string>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            string[] lStrArray = pString.Split(pSeperator);
            foreach (string lStr in lStrArray)
                lList.Add(string.IsNullOrEmpty(lStr.Trim()) ? pDefault : lStr.Trim());
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static string StringParse(List<string> pList, int pIndex, string pDefault = "")
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return pList != null ? pList[pIndex] : pDefault;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static string StringParse(IDictionary pDictionary, string pKey, string pDefault = "")
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            if (!pDictionary.Contains(pKey)) return pDefault;

            return (string)pDictionary[pKey] ?? pDefault;
        }
    }
}
