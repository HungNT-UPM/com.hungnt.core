using Sirenix.OdinInspector;

namespace HungNT.Editor
{
    public sealed class PackageCatalogRow
    {
        public string name;
        public string source;
        public bool installed;

        public static PackageCatalogRow FromEntry(PackageCatalogData.Entry entry)
        {
            return new PackageCatalogRow
            {
                name = entry.name,
                source = entry.source
            };
        }

        public PackageCatalogData.Entry ToEntry()
        {
            return new PackageCatalogData.Entry
            {
                name = name,
                source = source ?? ""
            };
        }
    }
}
