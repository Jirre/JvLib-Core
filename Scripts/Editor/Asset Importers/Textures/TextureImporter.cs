using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace JvLib.Editor.AssetImporters.Textures
{
    public class CustomTextureImporter : AssetPostprocessor
    {
        void OnPreprocessTexture()
        {
            if (!assetImporter.importSettingsMissing) return;
            TextureImporter importer = assetImporter as TextureImporter;
            if (importer == null) return;

            string filePath = TrimToContainingFolder(assetPath);

            JvTextureImportSettings settings = null;

            while (filePath.Length > 1 && settings == null)
            {
                Debug.Log(filePath);
                string[] paths = AssetDatabase.FindAssets($"t:{nameof(JvTextureImportSettings)}", new string[1] { filePath });
                foreach (string p in paths)
                {
                    if (TrimToContainingFolder(AssetDatabase.GUIDToAssetPath(p)) == filePath)
                    {
                        settings = AssetDatabase.LoadAssetAtPath<JvTextureImportSettings>(AssetDatabase.GUIDToAssetPath(p));
                        break;
                    }
                }
                filePath = TrimToContainingFolder(filePath);
            }

            importer.maxTextureSize = settings.MaxSize;
            importer.textureCompression = settings.Compression;
            if (settings.Compression != TextureImporterCompression.Uncompressed)
            {
                importer.crunchedCompression = settings.UseCrunchCompression;
                if (settings.UseCrunchCompression) importer.compressionQuality = settings.CompressionQuality;
            }

            importer.SetTextureSettings(settings.GetSettings());
        }

        void OnPostprocessTexture(Texture2D texture)
        {
            if (!assetImporter.importSettingsMissing) return;
            TextureImporter importer = assetImporter as TextureImporter;

            string filePath = TrimToContainingFolder(assetPath);

            JvTextureImportSettings settings = null;

            while (filePath.Length > 1 && settings == null)
            {
                Debug.Log(filePath);
                string[] paths = AssetDatabase.FindAssets($"t:{nameof(JvTextureImportSettings)}", new string[1] { filePath });
                foreach (string p in paths)
                {
                    if (TrimToContainingFolder(AssetDatabase.GUIDToAssetPath(p)) == filePath)
                    {
                        settings = AssetDatabase.LoadAssetAtPath<JvTextureImportSettings>(AssetDatabase.GUIDToAssetPath(p));
                        break;
                    }
                }
                filePath = TrimToContainingFolder(filePath);
            }

            if (settings == null)
                return;

            importer.maxTextureSize = settings.MaxSize;
            importer.textureCompression = settings.Compression;
            if (settings.Compression != TextureImporterCompression.Uncompressed)
            {
                importer.crunchedCompression = settings.UseCrunchCompression;
                if (settings.UseCrunchCompression) importer.compressionQuality = settings.CompressionQuality;
            }

            if (settings.TextureType == TextureImporterType.Sprite && settings.SpriteMode == SpriteImportMode.Multiple &&
                settings.SpriteSize.x >= 1 && settings.SpriteSize.y >= 1)
            {
                List<SpriteMetaData> metadata = new List<SpriteMetaData>();

                string spriteName = Path.GetFileName(assetPath);
                spriteName = spriteName.Remove(spriteName.IndexOf(Path.GetExtension(assetPath)));

                int count = 0;
                int width = Mathf.RoundToInt(settings.SpriteSize.x),
                    height = Mathf.RoundToInt(settings.SpriteSize.y);
                for (int iy = texture.height; (iy - height) >= 0; iy -= height)
                {
                    for (int ix = 0; (ix + width) <= texture.width; ix += width)
                    {
                        /*bool filled = false;
                        for(int tx = 0; tx < width; tx++)
                        {
                            if (filled) break;
                            for(int ty = 0; ty < height; ty++)
                            {
                                if(texture.GetPixel(ix+tx, iy-ty).a > 0)
                                {
                                    filled = true;
                                    break;
                                }
                            }
                        }
                        if (!filled) continue;*/

                        SpriteMetaData smd = new SpriteMetaData();
                        smd.pivot = settings.Pivot;
                        smd.alignment = 9;
                        smd.name = (spriteName + "_" + (count++).ToString());
                        smd.rect = new Rect(ix, iy - height, width, height);
                        metadata.Add(smd);
                    }
                }
                if ((metadata?.Count ?? 0) > 0)
                    importer.spritesheet = metadata.ToArray();
            }
            else importer.spritesheet = null;
            //This first line was the change that fixed it for me
            AssetDatabase.ForceReserializeAssets(new List<string>() { assetPath });
            //Those lines below I was already using so they may or may not be necessary
            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();

        }

        private string TrimToContainingFolder(string pPath)
        {
            if (!pPath.Contains("/") || pPath.LastIndexOf("/") <= 0) return string.Empty;
            return pPath.Remove(pPath.LastIndexOf("/"));
        }
    }
}
