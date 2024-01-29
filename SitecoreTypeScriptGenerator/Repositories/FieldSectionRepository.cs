
using SitecoreTypeScriptGenerator.Processor;

namespace SitecoreTypeScriptGenerator.Repositories
{
    internal class FieldSectionRepository : AbstractRepository<Item>
    {
        private static Dictionary<Guid, Item> Cache = new Dictionary<Guid, Item>();
        internal override Dictionary<Guid, Item> Items => Cache;

        internal override bool CreateFolder => false;
    }
}
