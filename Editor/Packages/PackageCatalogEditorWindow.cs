using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace HungNT.Editor
{
    public sealed class PackageCatalogEditorWindow : OdinEditorWindow
    {
        [MenuItem("HungNT/Package Manager", false, -100)]
        private static void Open()
        {
            var window = GetWindow<PackageCatalogEditorWindow>();
            window.titleContent = new GUIContent("Package Manager");
            window.minSize = new Vector2(640f, 360f);
        }

        [PropertySpace]
        [InfoBox("Catalog: Assets/BaseHungNT/Editor/PackageCatalogData.asset. Tick = installed in manifest; untick + Apply = remove.")]
        [ShowInInspector, ReadOnly]
        private string CatalogAssetPath => PackageCatalogDataStorage.AssetPath;

        [TableList(AlwaysExpanded = true, ShowIndexLabels = false)]
        [ShowInInspector]
        private List<PackageCatalogRow> rows = new();

        private PackageCatalogData _data;

        protected override void OnEnable()
        {
            base.OnEnable();
            Reload();
        }

        [HorizontalGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1f)]
        private void Reload()
        {
            _data = PackageCatalogDataStorage.LoadOrCreate();
            rows = PackageCatalogDataStorage.LoadRows(_data);

            if (ManifestDependencyService.SyncFromManifest(rows))
                PackageCatalogDataStorage.SaveRows(_data, rows);
        }

        [HorizontalGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(1f, 0.75f, 0.2f)]
        private void Apply()
        {
            if (_data == null)
                _data = PackageCatalogDataStorage.LoadOrCreate();

            PackageCatalogDataStorage.SaveRows(_data, rows);
            ManifestDependencyService.ApplyToManifest(rows);
        }

        [HorizontalGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.75f, 0.55f, 1f)]
        private void OpenCatalogAsset()
        {
            _data ??= PackageCatalogDataStorage.LoadOrCreate();
            Selection.activeObject = _data;
            EditorGUIUtility.PingObject(_data);
        }

        [HorizontalGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.5f, 1f, 0.55f)]
        private void OpenManifest()
        {
            ManifestDependencyService.OpenManifestFile();
        }

        [HorizontalGroup("Actions")]
        [Button(ButtonSizes.Large), GUIColor(0.85f, 0.85f, 0.85f)]
        private void OpenPackagesFolder()
        {
            ManifestDependencyService.OpenPackagesFolder();
        }
    }
}
