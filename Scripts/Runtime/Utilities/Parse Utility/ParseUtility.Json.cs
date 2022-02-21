using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Json
    {
        public static Hashtable JsonParse(string pJSONString)
        {
            string lJSONStr = pJSONString.Replace("\"", "");
            string[] keyvaluepairs = lJSONStr.Replace("\"", "").Split(';');
            int ispos;
            Hashtable ht = new Hashtable();

            foreach (string kvp in keyvaluepairs)
            {
                ispos = kvp.IndexOf('=');

                if (ispos >= 0)
                {
                    // Add the key value pair
                    Debug.Log("Adding:" + kvp.Substring(0, ispos).Trim() + "," + kvp.Substring(ispos + 1).Trim());
                    ht.Add(kvp.Substring(0, ispos).Trim(), kvp.Substring(ispos + 1));
                }
            }
            return ht;
        }

        public static Hashtable JsonParse(string pJSONString, string pStructConstructor) => JsonParse(pJSONString, new string[] { pStructConstructor });
        public static Hashtable JsonParse(string pJSONString, string[] pStructConstructor = null)
        {
            string lJSONStr = pJSONString.Replace("\"", "");
            //Remove Struct Constructors
            if (pStructConstructor != null && pStructConstructor.Length > 0)
            {
                for (int i = 0; i < pStructConstructor.Length; i++)
                {
                    if (!lJSONStr.Contains(pStructConstructor[i])) continue;
                    lJSONStr = lJSONStr.Replace(pStructConstructor[i], "");
                }
            }

            //Remove { } 
            lJSONStr = Regex.Replace(lJSONStr, "[{}]", "");

            string[] keyvaluepairs = lJSONStr.Replace("\"", "").Split(';');
            int ispos;
            Hashtable ht = new Hashtable();

            foreach (string kvp in keyvaluepairs)
            {
                ispos = kvp.IndexOf(':');

                if (ispos >= 0)
                {
                    // Add the key value pair
                    ht.Add(kvp.Substring(0, ispos).Trim(), kvp.Substring(ispos + 1));
                }
            }

            return ht;
        }
    }
}
