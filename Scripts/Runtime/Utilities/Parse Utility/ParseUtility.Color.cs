using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Color
    {
        /// <summary>
        /// Attempts to parse a string with the given seperator to a color
        /// </summary>
        public static Color32 RGBParse(string pString, char pSeperator = ';') => RGBParse(pString, new Color32(0, 0, 0, 255), pSeperator);
        /// <summary>
        /// Attempts to parse a string with the given seperator to a color, using the default fallback upon a failure
        /// </summary>
        public static Color32 RGBParse(string pString, Color32 pDefault, char pSeperator = ';')
        {
            Color32 result = pDefault;
            string lColorList = pString;

            if (string.IsNullOrEmpty(lColorList)) return result;

            List<int> lRBGList = IntParse(lColorList, pSeperator, 0);
            if (lRBGList == null || lRBGList.Count < 3 || lRBGList.Count > 4) return result;


            result.r = (byte)lRBGList[0];
            result.g = (byte)lRBGList[1];
            result.b = (byte)lRBGList[2];
            result.a = (lRBGList.Count == 4) ? (byte)lRBGList[3] : (byte)255;

            return result;
        }
        /// <summary>
        /// Attempts to parse a Hex String to a color
        /// </summary>
        public static Color32 HexParse(string pString) => HexParse(pString, new Color32(0, 0, 0, 255));
        /// <summary>
        /// Attempts to parse a Hex String to a color, using the default fallback upon a failure
        /// </summary>
        public static Color32 HexParse(string pString, Color32 pDefault)
        {
            string lString = pString.Replace("#", "");
            if (lString.Length == 3)
            {
                try
                {
                    byte lR = ByteParse(lString.Substring(0, 1) + lString.Substring(0, 1), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lG = ByteParse(lString.Substring(1, 1) + lString.Substring(1, 1), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lB = ByteParse(lString.Substring(2, 1) + lString.Substring(2, 1), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    return new Color32(lR, lG, lB, 255);
                }
                catch { return pDefault; }
            }
            else if (lString.Length == 4)
            {
                try
                {
                    byte lR = ByteParse(lString.Substring(0, 1) + lString.Substring(0, 1), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lG = ByteParse(lString.Substring(1, 1) + lString.Substring(1, 1), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lB = ByteParse(lString.Substring(2, 1) + lString.Substring(2, 1), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lA = ByteParse(lString.Substring(3, 1) + lString.Substring(3, 1), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    return new Color32(lR, lG, lB, lA);
                }
                catch { return pDefault; }
            }
            else if (lString.Length == 6)
            {
                try
                {
                    byte lR = ByteParse(lString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lG = ByteParse(lString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lB = ByteParse(lString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    return new Color32(lR, lG, lB, 255);
                }
                catch { return pDefault; }
            }
            else if (lString.Length == 8)
            {
                try
                {
                    byte lR = ByteParse(lString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lG = ByteParse(lString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lB = ByteParse(lString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    byte lA = ByteParse(lString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber, (byte)0);
                    return new Color32(lR, lG, lB, lA);
                }
                catch { return pDefault; }
            }
            return pDefault;
        }

        /// <summary>
        /// Tries to split a string in colors and return the result
        /// </summary>
        public static List<Color32> RGBParse(string pString, char pSeperator, char pColorSeperator = ';') =>
            RGBParse(pString, pSeperator, new Color32(0, 0, 0, 255), pColorSeperator);
        /// <summary>
        /// Tries to split a string in colors and return the result, using the default fallback upon a failure
        /// </summary>
        public static List<Color32> RGBParse(string pString, char pSeperator, Color32 pDefault, char pColorSeperator = ';')
        {
            List<Color32> lList = new List<Color32>();
            if ((pString?.Length ?? 0) > 0)
            {
                string[] lStrArray = pString.Split(pSeperator);
                foreach (string lStr in lStrArray)
                    lList.Add(RGBParse(lStr, pDefault, pColorSeperator));
            }
            return lList;
        }

        /// <summary>
        /// Tries to split a string in colors and return the result
        /// </summary>
        public static List<Color32> HexParse(string pString, char pSeperator) =>
            HexParse(pString, pSeperator, new Color32(0, 0, 0, 255));
        /// <summary>
        /// Tries to split a string in colors and return the result, using the default fallback upon a failure
        /// </summary>
        public static List<Color32> HexParse(string pString, char pSeperator, Color32 pDefault)
        {
            List<Color32> lList = new List<Color32>();
            if ((pString?.Length ?? 0) > 0)
            {
                string[] lStrArray = pString.Split(pSeperator);
                foreach (string lStr in lStrArray)
                    lList.Add(HexParse(lStr, pDefault));
            }
            return lList;
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list
        /// </summary>
        public static Color32 RGBParse(List<Color32> pList, int pIndex) =>
            RGBParse(pList, pIndex, new Color32(0, 0, 0, 255));
        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static Color32 RGBParse(List<Color32> pList, int pIndex, Color32 pDefault)
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return pList[pIndex];
        }

        /// <summary>
        /// Attempts to return the value in the given location of the list
        /// </summary>
        public static Color32 HexParse(List<string> pList, int pIndex) =>
            HexParse(pList, pIndex, new Color32(0, 0, 0, 255));
        /// <summary>
        /// Attempts to return the value in the given location of the list, returning the default value on a failure
        /// </summary>
        public static Color32 HexParse(List<string> pList, int pIndex, Color32 pDefault)
        {
            if ((pList?.Count ?? 0) <= pIndex) return pDefault;
            return HexParse(pList[pIndex], pDefault);
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key
        /// </summary>
        public static Color32 RGBParse(IDictionary pDictionary, string pKey, char pSeperator = ';') =>
            RGBParse(pDictionary, pKey, new Color32(0, 0, 0, 255), pSeperator);
        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static Color32 RGBParse(IDictionary pDictionary, string pKey, Color32 pDefault, char pSeperator = ';')
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            if (!pDictionary.Contains(pKey)) return pDefault;

            return RGBParse((string)pDictionary[pKey], pDefault, pSeperator);
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key
        /// </summary>
        public static Color32 HexParse(IDictionary pDictionary, string pKey) =>
            HexParse(pDictionary, pKey, new Color32(0, 0, 0, 255));
        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static Color32 HexParse(IDictionary pDictionary, string pKey, Color32 pDefault)
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            if (!pDictionary.Contains(pKey)) return pDefault;

            return HexParse((string)pDictionary[pKey], pDefault);
        }
    }
}
