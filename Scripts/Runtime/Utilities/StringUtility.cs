using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace JvLib.Utilities
{
    public static partial class StringUtility
    {
        /// <summary>
        /// Attempts to return the state of a certain Bit inside the Bit String
        /// </summary>
        public static bool ExtractBitString(string pStr, int pIndex)
        {
            if (pIndex < 0 || pIndex >= pStr.Length)
            {
                return false;
            }
            return pStr[pIndex] == '1';
        }

        /// <summary>
        /// Attempts to return a hashtable of keys and values in a URL
        /// </summary>
        public static Hashtable ExtractURLParams(string pURL)
        {
            string lEvalStr = pURL.Substring(pURL.LastIndexOf('?') + 1);
            return ParseUtility.HashtableParse(lEvalStr, '&');
        }

        #region --- ASCII ---
        /// <summary>
        /// Converts an ASCII Buffer into a Unicode string
        /// </summary>
        public static string AsciiBufferToUnicode(byte[] pBuffer, int pOffset, int pLength)
        {
            Encoding enc = Encoding.UTF8;

            return enc.GetString(pBuffer, pOffset, pLength);
        }
        /// <summary>
        /// Converts a string into an ASCII Buffer
        /// </summary>
        public static byte[] UnicodeToAsciiBuffer(string pString)
        {
            // Create encoding object
            Encoding enc = Encoding.UTF8;
            return enc.GetBytes(pString);
        }

        private static readonly byte[] XOR_ORBITS = { 0xF6, 0x75, 0xF4, 0x73, 0xF2, 0x71, 0xF0, 0x6F, 0xEE, 0x6D, 0xEC, 0x6B, 0xEA, 0x69, 0xE8, 0x67, 0xE6, 0x65 };
        private static readonly int XOR_BIT_COUNT = XOR_ORBITS.Length;
        
        /// <summary>
        /// Converts a secure ASCII Buffer into a Unicode string, making sure it doesn't overflow a regular byte-size
        /// </summary>
        public static string SecureAsciiBufferToUnicode(byte[] pBuffer, int pOffset, int pLength)
        {
            Encoding enc = Encoding.UTF8;
            int i;

            for (i = 0; i < pLength; i++)
            {
                pBuffer[i + pOffset] = (byte)(pBuffer[i + pOffset] ^ XOR_ORBITS[(i % XOR_BIT_COUNT)]);
            }

            return enc.GetString(pBuffer, pOffset, pLength);
        }

        /// <summary>
        /// Converts a string into an ASCII Buffer, making sure it doesn't overflow a regular byte-size
        /// </summary>
        public static byte[] UnicodeToSecureAsciiBuffer(string pString)
        {
            // Create encoding object
            Encoding enc = Encoding.UTF8;
            int i;

            byte[] buffer = enc.GetBytes(pString);

            for (i = 0; i < buffer.Length; i++)
            {
                buffer[i] = (byte)(buffer[i] ^ XOR_ORBITS[(i % XOR_BIT_COUNT)]);
            }

            return buffer;
        }

        /// <summary>
        /// Converts an ASCII buffer to a Hashtable
        /// </summary>
        public static Hashtable AsciiToHashtable(byte[] pBuffer, int pOffset, int pLength, char pParamSeparator = ';', char pValueSeparator = '=')
        {
            string msg = SecureAsciiBufferToUnicode(pBuffer, pOffset, pLength);
            string[] kvPairs = msg.Split(pParamSeparator);
            Hashtable ht = new Hashtable();

            foreach (string kvp in kvPairs)
            {
                int sPos = kvp.IndexOf(pValueSeparator);
                if (sPos >= 0)
                    ht.Add(kvp.Substring(0, sPos).Trim(), kvp.Substring(sPos + 1));
            }
            return ht;
        }

        /// <summary>
        /// Converts a Hashtable to an ASCII buffer
        /// </summary>
        public static byte[] HashtableToAscii(Hashtable pHashtable)
        {
            string txt = HashtableToString(pHashtable);

            // Create encoding object
            Encoding enc = Encoding.UTF8;

            // Create buffer
            byte[] buffer = new byte[txt.Length];

            // Encode
            enc.GetBytes(txt, 0, txt.Length, buffer, 0);

            return buffer;
        }

        /// <summary>
        /// Converts non-ASCII characters to an encoded style which is accepted
        /// </summary>
        public static string NonAsciiToUnicode(string pValue)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in pValue)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");

                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        /// <summary>
        /// Converts non-ASCII encoded string to a unicode string
        /// </summary>
        public static string AsciiToUnicode(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            return Regex.Replace(
                value,
                   @"\\u(?<Value>[a-zA-Z0-9]{4})",
                   m => ((char)int.Parse(m.Groups["Value"].Value, System.Globalization.NumberStyles.HexNumber)).ToString());
        }
        #endregion

        #region --- UTF-8 ---
        /// <summary>
        /// Converts s UTF8 encoded string to a regular Unicode string
        /// </summary>
        /// <param name="pString">UTF8 string</param>
        /// <returns>Unicode string</returns>
        public static string Utf8ToUnicode(string pString)
        {
            // read the string as UTF-8 bytes.
            byte[] encodedBytes = Encoding.UTF8.GetBytes(pString);

            // builds the converted string.
            return Encoding.Unicode.GetString(encodedBytes);
        }
        /// <summary>
        /// Returns a UTF-8 Character build from the Hex Value given in the string
        /// </summary>
        /// <param name="pString">String to convert into a UTF8 Character</param>
        /// <returns>UTF8 Character</returns>
        public static string UnicodeToUtf8(string pString) => char.ConvertFromUtf32(ParseUtility.IntParse(pString, System.Globalization.NumberStyles.HexNumber));
        #endregion

        #region --- Base-64 ---
        /// <summary>
        /// Converts a Base-64 string to a UTF-8 String
        /// </summary>
        /// <param name="pStr">Base-64 String</param>
        /// <returns>The resulting UTF-8 String</returns>
        static public string Base64ToUtf8(string pStr)
        {
            byte[] lDecodedBytes = Convert.FromBase64String(pStr);
            return Encoding.UTF8.GetString(lDecodedBytes);
        }
        #endregion

        #region --- Hash Tables ---
        /// <summary>
        /// Converts a hashtable to a collapsed string
        /// </summary>
        public static string HashtableToString(Hashtable pHashtable, char pParamSeparator = ';', char pValueSeparator = '=')
        {
            StringBuilder sb = new StringBuilder();

            foreach (string key in pHashtable.Keys)
            {
                string val = pHashtable[key].ToString();

                sb.Append(key);
                sb.Append(pValueSeparator);
                sb.Append(val);
                sb.Append(pParamSeparator);
            }

            return sb.ToString();
        }
        #endregion

        private const string HUNGARIAN_PREFIX = "m_";
        private const string UNDERSCORE = "_";
        private const string HUMAN_READABLE_SEPARATOR = " ";
        private const string SCREAM_WORD_SEPARATOR = "_";

        /// <summary>
        /// Gets the human readable version of  programmer text like a variable name.
        /// </summary>
        /// <param name="pProgrammerText">The programmer text.</param>
        /// <returns>The human readable equivalent of the programmer text.</returns>
        public static string GetHumanReadableText(string pProgrammerText)
        {
            bool wasLetter = false;
            bool wasUpperCase = false;
            bool addedSpace = false;
            string result = "";

            // First remove the  prefix if it exists.
            if (pProgrammerText.StartsWith(HUNGARIAN_PREFIX))
                pProgrammerText = pProgrammerText.Substring(HUNGARIAN_PREFIX.Length);

            // Deal with any miscellaneous underscores.
            pProgrammerText = pProgrammerText.Replace(UNDERSCORE, string.Empty);

            // Go through the original string and copy it with some modifications.
            for (int i = 0; i < pProgrammerText.Length; i++)
            {
                // If we try to convert "iOS" to a human readable string, this method would
                // return something like "i Os". So we check if we find the text "iOS" here,
                // and just copy it to the result.
                if (MatchesIOS(pProgrammerText, i))
                {
                    result += "iOS";
                    wasLetter = true;
                    wasUpperCase = false;
                    i += 3;
                    continue;
                }

                // If there was a change in caps add spaces.
                bool isNumberOrUpper = char.IsUpper(pProgrammerText[i]) || char.IsNumber(pProgrammerText[i]);
                if ((isNumberOrUpper != wasUpperCase)
                    && i > 0 && !addedSpace)
                {
                    // Upper case to lower case.
                    // I added this so that something like 'GUIItem' turns into 'GUI Item',
                    // but that means we have to make sure that no symbols are involved.
                    if (wasUpperCase && i > 1)
                    {
                        // From letter to letter means we have to insert a space one character back.
                        // Otherwise it's going from a letter to a symbol and we can just add a space.
                        if (wasLetter && char.IsLetter(pProgrammerText[i]))
                            result = result.Insert(result.Length - 1, HUMAN_READABLE_SEPARATOR);
                        else
                            result += HUMAN_READABLE_SEPARATOR;
                        addedSpace = true;
                    }
                    // Lower case to upper case.
                    if (!wasUpperCase)
                    {
                        result += HUMAN_READABLE_SEPARATOR;
                        addedSpace = true;
                    }
                }
                else
                {
                    // No case change.
                    addedSpace = false;
                }

                // Add the character.
                result += pProgrammerText[i];

                // Capitalize the first character.
                if (i == 0)
                    result = result.ToUpper();

                // Remember things about the previous letter.
                wasLetter = char.IsLetter(pProgrammerText[i]);
                wasUpperCase = char.IsUpper(pProgrammerText[i]);
            }
            return result;
        }

        /// <summary>
        /// See if a string matches the text "iOS" on a specific index.
        /// Is used in StringUtility.GetHumanReadableText above.
        /// <see cref="GetHumanReadableText"/>
        /// </summary>
        /// <param name="pProgrammerText">Text to match</param>
        /// <param name="i">Index to match at</param>
        /// <returns>If programmerText matches "iOS" at index i</returns>
        private static bool MatchesIOS(string pProgrammerText, int i)
        {
            if (pProgrammerText.Length < i + 3)
                return false;

            return pProgrammerText[i] == 'i'
                   && pProgrammerText[i + 1] == 'O'
                   && pProgrammerText[i + 2] == 'S';
        }
    }
}
