using System;

namespace JvLib.Utilities
{
    public static partial class ParseUtility //DateTime
    {
        /// <summary>
        /// Attempts to parse a string with the given separator to a DateTime, using the default fallback upon a failure
        /// </summary>
        public static DateTime DateTimeParse(string pString, string pFormat) => DateTimeParse(pString, pFormat, new DateTime(1970, 1, 1, 0, 0, 0));
        /// <summary>
        /// Attempts to parse a string with the given separator to a DateTime, using the default fallback upon a failure
        /// </summary>
        public static DateTime DateTimeParse(string pString, string pFormat, DateTime pDefault)
        {
            return DateTime.TryParseExact(pString, pFormat, null, System.Globalization.DateTimeStyles.None, out DateTime result) ?
                result : 
                pDefault;
        }
    }
}
