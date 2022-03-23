using UnityEngine;

namespace JvLib.Utilities
{
    public static class MathUtility
    {
        #region --- Float ---
        /// <summary>
        /// Calculates the sign of a given value (0 returns 0)
        /// </summary>
        public static float Sign(float pValue)
        {
            if (pValue == 0) return 0;
            return Mathf.Sign(pValue);
        }
        /// <summary>
        /// Calculates the distance between two positions
        /// </summary>
        /// <param name="pPointA">Point A</param>
        /// <param name="pPointB">Point B</param>
        /// <returns>Distance</returns>
        public static float PointDistance(Vector2 pPointA, Vector2 pPointB)
        {
            float x = Mathf.Abs(pPointA.x - pPointB.x),
                y = Mathf.Abs(pPointA.y - pPointB.y);

            return Mathf.Sqrt(Mathf.Pow(x, 2f) + Mathf.Pow(y, 2f));
        }
        /// <summary>
        /// Calculates the distance between two positions
        /// </summary>
        /// <param name="pXA">X of Point 1</param>
        /// <param name="pYA">Y of Point 1</param>
        /// <param name="pXB">X of Point 2</param>
        /// <param name="pYB">Y of Point 2</param>
        /// <returns>Distance</returns>
        public static float PointDistance(float pXA, float pYA, float pXB, float pYB) =>
            PointDistance(new Vector2(pXA, pYA), new Vector2(pXB, pYB));
        #endregion

        #region --- Degrees ---
        /// <summary>
        /// Returns the angle in degrees whose Tan is y/x
        /// </summary>
        public static float DegAtan2(float y, float x) => Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        /// <summary>
        /// Returns the cosine of angle f (in Degrees)
        /// </summary>
        public static float DegCos(float f) => Mathf.Cos(Mathf.Deg2Rad * f);
        /// <summary>
        /// Returns the sine of angle f (in Degrees)
        /// </summary>
        public static float DegSin(float f) => Mathf.Sin(Mathf.Deg2Rad * f);

        /// <summary>
        /// Get the rotation in Degrees from origin to target
        /// </summary>
        /// <param name="pOrigin">Origin point</param>
        /// <param name="pTarget">Target point</param>
        public static float DegPointDirection(Vector2 pOrigin, Vector2 pTarget) =>
            DegAtan2(pTarget.y - pOrigin.y, pTarget.x - pOrigin.x);
        /// <summary>
        /// Get the rotation in Degrees from origin to target
        /// </summary>
        /// <param name="pOx">Origin X</param>
        /// <param name="pOy">Origin Y</param>
        /// <param name="pTx">Target X</param>
        /// <param name="pTy">Target Y</param>
        public static float DegPointDirection(float pOx, float pOy, float pTx, float pTy) =>
            DegPointDirection(new Vector2(pOx, pOy), new Vector2(pTx, pTy));

        /// <summary>
        /// Get the delta value of two degree based rotations
        /// </summary>
        /// <param name="pDirA">Rotation A</param>
        /// <param name="pDirB">Rotation B</param>
        /// <returns>Delta</returns>
        public static float DegDelta(float pDirA, float pDirB)
        {
            float a = (((pDirB - pDirA) + 180f) % 360f) - 180f;
            return a + (180f * (a > 180f ? -2 : a < -180f ? 2 : 0));
        }

        /// <summary>
        /// Lerp from one rotation to the other in the shortest possible way
        /// </summary>
        /// <param name="pDirA">Rotation A</param>
        /// <param name="pDirB">Rotation B</param>
        /// <param name="pProgress">Lerp Progress</param>
        /// <returns>Lerped Rotation</returns>
        public static float DegLerp(float pDirA, float pDirB, float pProgress) =>
            pDirA + DegDelta(pDirA, pDirB) * Mathf.Clamp01(pProgress);
        /// <summary>
        /// Unclamped Lerp from one rotation to the other in the shortest possible way
        /// </summary>
        /// <param name="pDirA">Rotation A</param>
        /// <param name="pDirB">Rotation B</param>
        /// <param name="pProgress">Lerp Progress</param>
        /// <returns>Lerped Rotation</returns>
        public static float DegLerpUnclamped(float pDirA, float pDirB, float pProgress) =>
            pDirA + DegDelta(pDirA, pDirB) * pProgress;

