using System;
using System.Collections.Generic;
using UnityEngine;

namespace HungNT.Editor
{
    [CreateAssetMenu(fileName = "PackageCatalogData", menuName = "HungNT/Package Catalog Data")]
    public sealed class PackageCatalogData : ScriptableObject
    {
        [Serializable]
        public sealed class Entry
        {
            public string name;
            public string source;
        }

        public List<Entry> packages = new();
    }
}
