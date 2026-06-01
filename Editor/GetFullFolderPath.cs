using System.IO;
using UnityEditor;
using UnityEngine;

namespace HungNT.Editor
{
    public static class GetFullFolderPath
    {
        [MenuItem("Assets/HungNT/Copy Folder Path", false, -1)]
        private static void CopyFullPath()
        {
            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(assetPath))
            {
                Debug.LogWarning("No folder selected or invalid selection!");
                return;
            }

            var fullPath = Path.GetFullPath(assetPath);
            GUIUtility.systemCopyBuffer = fullPath;
            Debug.Log($"Copied to clipboard: {fullPath}");
        }

        [MenuItem("Assets/HungNT/Copy Folder Path", true)]
        private static bool ValidateCopyFullPath()
        {
            var assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            return !string.IsNullOrEmpty(assetPath) && AssetDatabase.IsValidFolder(assetPath);
        }
    }
}
