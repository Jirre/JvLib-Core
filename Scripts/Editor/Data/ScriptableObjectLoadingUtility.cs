using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace JvLib.Editor.Data
{
    public class LoadableAttribute : Attribute
    {
    }

    [InitializeOnLoadAttribute]
    public static class ScriptableObjectLoadingUtility
    {
        static ScriptableObjectLoadingUtility()
        {
            UnityEditor.Editor.finishedDefaultHeaderGUI += DisplaySaveLoadButton;
        }
        
        static void DisplaySaveLoadButton(UnityEditor.Editor editor)
        {
            if (!EditorUtility.IsPersistent(editor.target))
                return;

            if (!(editor.target is ScriptableObject sObject))
                return;

            if (!editor.target.GetType().IsDefined(typeof(LoadableAttribute), true))
                return;

            Rect totalRect = EditorGUILayout.GetControlRect();

            //TODO: Save functionality not fully stable, WIP
            if (GUI.Button(new Rect(totalRect.x, totalRect.y, totalRect.width * .5f, totalRect.height), "Save To Json"))
            {
                string jsonString = sObject.ToJson();

                string path = EditorUtility.SaveFilePanel("Save Data to File", "", sObject.name, "json");
                if (path.Length == 0) return;
            
                StreamWriter writer = new StreamWriter(path, false);
                writer.Write(jsonString);
                writer.Close();
            }
            
            if (GUI.Button(new Rect(totalRect.x + totalRect.width * .5f, totalRect.y, totalRect.width * .5f, totalRect.height), "Load From Json"))
            {
                string path = EditorUtility.OpenFilePanel("Load Data from File", "", "json");
                if (path.Length == 0 || !EditorUtility.DisplayDialog("Overwrite Existing Data?",
                        string.Format(
                            "Are you sure you want to overwrite the existing data of [{0}] with the data found in [{1}]?",
                            sObject.name,
                            Path.GetFileName(path)), "Overwrite", "Cancel")) return;
            
                string fileContent = File.ReadAllText(path);
                sObject.FromJson(fileContent);
            }
        }
    }
}
