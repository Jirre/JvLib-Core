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
        public static Vector3 VectorParse(string pString, char pVectorSeperator = ',') => VectorParse(pString, Vector2.zero, pVectorSeperator);
        /// <summary>
        /// Attempts to parse a string with the given seperator to a Vector2, using the default fallback upon a failure
        /// </summary>
        public static Vector3 VectorParse(string pString, Vector2 pVectorDefault, char pSeperator = ',')
        {
            Vector3 result = pVectorDefault;
            string lPosString = pString;

            if (string.IsNullOrEmpty(lPosString)) return result;

            List<float> lPosList = FloatParse(lPosString, pSeperator, 0.0f);
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
            return pList[pIndex];
        }

        /// <summary>
        /// Tries to split a string in a collection of Vector3 and return the result
        /// </summary>
        public static List<Vector3> VectorParse(string pString, char pSeperator, char pVectorSeperator = ';') =>
            VectorParse(pString, pSeperator, Vector3.zero, pVectorSeperator);
        /// <summary>
        /// Tries to split a string in a collection of Vector3 and return the result
        /// </summary>
        public static List<Vector3> VectorParse(string pString, char pSeperator, Vector3 pDefault, char pVectorSeperator = ';')
        {
            List<Vector3> lList = new List<Vector3>();
            if ((pString?.Length ?? 0) > 0)
            {
                string[] lStrArray = pString.Split(pSeperator);
                foreach (string lStr in lStrArray)
                    lList.Add(VectorParse(lStr, pDefault, pVectorSeperator));
            }
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static Vector3 VectorParse(IDictionary pDictionary, string pKey, char pSeperator = ';') =>
            VectorParse(pDictionary, pKey, Vector3.zero, pSeperator);
        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static Vector3 VectorParse(IDictionary pDictionary, string pKey, Vector3 pDefault, char pSeperator = ';')
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            if (!pDictionary.Contains(pKey)) return pDefault;

            return VectorParse((string)pDictionary[pKey], pDefault, pSeperator);
        }
    }
}