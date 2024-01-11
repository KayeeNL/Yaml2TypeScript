
using Yaml2TypeScript.Models;

namespace Yaml2TypeScript.Processor
{
    internal class Item : Base
    {
        public string? Parent { get; set; }
        public string? Template { get; set; }
        public string? Path { get; set; }
        public FieldReference[]? SharedFields { get; set; }


        // not in Yaml file!!
        public List<Item>? SectionReferences { get; set; }
        public List<Item>? FieldItems { get; set; }
    }
}
