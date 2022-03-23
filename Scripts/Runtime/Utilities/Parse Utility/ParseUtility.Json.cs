using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Json
    {
        public static Hashtable JsonParse(string pJSONString)
        {
            string lJsonStr = pJSONString.Replace("\"", "");
            string[] kvPairs = lJsonStr.Replace("\"", "").Split(';');
            Hashtable ht = new Hashtable();

            foreach (string kvp in kvPairs)
            {
                int sPos = kvp.IndexOf('=');

                if (sPos < 0) continue;
                // Add the key value pair
                Debug.Log("Adding:" + kvp.Substring(0, sPos).Trim() + "," + kvp.Substring(sPos + 1).Trim());
                ht.Add(kvp.Substring(0, sPos).Trim(), kvp.Substring(sPos + 1));
            }
            return ht;
        }

        public static Hashtable JsonParse(string pJsonString, string pStructConstructor) => JsonParse(pJsonString, new string[] { pStructConstructor });
        public static Hashtable JsonParse(string pJsonString, string[] pStructConstructor = null)
        {
            string lJsonStr = pJsonString.Replace("\"", "");
            //Remove Struct Constructors
            if (pStructConstructor != null && pStructConstructor.Length > 0)
            {
                foreach (string s in pStructConstructor)
                {
                    if (!lJsonStr.Contains(s)) continue;
                    lJsonStr = lJsonStr.Replace(s, "");
                }
            }

            //Remove { } 
            lJsonStr = Regex.Replace(lJsonStr, "[{}]", "");

            string[] kvPairs = lJsonStr.Replace("\"", "").Split(';');
            Hashtable ht = new Hashtable();

            foreach (string kvp in kvPairs)
            {
                int sPos = kvp.IndexOf(':');

                if (sPos >= 0)
                {
                    // Add the key value pair
                    ht.Add(kvp.Substring(0, sPos).Trim(), kvp.Substring(sPos + 1));
                }
            }

            return ht;
        }
    }
}
