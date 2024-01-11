﻿
using Yaml2TypeScript.Processor;

namespace Yaml2TypeScript.Models.TypeScript
{
    internal class TypeScriptField
    {
        public Guid SitecoreId { get; set; }

        public required string Name { get; set; }
        public FieldType FieldType { get; set; }

        public override string ToString()
        {
            var typeScriptFieldType = ProcessorUtils.GetTypeScriptFieldType(FieldType);
            return $"{Name}: {typeScriptFieldType}; // {FieldType}";
        }
    }
}
