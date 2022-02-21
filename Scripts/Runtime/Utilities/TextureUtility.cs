using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace JvLib.Utilities
{
    public static class TextureUtility
    {
        /// <summary>
        /// Converts an Image to a base-64 string
        /// </summary>
        /// <param name="pImage">Source Image</param>
        /// <returns>base-64 encoded string</returns>
        public static string ToBase64(Texture2D pImage)
        {
            byte[] bytes;

            bytes = pImage.EncodeToPNG();

            string enc = Convert.ToBase64String(bytes);
            return enc;
        }
        /// <summary>
        /// Convert a Base-64 string to an Image
        /// </summary>
        /// <param name="pBase64Image">Source Base-encoded String</param>
        /// <returns>Decoded Image</returns>
        public static Texture2D FromBase64(string pBase64Image)
        {
            if (!Regex.IsMatch(pBase64Image, "^([A-Za-z0-9+/]{4})*([A-Za-z0-9+/]{4}|[A-Za-z0-9+/]{3}=|[A-Za-z0-9+/]{2}==)$"))
            {
                Debug.LogWarning("Base64Image is not a base 64 string");
                return new Texture2D(1, 1);
            }
            Texture2D tex = new Texture2D(1, 1);
            byte[] bytes = Convert.FromBase64String(pBase64Image);

            tex.LoadImage(bytes);

            return tex;
        }
    }
}