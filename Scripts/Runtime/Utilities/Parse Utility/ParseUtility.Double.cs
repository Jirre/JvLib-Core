using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.Utilities
{
    public static partial class ParseUtility
    {
        /// <summary>
        /// Attempts to parse a string to a double, using the default fallback upon a failure
        /// </summary>
        public static double DoubleParse(string pString, double pDefault = 0f)
        {
            if (double.TryParse(pString, out double result)) return result;
            return pDefault;
        }

        /// <summary>
        /// Attempts to parse a string to a double, using the default fallback upon a failure
        /// </summary>
        public static double DoubleParse(string pString, System.Globalization.NumberStyles pStyle, double pDefault = 0f)
        {
            if (double.TryParse(pString, pStyle, null, out double result)) return result;
            return pDefault;
        }

        /// <summary>
        /// Attempts to parse a string to a double, using the default fallback upon a failure
        /// </summary>
        public static double DoubleParse(string pString, System.Globalization.NumberStyles pStyle, IFormatProvider pFormat, double pDefault = 0f)
        {
            if (double.TryParse(pString, pStyle, pFormat, out double result)) return result;
            return pDefault;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static double DoubleParse(List<double> pList, int pIndex, double pDefault = 0)
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return pList[pIndex];
        }

        /// <summary>
        /// Tries to split a string in doubles and return the result
        /// </summary>
        public static List<double> DoubleParse(string pString, char pSeperator, double pDefault = 0)
        {
            List<double> lList = new List<double>();
            if ((pString?.Length ?? 0) > 0)
            {
                string[] lStrArray = pString.Split(pSeperator);
                foreach (string lStr in lStrArray)
                    lList.Add(DoubleParse(lStr, pDefault));
            }
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static double DoubleParse(IDictionary pDictionary, string pKey, double pDefault = 0)
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            if (!pDictionary.Contains(pKey)) return pDefault;

            return DoubleParse((string)pDictionary[pKey], pDefault);
        }
    }
}
