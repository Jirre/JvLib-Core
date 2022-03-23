using UnityEngine;

namespace JvLib.Utilities
{
    public class ColorUtility : MonoBehaviour
    {
        public static Color32 ColorToColor32(Color pColor)
        {
            return new Color32(
                (byte) Mathf.RoundToInt(pColor.r * 255f),
                (byte) Mathf.RoundToInt(pColor.g * 255f),
                (byte) Mathf.RoundToInt(pColor.b * 255f),
                (byte) Mathf.RoundToInt(pColor.a * 255f));
        }

        public static Color Color32ToColor(Color32 pColor)
        {
            return new Color32(
                (byte) Mathf.RoundToInt((float) pColor.r / 255f),
                (byte) Mathf.RoundToInt((float) pColor.g / 255f),
                (byte) Mathf.RoundToInt((float) pColor.b / 255f),
                (byte) Mathf.RoundToInt((float) pColor.a / 255f));
        }
    }
}
