using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HungNT.Editor
{
    public static class PackageCatalogDataStorage
    {
        public const string AssetPath = "Assets/Editor/PackageCatalogData.asset";

        public static PackageCatalogData LoadOrCreate()
        {
            var data = AssetDatabase.LoadAssetAtPath<PackageCatalogData>(AssetPath);
            if (data != null)
                return data;

            EnsureFolder(Path.GetDirectoryName(AssetPath));
            data = ScriptableObject.CreateInstance<PackageCatalogData>();
            AssetDatabase.CreateAsset(data, AssetPath);
            AssetDatabase.SaveAssets();
            Debug.Log($"Created package catalog at {AssetPath}");
            return data;
        }

        private static void EnsureFolder(string folderPath)
        {
            folderPath = folderPath?.Replace('\\', '/');
            if (string.IsNullOrEmpty(folderPath) || AssetDatabase.IsValidFolder(folderPath))
                return;

            var parent = Path.GetDirectoryName(folderPath)?.Replace('\\', '/');
            var folderName = Path.GetFileName(folderPath);
            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(folderName))
                return;

            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, folderName);
        }

        public static List<PackageCatalogRow> LoadRows(PackageCatalogData data)
        {
            var rows = new List<PackageCatalogRow>(data.packages.Count);
            foreach (var entry in data.packages)
                rows.Add(PackageCatalogRow.FromEntry(entry));

            return rows;
        }

        public static void SaveRows(PackageCatalogData data, IReadOnlyList<PackageCatalogRow> rows)
        {
            data.packages.Clear();

            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.name))
                    continue;

                data.packages.Add(row.ToEntry());
            }

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }
    }
}
