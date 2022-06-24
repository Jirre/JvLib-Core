using UnityEngine;
using UnityEditor;

namespace JvLib.Editor.AssetImporters.Textures
{
    [CreateAssetMenu(
        fileName = "TextureImportSettings", 
        menuName = "JvLib/Import Settings/Texture", 
        order = 175)]
    public class JvTextureImportSettings : ScriptableObject
    {
        public TextureImporterType TextureType = TextureImporterType.Default;
        public TextureImporterShape TextureShape = TextureImporterShape.Texture2D; // If Required, lets add this but not now

        public SpriteImportMode SpriteMode = SpriteImportMode.Single;
        public Vector2 SpriteSize = Vector2.zero;
        public float PixelsPerUnit = 100f;
        public SpriteMeshType MeshType = SpriteMeshType.FullRect;
        public uint ExtrudeEdges = 0;
        public Vector2 Pivot = new Vector2(0.5f, 0.5f);
        public bool GeneratePhysicsShape = true;

        public bool sRGB = true;
        public TextureImporterSingleChannelComponent Channel = TextureImporterSingleChannelComponent.Alpha;

        public TextureImporterAlphaSource AlphaSource = TextureImporterAlphaSource.FromInput;
        public bool AlphaIsTransparency = false;
        public bool IgnorePNGFileGamma = false;
        public bool CreateFromGrayscale = false;

        public bool Readable = false;
        public bool StreamingMipmaps = false;
        public int MipMapPriority = 0;
        public bool VirtualTextureOnly = false;

        public bool GenerateMipmaps = true;
        public bool BorderMipMaps = false;
        public TextureImporterMipFilter MipMapFilter = TextureImporterMipFilter.BoxFilter;
        public bool MipMapsPreserveCoverage = false;
        public float AlphaCutoffValue = 0.5f;

        public TextureWrapMode WrapMode = TextureWrapMode.Clamp;
        public FilterMode FilterMode = FilterMode.Bilinear;
        public int AnisoLevel = 1;

        public int MaxSize = 2048;
        public TextureImporterCompression Compression = TextureImporterCompression.Compressed;
        public bool UseCrunchCompression = false;
        public int CompressionQuality = 50;

        public TextureImporterSettings GetSettings()
        {
            TextureImporterSettings settings = new TextureImporterSettings()
            {
                textureType = TextureType,
                textureShape = TextureShape,

                spriteMode = (int)SpriteMode,
                spritePixelsPerUnit = PixelsPerUnit,
                spriteMeshType = MeshType,
                spriteExtrude = ExtrudeEdges,
                spritePivot = Pivot,
                spriteGenerateFallbackPhysicsShape = GeneratePhysicsShape,

                sRGBTexture = sRGB,
                alphaSource = AlphaSource,
                alphaIsTransparency = AlphaIsTransparency,
                ignorePngGamma = IgnorePNGFileGamma,

                readable = Readable,
                streamingMipmaps = StreamingMipmaps,
                streamingMipmapsPriority = MipMapPriority,
                vtOnly = VirtualTextureOnly,

                mipmapEnabled = GenerateMipmaps,
                borderMipmap = (GenerateMipmaps ? BorderMipMaps : false),
                mipmapFilter = (GenerateMipmaps ? TextureImporterMipFilter.BoxFilter : MipMapFilter),
                mipMapsPreserveCoverage = (GenerateMipmaps ? false : MipMapsPreserveCoverage),
                mipmapBias = (GenerateMipmaps ? (MipMapsPreserveCoverage ? AlphaCutoffValue : 0.5f) : 0.5f),

                wrapMode = WrapMode,
                filterMode = FilterMode,
                aniso = AnisoLevel,
            };

            return settings;
        }
    }
}
