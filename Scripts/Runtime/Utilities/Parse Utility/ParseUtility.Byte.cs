using System;
using System.Collections;
using System.Collections.Generic;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Byte
    {
        /// <summary>
        /// Attempts to parse a string to an byte, using the default fallback upon a failure
        /// </summary>
        public static byte ByteParse(string pString, byte pDefault = 0)
        {
            return byte.TryParse(pString, out byte result) ? result : pDefault;
        }

        /// <summary>
        /// Attempts to parse a string to an byte, using the default fallback upon a failure
        /// </summary>
        public static byte ByteParse(string pString, System.Globalization.NumberStyles pStyle, byte pDefault = 0)
        {
            return byte.TryParse(pString, pStyle, null, out byte result) ? result : pDefault;
        }

        /// <summary>
        /// Attempts to parse a string to an byte, using the default fallback upon a failure
        /// </summary>
        public static byte ByteParse(string pString, System.Globalization.NumberStyles pStyle, IFormatProvider pFormat, byte pDefault = 0)
        {
            return byte.TryParse(pString, pStyle, pFormat, out byte result) ? result : pDefault;
        }

        /// <summary>
        /// Tries to split a string in bytes and return the result
        /// </summary>
        public static List<byte> ByteParse(string pString, char pSeperator, byte pDefault = 0)
        {
            List<byte> lList = new List<byte>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            string[] lStrArray = pString.Split(pSeperator);
            foreach (string lStr in lStrArray)
                lList.Add(ByteParse(lStr, pDefault));
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static byte ByteParse(List<byte> pList, int pIndex, byte pDefault = 0)
        {
            return (pList?.Count ?? 0) <= pIndex ? pDefault : pList[pIndex];
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static byte ByteParse(IDictionary pDictionary, string pKey, byte pDefault = 0)
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            return !pDictionary.Contains(pKey) ? pDefault : ByteParse((string)pDictionary[pKey], pDefault);
        }
    }
}