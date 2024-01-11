
using System.Text;
using Yaml2TypeScript.Models.TypeScript;
using Yaml2TypeScript.Processor;

namespace Yaml2TypeScript.Repositories
{
    internal class TypeScriptClassRepository
    {
        internal static Dictionary<Guid, TypeScriptClass> Items = new Dictionary<Guid, TypeScriptClass>();

        internal void CreateTypes(ClassRepository repo, FieldSectionRepository repoFieldSection)
        {
            HashSet<Guid> baseTemplateIds = new HashSet<Guid>();

            var repoItems = repo.GetValues();
            foreach (var item in repoItems) 
            {
                Guid key = new Guid(item.ID);
                var tsClass = new TypeScriptClass
                {
                    ClassName = item.ClassName,
                    SitecoreId = key,
                    FilePath = item.FileFullPath,
                    RelativeFilePath = item.ImportRelativePath,
                    SitecoreInheritenceIds = ProcessorUtils.GetBaseTemplateIds(item)?.ToArray() ?? [],
                    SitecoreSectionRefernceIds = item?.SectionReferences?.Select(x => new Guid(x.ID)).ToArray() ?? [],
                    Fields = CreateFields(item, repoFieldSection),
                };

                foreach (var baseId in tsClass.SitecoreInheritenceIds)
                {
                    baseTemplateIds.Add(baseId);
                }                

                Items.Add(key, tsClass);
            }

            RecreateInheritance();
        }

        private void RecreateInheritance()
        {
            foreach (var tsClass in Items.Values)
            {
                CalculateInheritance(tsClass);
            }
        }

        private TypeScriptField[] CreateFields(Item item, FieldSectionRepository repoFieldSection)
        {
            var result = new List<TypeScriptField>();

            foreach (var sc in item.SectionReferences ?? [])
            {
                if (repoFieldSection.TryGetValue(new Guid(sc.ID), out Item fieldSection))
                {
                    foreach (Field field in fieldSection.FieldItems ?? [])
                    {
                        result.Add(new TypeScriptField
                        {
                            Name = field.Name,
                            FieldType = field.FieldType,
                            SitecoreId = new Guid(field.ID)
                        });

                    }
                }
            }
            return result.ToArray();
        }

        private TypeScriptField[] CreateFields(List<Item>? fieldItems)
        {
            if (fieldItems == null || fieldItems?.Count == 0)
            {
                return new TypeScriptField[0];
            }

            var result = new List<TypeScriptField>();
            foreach(Field field in fieldItems)
            {
                result.Add(new TypeScriptField 
                { 
                    Name = field.Name, 
                    FieldType = field.FieldType, 
                    SitecoreId = new Guid(field.ID) 
                });
            }
            return result.ToArray();
        }

        internal TypeScriptClass[] GetValues()
        {
            return Items.Values.ToArray();
        }

        private void CalculateInheritance(TypeScriptClass tsClass)
        {
            tsClass.InheritenceClasses = tsClass.SitecoreInheritenceIds?.Select(LookupClass)?.Where(x => x != null).ToArray();
            tsClass.ImportString = CreateImportString(tsClass);
        }

        private string? CreateImportString(TypeScriptClass tsClass)
        {
            if(tsClass?.InheritenceClasses == null || tsClass.InheritenceClasses.Length == 0)
            {
                return null;
            }

            StringBuilder result = new StringBuilder();
            foreach (var cs in tsClass.InheritenceClasses)
            {
                result.AppendLine($"import { "{ " + cs.ClassName + " }"} from '{cs.RelativeFilePath}';");
            }

            return result.ToString();
        }

        private TypeScriptClass? LookupClass(Guid guid)
        {
            if (!Items.ContainsKey(guid))
            {
                if (guid != new Guid("1930bbeb-7805-471a-a3be-4858ac7cf696") &&
                    guid != new Guid("8ca06d6a-b353-44e8-bc31-b528c7306971"))
                {
                    // system template
                    Console.WriteLine($"Missing key.....{guid}");
                }
                return null;
            }

            return Items[guid];
        }
    }
}
