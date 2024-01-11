
using Yaml2TypeScript.Processor;

namespace Yaml2TypeScript.Repositories
{
    internal class FieldSectionRepository : AbstractRepository<Item>
    {
        private static Dictionary<Guid, Item> Cache = new Dictionary<Guid, Item>();
        internal override Dictionary<Guid, Item> Items => Cache;

        internal override bool CreateFolder => false;
    }
}
