using System;
using JvLib.Utilities;
using UnityEngine;

namespace JvLib.Animations
{
    public enum ESwayMethod
    {
        DegCos,
        DegSin,
        RadCos,
        RadSin,
    }

    public static class SwayMethod
    {
        public static float Solve(ESwayMethod pMethod, float pValue)
        {
            return pMethod switch
            {
                ESwayMethod.DegCos => MathUtility.DegCos(pValue),
                ESwayMethod.DegSin => MathUtility.DegSin(pValue),

                ESwayMethod.RadCos => Mathf.Cos(pValue),
                ESwayMethod.RadSin => Mathf.Sin(pValue),

                _ => throw new ArgumentOutOfRangeException(nameof(pMethod), pMethod, null)
            };
        }
    }
}
