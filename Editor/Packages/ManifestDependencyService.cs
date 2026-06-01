using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace HungNT.Editor
{
    public static class ManifestDependencyService
    {
        public static string ManifestPath =>
            Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Packages", "manifest.json"));

        public static void OpenManifestFile()
        {
            if (!File.Exists(ManifestPath))
            {
                Debug.LogError($"manifest.json not found at {ManifestPath}");
                return;
            }

            var process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = ManifestPath,
                UseShellExecute = true,
                Verb = "open"
            };
            process.Start();
        }

        public static void OpenPackagesFolder()
        {
            var packageFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "Packages"));

            if (Directory.Exists(packageFolderPath))
                EditorUtility.RevealInFinder(packageFolderPath);
            else
                Debug.LogError("Package folder not found!");
        }

        public static bool SyncFromManifest(IList<PackageCatalogRow> rows)
        {
            if (rows == null || !TryReadDependencies(out var deps))
                return false;

            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.name))
                    continue;

                var manifestSource = deps[row.name]?.ToString();
                row.installed = manifestSource != null;

                if (row.installed && string.IsNullOrWhiteSpace(row.source))
                    row.source = manifestSource;
            }

            var added = false;
            foreach (var property in deps.Properties())
            {
                var source = property.Value?.ToString();
                if (!IsUrlDependency(source))
                    continue;

                if (ContainsName(rows, property.Name))
                    continue;

                rows.Add(new PackageCatalogRow
                {
                    name = property.Name,
                    source = source,
                    installed = true
                });
                added = true;
            }

            Debug.Log("Synced package list from manifest.json.");
            return added;
        }

        public static void ApplyToManifest(IList<PackageCatalogRow> rows)
        {
            if (rows == null || rows.Count == 0)
                return;

            if (!File.Exists(ManifestPath))
            {
                Debug.LogError("manifest.json not found.");
                return;
            }

            var json = File.ReadAllText(ManifestPath);
            var manifest = JObject.Parse(json);
            var deps = manifest["dependencies"] as JObject;

            if (deps == null)
            {
                Debug.LogError("'dependencies' section not found in manifest.json.");
                return;
            }

            var changed = false;

            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.name))
                    continue;

                if (row.installed)
                {
                    if (string.IsNullOrWhiteSpace(row.source))
                    {
                        Debug.LogWarning($"Package '{row.name}' is enabled but source URL is missing.");
                        continue;
                    }

                    if (deps[row.name]?.ToString() != row.source)
                    {
                        deps[row.name] = row.source;
                        Debug.Log($"Added or updated package: {row.name} → {row.source}");
                        changed = true;
                    }
                }
                else if (deps.Remove(row.name))
                {
                    Debug.Log($"Removed package: {row.name}");
                    changed = true;
                }
            }

            if (!changed)
            {
                Debug.Log("No changes were made to manifest.json.");
                return;
            }

            File.WriteAllText(ManifestPath, manifest.ToString());
            AssetDatabase.Refresh();
            SyncFromManifest(rows);
            Debug.Log("manifest.json has been updated.");
        }

        private static bool ContainsName(IList<PackageCatalogRow> rows, string name)
        {
            foreach (var row in rows)
            {
                if (row.name == name)
                    return true;
            }

            return false;
        }

        private static bool IsUrlDependency(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;

            if (source.StartsWith("file:"))
                return false;

            return source.StartsWith("http://")
                   || source.StartsWith("https://")
                   || source.StartsWith("git://")
                   || source.Contains(".git");
        }

        private static bool TryReadDependencies(out JObject deps)
        {
            deps = null;

            if (!File.Exists(ManifestPath))
            {
                Debug.LogError("manifest.json not found.");
                return false;
            }

            var json = File.ReadAllText(ManifestPath);
            var manifest = JObject.Parse(json);
            deps = manifest["dependencies"] as JObject;

            if (deps != null)
                return true;

            Debug.LogError("'dependencies' section not found in manifest.json.");
            return false;
        }
    }
}
