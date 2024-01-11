﻿
using Yaml2TypeScript.Models.TypeScript;
using Yaml2TypeScript.Processor;

namespace Yaml2TypeScript.Repositories
{
    internal class TypeScriptFieldsRepository
    {
        internal static Dictionary<Guid, TypeScriptField> Items = new Dictionary<Guid, TypeScriptField>();

        internal void CreateTypes(FieldRepository repo)
        {
            var repoItems = repo.GetValues();
            foreach (var item in repoItems) 
            {
                Field field = item as Field;
                if(field == null)
                {
                    continue;
                }

                Guid key = new Guid(field.ID);
                var tsClass = new TypeScriptField
                {
                    Name = field.Name,
                    FieldType = field.FieldType,
                    SitecoreId = key,
                };
                Items.Add(key, tsClass);
            }
        }
    }
}
