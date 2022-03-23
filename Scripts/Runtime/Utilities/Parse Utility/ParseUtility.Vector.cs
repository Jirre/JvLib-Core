using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Vector
    {
        /// <summary>
        /// Attempts to parse a string with the given seperator to a Vector2, using the default fallback upon a failure
        /// </summary>
        public static Vector3 VectorParse(string pString, char pVectorSeparator = ',') => VectorParse(pString, Vector2.zero, pVectorSeparator);
        /// <summary>
        /// Attempts to parse a string with the given seperator to a Vector2, using the default fallback upon a failure
        /// </summary>
        public static Vector3 VectorParse(string pString, Vector2 pVectorDefault, char pSeparator = ',')
        {
            Vector3 result = pVectorDefault;

            if (string.IsNullOrEmpty(pString)) return result;

            List<float> lPosList = FloatParse(pString, pSeparator, 0.0f);
            if (lPosList == null || (lPosList.Count != 2 && lPosList.Count != 3)) return result;

            result.x = lPosList[0];
            result.y = lPosList[1];
            result.z = lPosList.Count == 3 ? lPosList[2] : 0f;

            return result;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static Vector3 VectorParse(List<Vector3> pList, int pIndex) =>
            VectorParse(pList, pIndex, Vector3.zero);
        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static Vector3 VectorParse(List<Vector3> pList, int pIndex, Vector3 pDefault)
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return pList != null ? pList[pIndex] : pDefault;
        }

        /// <summary>
        /// Tries to split a string in a collection of Vector3 and return the result
        /// </summary>
        public static List<Vector3> VectorParse(string pString, char pSeparator, char pVectorSeparator = ';') =>
            VectorParse(pString, pSeparator, Vector3.zero, pVectorSeparator);
        /// <summary>
        /// Tries to split a string in a collection of Vector3 and return the result
        /// </summary>
        public static List<Vector3> VectorParse(string pString, char pSeparator, Vector3 pDefault, char pVectorSeperator = ';')
        {
            List<Vector3> lList = new List<Vector3>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            string[] lStrArray = pString.Split(pSeparator);
            foreach (string lStr in lStrArray)
                lList.Add(VectorParse(lStr, pDefault, pVectorSeperator));
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static Vector3 VectorParse(IDictionary pDictionary, string pKey, char pSeparator = ';') =>
            VectorParse(pDictionary, pKey, Vector3.zero, pSeparator);
        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static Vector3 VectorParse(IDictionary pDictionary, string pKey, Vector3 pDefault, char pSeparator = ';')
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            return !pDictionary.Contains(pKey) ? pDefault : VectorParse((string)pDictionary[pKey], pDefault, pSeparator);
        }
    }
}