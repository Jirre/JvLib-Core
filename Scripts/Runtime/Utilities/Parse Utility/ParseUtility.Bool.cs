using System.Collections;
using System.Collections.Generic;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Bool
    {
        /// <summary>
        /// Attempts to parse a string to a bool, using the default fallback upon a failure
        /// </summary>
        public static bool BoolParse(string pString, bool pDefault = false)
        {
            return bool.TryParse(pString, out bool result) ? result : pDefault;
        }

        /// <summary>
        /// Tries to split a string in bools and return the result
        /// </summary>
        public static List<bool> BoolParse(string pString, char pSeperator, bool pDefault = false)
        {
            List<bool> lList = new List<bool>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            
            string[] lStrArray = pString.Split(pSeperator);
            foreach (string lStr in lStrArray)
                lList.Add(BoolParse(lStr, pDefault));
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static bool BoolParse(List<bool> pList, int pIndex, bool pDefault = false)
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return pList?[pIndex] ?? pDefault;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static bool BoolParse(IDictionary pDictionary, string pKey, bool pDefault = false)
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            return !pDictionary.Contains(pKey) ? pDefault : BoolParse((string)pDictionary[pKey], pDefault);
        }
    }
}
