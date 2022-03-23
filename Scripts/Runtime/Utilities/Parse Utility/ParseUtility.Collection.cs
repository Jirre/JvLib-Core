using System.Collections;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //Collection
    {
        /// <summary>
        /// Attempts to parse a Hash Table with the given seperators
        /// </summary>
        public static Hashtable HashtableParse(string pString, char pParamSeperator = ';', char pValueSeperator = '=')
        {
            string[] keyValuePairs = pString.Split(pParamSeperator);
            Hashtable ht = new Hashtable();
            if ((keyValuePairs?.Length ?? 0) <= 0) return ht;
            foreach (string e in keyValuePairs)
            {
                if (string.IsNullOrEmpty(e) || !e.Contains(pValueSeperator.ToString())) continue;
                string[] entry = e.Split(pValueSeperator);
                if ((entry?.Length ?? 0) > 0 && !ht.ContainsKey(entry[0]))
                    ht.Add(entry[0].Trim(), entry[1].Trim());
            }
            return ht;
        }

        public static IList ListParse(IDictionary pDictionary, string pKey)
        {
            IList result = null;
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return result;
            if (!pDictionary.Contains(pKey)) return result;
            try
            {
                result = (IList)pDictionary[pKey];
            }
            catch { return null; }
            return result;
        }

        public static IDictionary DictionaryParse(IDictionary pDictionary, string pKey)
        {
            IDictionary result = null;
            if (pDictionary == null || string.IsNullOrWhiteSpace(pKey)) return result;
            if (!pDictionary.Contains(pKey)) return result;
            try
            {
                result = (IDictionary)pDictionary[pKey];
            }
            catch { return null; }
            return result;
        }
    }
}
