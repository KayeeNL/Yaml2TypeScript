
using Yaml2TypeScript.Models;

namespace Yaml2TypeScript.Processor
{
    internal class FieldReference : Base
    {
        public string? Hint { get; set; }
        public string? Value { get; set; }
    }
}
