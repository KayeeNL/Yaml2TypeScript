using System.Text;
using SitecoreTypeScriptGenerator.Models.TypeScript;
using SitecoreTypeScriptGenerator.Repositories;

namespace SitecoreTypeScriptGenerator.Processor
{
    internal class GenerateTypeScriptFiles2
    {
        public static int Generate()
        {
            ProcessorUtils.GetRootGenerationDirectory();
            return CreateTypes();
        }

        private static int CreateTypes()
        {
            int filesWritten = 0;

            // create typescript types for fields
            new TypeScriptFieldsRepository().CreateTypes(new FieldRepository());

            // create typescript classes
            var repoTypeScriptClassRepo = new TypeScriptClassRepository();
            repoTypeScriptClassRepo.CreateTypes(new ClassRepository(), new FieldSectionRepository());

            var items = repoTypeScriptClassRepo.GetValues();
            foreach (var item in items) 
            {
                if(item.IsSectionNotClass)
                {
                    // skip sections
                    continue;
                }

                string contents = CreateNewType(item);
                if(!string.IsNullOrEmpty(contents))
                {
                    string fileName = item.FilePath;
                    File.WriteAllText(fileName, contents);
                    Console.WriteLine($"wrote: {fileName}");
                    filesWritten++;
                }                
            }

            return filesWritten;
        }

        private static string CreateNewType(TypeScriptClass item)
        {
            if(item?.Fields?.Length== 0 &&
                item?.InheritenceClasses?.Length == 0) 
            {
                Console.WriteLine($"warning: class {item?.ClassName} no fields and no base classes. {item?.SitecoreId}");
                return string.Empty;
            }

            StringBuilder result = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(item.ImportString))
            {
                result.Append(item.ImportString);
            }

            if (item?.Fields?.Length > 0)
            {
                // remove import of Item[] because that's not a OOB type
                var allFieldsTypes = item.Fields.Where(x => x.FieldType != FieldType.ItemReferenceArray && x.FieldType != FieldType.ItemReference).Select(x => ProcessorUtils.GetTypeScriptFieldTypeForImport(x.FieldType)).Where(x => !string.IsNullOrEmpty(x)).ToArray() ?? [];
                HashSet<string> uniqueValues = new HashSet<string>(allFieldsTypes);
                string importLine = string.Join(", ", uniqueValues.ToArray().Order());

                if(!string.IsNullOrWhiteSpace(importLine))
                {
                    importLine = $"import {{ {importLine} }} from '@sitecore-jss/sitecore-jss-nextjs';";
                }
                result.AppendLine(importLine);

                var hasItemReference = item.Fields.Any(x => x.FieldType == FieldType.ItemReferenceArray || x.FieldType == FieldType.ItemReference);
                if (hasItemReference)
                {
                    result.AppendLine($"import {{ {ConfigLoader.GetBaseItemClassName()} }} from '{ConfigLoader.GetBaseItemClassImportRelativePath()}';");
                }
            }

            result.AppendLine();
            result.Append($"export type {item.ClassName} =");
            if (item?.InheritenceClasses?.Length > 0)
            {
                result.Append(" " + string.Join(" & ", item.InheritenceClasses.Select(x => x.ClassName)));
            }

            if(item?.Fields?.Length > 0)
            {
                if (item?.InheritenceClasses?.Length > 0)
                {
                    result.Append(" &");
                }

                var fieldStrings = item?.Fields?.Select(x => x.ToString()).Where(x => !string.IsNullOrWhiteSpace(x)) ?? [];

                result.AppendLine(" {");
                result.AppendLine("  fields: {");
                result.AppendLine("    " + string.Join($"{Environment.NewLine}    ", fieldStrings));
                result.AppendLine("  };");
                result.Append("}");
            }

            result.Append(";");
            result.AppendLine();

            return result.ToString();
        }
    }
}
