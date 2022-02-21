using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Float
    {
        /// <summary>
        /// Attempts to parse a string to a float, using the default fallback upon a failure
        /// </summary>
        public static float FloatParse(string pString, float pDefault = 0f)
        {
            if (float.TryParse(pString, out float result)) return result;
            return pDefault;
        }
        /// <summary>
        /// Attempts to parse a string to a float, using the default fallback upon a failure
        /// </summary>
        public static float FloatParse(string pString, System.Globalization.NumberStyles pStyle, float pDefault = 0)
        {
            if (float.TryParse(pString, pStyle, null, out float result)) return result;
            return pDefault;
        }
        /// <summary>
        /// Attempts to parse a string to a float, using the default fallback upon a failure
        /// </summary>
        public static float FloatParse(string pString, System.Globalization.NumberStyles pStyle, IFormatProvider pFormat, float pDefault = 0f)
        {
            if (float.TryParse(pString, pStyle, pFormat, out float result)) return result;
            return pDefault;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static float FloatParse(List<float> pList, int pIndex, float pDefault = 0)
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return pList[pIndex];
        }

        /// <summary>
        /// Tries to split a string in floats and return the result
        /// </summary>
        public static List<float> FloatParse(string pString, char pSeperator, float pDefault = 0)
        {
            List<float> lList = new List<float>();
            if ((pString?.Length ?? 0) > 0)
            {
                string[] lStrArray = pString.Split(pSeperator);
                foreach (string lStr in lStrArray)
                    lList.Add(FloatParse(lStr, pDefault));
            }
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static float FloatParse(IDictionary pDictionary, string pKey, float pDefault = 0)
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            if (!pDictionary.Contains(pKey)) return pDefault;

            return FloatParse((string)pDictionary[pKey], pDefault);
        }
    }
}
