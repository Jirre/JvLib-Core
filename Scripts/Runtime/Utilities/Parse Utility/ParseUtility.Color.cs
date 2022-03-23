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
        public static Color32 RGBParse(string pString, char pSeparator = ';') => RGBParse(pString, new Color32(0, 0, 0, 255), pSeparator);
        /// <summary>
        /// Attempts to parse a string with the given seperator to a color, using the default fallback upon a failure
        /// </summary>
        public static Color32 RGBParse(string pString, Color32 pDefault, char pSeparator = ';')
        {
            Color32 result = pDefault;
            string lColorList = pString;

            if (string.IsNullOrEmpty(lColorList)) return result;

            List<int> lRbgList = IntParse(lColorList, pSeparator);
            if (lRbgList == null || lRbgList.Count < 3 || lRbgList.Count > 4) return result;


            result.r = (byte)lRbgList[0];
            result.g = (byte)lRbgList[1];
            result.b = (byte)lRbgList[2];
            result.a = (lRbgList.Count == 4) ? (byte)lRbgList[3] : (byte)255;

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
            switch (lString.Length)
            {
                case 3:
                    try
                    {
                        byte lR = ByteParse(lString.Substring(0, 1) + lString.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                        byte lG = ByteParse(lString.Substring(1, 1) + lString.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                        byte lB = ByteParse(lString.Substring(2, 1) + lString.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                        return new Color32(lR, lG, lB, 255);
                    }
                    catch { return pDefault; }
                    
                case 4:
                    try
                    {
                        byte lR = ByteParse(lString.Substring(0, 1) + lString.Substring(0, 1), System.Globalization.NumberStyles.HexNumber);
                        byte lG = ByteParse(lString.Substring(1, 1) + lString.Substring(1, 1), System.Globalization.NumberStyles.HexNumber);
                        byte lB = ByteParse(lString.Substring(2, 1) + lString.Substring(2, 1), System.Globalization.NumberStyles.HexNumber);
                        byte lA = ByteParse(lString.Substring(3, 1) + lString.Substring(3, 1), System.Globalization.NumberStyles.HexNumber);
                        return new Color32(lR, lG, lB, lA);
                    }
                    catch { return pDefault; }

                case 6:
                    try
                    {
                        byte lR = ByteParse(lString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                        byte lG = ByteParse(lString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                        byte lB = ByteParse(lString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                        return new Color32(lR, lG, lB, 255);
                    }
                    catch { return pDefault; }

                case 8:
                    try
                    {
                        byte lR = ByteParse(lString.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                        byte lG = ByteParse(lString.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                        byte lB = ByteParse(lString.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                        byte lA = ByteParse(lString.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                        return new Color32(lR, lG, lB, lA);
                    }
                    catch { return pDefault; }

                default:
                    return pDefault;
            }
        }

        /// <summary>
        /// Tries to split a string in colors and return the result
        /// </summary>
        public static List<Color32> RGBParse(string pString, char pSeparator, char pColorSeparator = ';') =>
            RGBParse(pString, pSeparator, new Color32(0, 0, 0, 255), pColorSeparator);
        /// <summary>
        /// Tries to split a string in colors and return the result, using the default fallback upon a failure
        /// </summary>
        public static List<Color32> RGBParse(string pString, char pSeparator, Color32 pDefault, char pColorSeperator = ';')
        {
            List<Color32> lList = new List<Color32>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            string[] lStrArray = pString.Split(pSeparator);
            foreach (string lStr in lStrArray)
                lList.Add(RGBParse(lStr, pDefault, pColorSeperator));
            return lList;
        }

        /// <summary>
        /// Tries to split a string in colors and return the result
        /// </summary>
        public static List<Color32> HexParse(string pString, char pSeparator) =>
            HexParse(pString, pSeparator, new Color32(0, 0, 0, 255));
        /// <summary>
        /// Tries to split a string in colors and return the result, using the default fallback upon a failure
        /// </summary>
        public static List<Color32> HexParse(string pString, char pSeparator, Color32 pDefault)
        {
            List<Color32> lList = new List<Color32>();
            if ((pString?.Length ?? 0) <= 0) return lList;
            string[] lStrArray = pString.Split(pSeparator);
            foreach (string lStr in lStrArray)
                lList.Add(HexParse(lStr, pDefault));
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
            return pList != null ? pList[pIndex] : pDefault;
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
            return pList != null ? HexParse(pList[pIndex], pDefault) : pDefault;
        }

        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key
        /// </summary>
        public static Color32 RGBParse(IDictionary pDictionary, string pKey, char pSeparator = ';') =>
            RGBParse(pDictionary, pKey, new Color32(0, 0, 0, 255), pSeparator);
        /// <summary>
        /// Attempts to return the value in the hashtable corresponding with the Key, returning the default value on a failure
        /// </summary>
        public static Color32 RGBParse(IDictionary pDictionary, string pKey, Color32 pDefault, char pSeparator = ';')
        {
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return pDefault;
            return !pDictionary.Contains(pKey) ? pDefault : RGBParse((string)pDictionary[pKey], pDefault, pSeparator);
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
            return !pDictionary.Contains(pKey) ? pDefault : HexParse((string)pDictionary[pKey], pDefault);
        }
    }
}
