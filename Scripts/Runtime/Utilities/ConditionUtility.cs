using System;
using UnityEngine;

namespace JvLib.Utilities
{
    public enum EBoolCondition
    {
        Ignore,
        True,
        False
    }
    public enum ENumericCondition
    {
        Ignore,

        Equal,
        NotEqual,

        Greater,
        GreaterOrEqual,

        Less,
        LessOrEqual,
    }

    public static class ConditionUtility
    {
        /// <summary>
        /// Checks if Bool Value matches the codition
        /// </summary>
        public static bool IsMatching(bool value, EBoolCondition condition)
        {
            return condition switch
            {
                EBoolCondition.Ignore => true,

                EBoolCondition.True => value,
                EBoolCondition.False => !value,

                _ => throw new ArgumentOutOfRangeException(nameof(condition), condition, null)
            };
        }

        /// <summary>
        /// Checks if float Value matches the codition
        /// </summary>
        /// <param name="buffer">Implements a buffer to catch floating point errors on an 'equal' check</param>
        public static bool IsMatching(float value, float target, ENumericCondition condition, float buffer = 0f)
        {
            return condition switch
            {
                ENumericCondition.Ignore => true,

                ENumericCondition.Equal => Mathf.Approximately(value, target) || Mathf.Abs(value - target) <= buffer,
                ENumericCondition.NotEqual => !Mathf.Approximately(value, target) && Mathf.Abs(value - target) > buffer,

                ENumericCondition.Greater => value + buffer > target,
                ENumericCondition.GreaterOrEqual => Mathf.Approximately(value, target) || value + buffer >= target,

                ENumericCondition.Less => value - buffer < target,
                ENumericCondition.LessOrEqual => Mathf.Approximately(value, target) || value - buffer <= target,

                _ => throw new ArgumentOutOfRangeException(nameof(condition), condition, null)
            };
        }

        /// <summary>
        /// Checks if int Value matches the codition
        /// </summary>
        public static bool IsMatching(int value, int target, ENumericCondition condition)
        {
            return condition switch
            {
                ENumericCondition.Ignore => true,

                ENumericCondition.Equal => value == target,
                ENumericCondition.NotEqual => value != target,

                ENumericCondition.Greater => value > target,
                ENumericCondition.GreaterOrEqual => value >= target,

                ENumericCondition.Less => value <= target,
                ENumericCondition.LessOrEqual => value <= target,

                _ => throw new ArgumentOutOfRangeException(nameof(condition), condition, null)
            };
        }
    }
}

