using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JvLib.Editor.AssetImporters.Textures
{
    using UnityEditor;
    [CustomEditor(typeof(JvTextureImportSettings))]
    public class JvTextureImportSettingsEditor : UnityEditor.Editor
    {
        bool advancedOpenFlag = true;

        private JvTextureImportSettings Target;

        private void OnEnable()
        {
            Target = target as JvTextureImportSettings;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("TextureType"));
            /*if(Target.TextureType == TextureImporterType.GUI ||
                Target.TextureType == TextureImporterType.Cursor ||
                Target.TextureType == TextureImporterType.Lightmap ||
                Target.TextureType == TextureImporterType.DirectionalLightmap ||
                Target.TextureType == TextureImporterType.Shadowmask)
            {
                serializedObject.FindProperty("TextureShape").intValue = 1;
                GUI.enabled = false;
            }
            serializedObject.FindProperty("TextureShape").intValue = EditorGUILayout.IntPopup("Texture Shape", 
                serializedObject.FindProperty("TextureShape").intValue,
                new string[4] { "2D", "Cube", "2D Array", "3D" }, new int[4] { 1, 2, 4, 8 });*/  // If Required, lets add this but not now

            GUI.enabled = true;

            EditorGUILayout.Space();
            if (Target.TextureType == TextureImporterType.Default)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("sRGB"));
            }
            if(Target.TextureType == TextureImporterType.Sprite)
            {
                DrawSpriteSettings();
            }

            if (Target.TextureType == TextureImporterType.Default ||
                Target.TextureType == TextureImporterType.Cookie)
            {
                DrawAlphaSettings();
            }
            if (Target.TextureType == TextureImporterType.SingleChannel)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Channel"));
                if (Target.Channel == TextureImporterSingleChannelComponent.Alpha)
                    DrawAlphaSettings();
            }

                if (Target.TextureType == TextureImporterType.Default || 
                Target.TextureType == TextureImporterType.NormalMap ||
                Target.TextureType == TextureImporterType.Cookie ||
                Target.TextureType == TextureImporterType.SingleChannel)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("IgnorePNGFileGamma"));
            }
            if(Target.TextureType == TextureImporterType.NormalMap)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("CreateFromGrayscale"));
            }

            EditorGUILayout.Space();
            if(advancedOpenFlag = EditorGUILayout.Foldout(advancedOpenFlag, new GUIContent("Advanced")))
            {
                EditorGUI.indentLevel++;
                if (Target.TextureType == TextureImporterType.Sprite)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("sRGB"));
                }
                if (Target.TextureType == TextureImporterType.Sprite ||
                    Target.TextureType == TextureImporterType.GUI || 
                    Target.TextureType == TextureImporterType.Cursor)
                    DrawAlphaSettings();

                if (Target.TextureType == TextureImporterType.Sprite ||
                    Target.TextureType == TextureImporterType.GUI ||
                    Target.TextureType == TextureImporterType.Cursor || 
                    Target.TextureType == TextureImporterType.Lightmap ||
                    Target.TextureType == TextureImporterType.Shadowmask)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("IgnorePNGFileGamma"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("Readable"), new GUIContent("Read/Write Enabled"));

                if (Target.TextureType == TextureImporterType.Default ||
                    Target.TextureType == TextureImporterType.NormalMap ||
                    Target.TextureType == TextureImporterType.Lightmap ||
                    Target.TextureType == TextureImporterType.DirectionalLightmap ||
                    Target.TextureType == TextureImporterType.Shadowmask || 
                    Target.TextureType == TextureImporterType.SingleChannel)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("StreamingMipmaps"));

                if (Target.TextureType == TextureImporterType.Default ||
                    Target.TextureType == TextureImporterType.NormalMap)
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("VirtualTextureOnly"));
                DrawMipMapSettings();

                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("WrapMode"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("FilterMode"));

            if (Target.TextureType == TextureImporterType.Sprite ||
                Target.TextureType == TextureImporterType.GUI ||
                Target.TextureType == TextureImporterType.Cursor ||
                Target.TextureType == TextureImporterType.Cookie)
            {
                serializedObject.FindProperty("AnisoLevel").intValue = (Target.TextureType == TextureImporterType.Cookie ? 0 : 1);
                GUI.enabled = false;
            }
            serializedObject.FindProperty("AnisoLevel").intValue = EditorGUILayout.IntSlider(new GUIContent("Aniso Level"), serializedObject.FindProperty("AnisoLevel").intValue, 0, 16);
            if (Target.AnisoLevel > 1) EditorGUILayout.HelpBox("Anisotropic filtering is enabled for all textures in Quality Settings.", MessageType.Info);
            GUI.enabled = true;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Texture Settings", EditorStyles.boldLabel);
            serializedObject.FindProperty("MaxSize").intValue = EditorGUILayout.IntPopup("Max Size", serializedObject.FindProperty("MaxSize").intValue, 
                new string[10] { "32", "64", "128", "256", "512", "1024", "2048", "4096", "8192", "16384" }, 
                new int[10] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384 });

            serializedObject.FindProperty("Compression").intValue = EditorGUILayout.IntPopup("Compression", serializedObject.FindProperty("Compression").intValue,
                new string[4] { "None", "Low Quality", "Normal Quality", "High Quality" },
                new int[4] { 0, 3, 1, 2 });

            if(Target.Compression != TextureImporterCompression.Uncompressed)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("UseCrunchCompression"));
                if(Target.UseCrunchCompression) serializedObject.FindProperty("CompressionQuality").intValue = 
                        EditorGUILayout.IntSlider(new GUIContent("Compression Quality"), serializedObject.FindProperty("CompressionQuality").intValue, 0, 100);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawSpriteSettings()
        {
            serializedObject.FindProperty("SpriteMode").intValue = EditorGUILayout.IntPopup("Sprite Mode",
               serializedObject.FindProperty("SpriteMode").intValue,
               new string[3] { "Single", "Multiple", "Polygon" }, new int[3] { 1, 2, 3 });

            EditorGUI.indentLevel++;
            if (Target.SpriteMode == SpriteImportMode.Multiple)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("SpriteSize"));
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("PixelsPerUnit"));

            if (Target.SpriteMode == SpriteImportMode.Single ||
                Target.SpriteMode == SpriteImportMode.Multiple)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MeshType"));
                serializedObject.FindProperty("ExtrudeEdges").intValue = EditorGUILayout.IntSlider(new GUIContent("ExtrudeEdges"), serializedObject.FindProperty("ExtrudeEdges").intValue, 0, 32);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Pivot"));
            }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GeneratePhysicsShape"));
            EditorGUI.indentLevel--;
        }

        private void DrawAlphaSettings()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AlphaSource"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("AlphaIsTransparency"));
        }

        private void DrawMipMapSettings()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GenerateMipmaps"));
            if (Target.GenerateMipmaps)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("BorderMipMaps"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MipMapFilter"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MipMapsPreserveCoverage"));
                if (Target.MipMapsPreserveCoverage)
                {
                    EditorGUI.indentLevel++;
                    serializedObject.FindProperty("AlphaCutoffValue").floatValue = EditorGUILayout.Slider(new GUIContent("Alpha Cutoff Value"), serializedObject.FindProperty("AlphaCutoffValue").floatValue, 0f, 1f);
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}
