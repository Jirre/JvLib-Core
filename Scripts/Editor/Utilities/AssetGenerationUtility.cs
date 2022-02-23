using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace JvLib.Editor.Utilities
{
    public static class AssetGenerationUtility
    {
        public static string GeneratedAssetsFolder
        {
            get { return Path.Combine(Application.dataPath, "Generated"); }
        }

        public static string GeneratedCodeFolder
        {
            get { return Path.Combine(GeneratedAssetsFolder, "Scripts"); }
        }

        public static string GetCodeFolderWithAssemblyReference(Type type, string subFolder = "")
        {
            return GetCodeFolderWithAssemblyReference(type.Assembly, subFolder);
        }

        public static string GetCodeFolderWithAssemblyReference(Assembly assembly, string subFolder = "")
        {
            string result = Path.Combine(GeneratedCodeFolder, subFolder);
            string assemblyName = assembly.GetName().Name;
            if (!assemblyName.StartsWith("Assembly-CSharp"))
            {
                result = Path.Combine(result, assemblyName);
                if (!GenerateAsmRef(result, assembly))
                {
                    return null;
                }
            }
            return result;
        }

        [Serializable]
        private class AssemblyDefinition
        {
            [SerializeField]
            public string name;
        }

        [Serializable]
        private class AssemblyReference
        {
            [SerializeField]
            public string reference;
        }

        private static bool GenerateAsmRef(string targetFolder, Assembly targetAssembly)
        {
            Directory.CreateDirectory(targetFolder);

            string path = GetAssemblyDefinitionPath(targetAssembly);
            string guid = AssetDatabase.AssetPathToGUID(path);
            if (path == null || string.IsNullOrEmpty(guid))
            {
                Debug.LogError("Could not find assembly definition for assembly: " + targetAssembly.FullName);
                return false;
            }

            string[] asmRefFiles = Directory.GetFiles(targetFolder, "*.asmref");
            if (asmRefFiles.Length > 0)
            {
                if (asmRefFiles.Length == 1)
                {
                    if (Path.GetFileNameWithoutExtension(asmRefFiles[0]) == Path.GetFileNameWithoutExtension(path))
                        return true;
                }

                Debug.LogError("Folder " + targetFolder + " already contains an asmref file");
                return false;
            }

            AssemblyReference reference = new AssemblyReference();
            reference.reference = "GUID:" + guid;

            string outputFileName = Path.Combine(targetFolder, Path.GetFileNameWithoutExtension(path) + ".asmref");
            File.WriteAllText(outputFileName, EditorJsonUtility.ToJson(reference, true), Encoding.UTF8);
            return true;
        }

        private static string GetAssemblyDefinitionPath(Assembly targetAssembly)
        {
            string[] paths = AssetDatabase.FindAssets("t:asmdef").Select(AssetDatabase.GUIDToAssetPath).ToArray();
            string assemblyName = targetAssembly.GetName().Name;

            foreach (string path in paths)
            {
                AssemblyDefinition assemblyDefinition = new AssemblyDefinition();
                EditorJsonUtility.FromJsonOverwrite(File.ReadAllText(path, Encoding.UTF8), assemblyDefinition);

                if (assemblyDefinition.name == assemblyName)
                    return path;
            }
            return null;
        }
    }
}
