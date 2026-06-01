using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace HungNT.Editor
{
    public static class SaveMenuItem
    {
        [MenuItem("HungNT/Data/Clear All Data")]
        private static void ClearData()
        {
            PlayerPrefs.DeleteAll();

            var persistentDataPath = Application.persistentDataPath;
            if (Directory.Exists(persistentDataPath))
            {
                Directory.Delete(persistentDataPath, true);
                Directory.CreateDirectory(persistentDataPath);
            }

            Debug.Log("All data cleared!".Color("lime"));
        }

        [MenuItem("HungNT/Data/Open Persistent Data")]
        public static void OpenSaveFolder()
        {
            var saveFolderPath = Application.persistentDataPath;

            if (!string.IsNullOrEmpty(saveFolderPath))
                Process.Start("explorer.exe", saveFolderPath.Replace("/", "\\"));
            else
                Debug.LogError("Save data folder path is invalid or could not be found.");
        }
    }
}
