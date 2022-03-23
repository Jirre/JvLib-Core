using UnityEngine;

namespace JvLib.Utilities
{
    public static class PropertyUtility
    {
        /// <summary>
        /// Checks if the value of the current and the new value are different and exchanges the value of the current with the new
        /// </summary>
        public static bool SetColor(ref Color currentValue, Color newValue)
        {
            if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
                return false;
            currentValue = newValue;
            return true;
        }

        /// <summary>
        /// Checks if the value of the current and the new value are different and exchanges the value of the current with the new
        /// </summary>
        /// <typeparam name="T">Generic type of the Struct to check</typeparam>
        public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
        {
            if (currentValue.Equals(newValue))
                return false;
            currentValue = newValue;
            return true;
        }

        /// <summary>
        /// Checks if the value of the current and the new value are different and exchanges the value of the current with the new
        /// </summary>
        /// <typeparam name="T">Generic type of the Class to check</typeparam>
        public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
        {
            if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
                return false;
            currentValue = newValue;
            return true;
        }
    }
}