        /// <summary>
        /// Normalize a position with a given rotation in degrees
        /// </summary>
        /// <param name="pPos">Center point to rotate around</param>
        /// <param name="pSize">Size of the rectangle to rotate around</param>
        /// <param name="pDir">Direction to be positioned at compared to the center</param>
        /// <returns>Normalized position</returns>
        public static Vector2 DegNormalizedPosition(Vector2 pPos, Vector2 pSize, float pDir)
        {
            float rx = DegCos(pDir), ry = DegSin(pDir);
            float rl = Mathf.Max(Mathf.Abs(rx), Mathf.Abs(ry));
            if (rl < 1)
            {
                rx /= rl;
                ry /= rl;
            }
            return new Vector2(
                pPos.x + rx * pSize.x,
                pPos.y + ry * pSize.y);
        }
        /// <summary>
        /// Normalize a position with a given rotation in degrees
        /// </summary>
        /// <param name="pRect">Rectangle to follow during the rotation</param>
        /// <param name="pDir">Direction to be positioned at compared to the center</param>
        /// <returns>Normalized position</returns>
        public static Vector2 DegNormalizedPosition(Rect pRect, float pDir)
        {
            float rx = DegCos(pDir), ry = DegSin(pDir);
            float rl = Mathf.Max(Mathf.Abs(rx), Mathf.Abs(ry));
            if (rl < 1)
            {
                rx /= rl;
                ry /= rl;
            }
            float mx = pRect.x + (pRect.width * 0.5f),
                  my = pRect.y - (pRect.height * 0.5f);

            return new Vector2(
                mx + rx * (pRect.width * 0.5f),
                my + ry * (pRect.height * 0.5f));
        }
        #endregion

        #region --- Radian ---
        /// <summary>
        /// Get the rotation in Radians from origin to target
        /// </summary>
        /// <param name="pOrigin">Origin point</param>
        /// <param name="pTarget">Target point</param>
        public static float RadPointDirection(Vector2 pOrigin, Vector2 pTarget) =>
            Mathf.Atan2(pTarget.y - pOrigin.y, pTarget.x - pOrigin.x);
        /// <summary>
        /// Get the rotation in Radians from origin to target
        /// </summary>
        /// <param name="pOx">Origin X</param>
        /// <param name="pOy">Origin Y</param>
        /// <param name="pTx">Target X</param>
        /// <param name="pTy">Target Y</param>
        public static float RadPointDirection(float pOx, float pOy, float pTx, float pTy) =>
            RadPointDirection(new Vector2(pOx, pOy), new Vector2(pTx, pTy));

        /// <summary>
        /// Get the delta value of two radian based rotations
        /// </summary>
        /// <param name="pDirA">Rotation A</param>
        /// <param name="pDirB">Rotation B</param>
        /// <returns>Delta</returns>
        public static float RadDelta(float pDirA, float pDirB)
        {
            float a = (((pDirB - pDirA) + Mathf.PI) % (Mathf.PI * 2f)) - Mathf.PI;
            return a + (Mathf.PI * (a > Mathf.PI ? -2 : a < -Mathf.PI ? 2 : 0));
        }
        /// <summary>
        /// Lerp from one rotation to the other in the shortest possible way
        /// </summary>
        /// <param name="pDirA">Rotation A</param>
        /// <param name="pDirB">Rotation B</param>
        /// <param name="pProgress">Lerp Progress</param>
        /// <returns>Lerped Rotation</returns>
        public static float RadLerp(float pDirA, float pDirB, float pProgress) =>
            pDirA + RadDelta(pDirA, pDirB) * Mathf.Clamp01(pProgress);
        /// <summary>
        /// Unclamped Lerp from one rotation to the other in the shortest possible way
        /// </summary>
        /// <param name="pDirA">Rotation A</param>
        /// <param name="pDirB">Rotation B</param>
        /// <param name="pProgress">Lerp Progress</param>
        /// <returns>Lerped Rotation</returns>
        public static float RadLerpUnclamped(float pDirA, float pDirB, float pProgress) =>
            pDirA + RadDelta(pDirA, pDirB) * pProgress;

        /// <summary>
        /// Normalize a position with a given rotation in degrees
        /// </summary>
        /// <param name="pPos">Center point to rotate around</param>
        /// <param name="pSize">Size of the rectangle to rotate around</param>
        /// <param name="pRad">Direction to be positioned at compared to the center</param>
        /// <returns>Normalized position</returns>
        public static Vector2 RadNormalizedPosition(Vector2 pPos, Vector2 pSize, float pRad)
        {
            float rx = Mathf.Cos(pRad), ry = Mathf.Sin(pRad);
            float rl = Mathf.Max(Mathf.Abs(rx), Mathf.Abs(ry));
            if (rl < 1)
            {
                rx /= rl;
                ry /= rl;
            }
            return new Vector2(
                pPos.x + rx * pSize.x,
                pPos.y + ry * pSize.y);
        }
        /// <summary>
        /// Normalize a position with a given rotation in degrees
        /// </summary>
        /// <param name="pRect">Rectangle to follow during the rotation</param>
        /// <param name="pRad">Direction to be positioned at compared to the center</param>
        /// <returns>Normalized position</returns>
        public static Vector2 RadNormalizedPosition(Rect pRect, float pRad)
        {
            float rx = Mathf.Cos(pRad), ry = Mathf.Sin(pRad);
            float rl = Mathf.Max(Mathf.Abs(rx), Mathf.Abs(ry));
            if (rl < 1)
            {
                rx /= rl;
                ry /= rl;
            }
            float mx = pRect.x + (pRect.width * 0.5f),
                  my = pRect.y - (pRect.height * 0.5f);

            return new Vector2(
                mx + rx * (pRect.width * 0.5f),
                my + ry * (pRect.height * 0.5f));
        }
        #endregion
    }
}