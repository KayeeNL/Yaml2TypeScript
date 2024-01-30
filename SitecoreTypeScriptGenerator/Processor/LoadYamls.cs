
using SitecoreTypeScriptGenerator.Models;
using SitecoreTypeScriptGenerator.Repositories;

namespace SitecoreTypeScriptGenerator.Processor
{
    internal class LoadYamls
    {
        private const string YamlSearchPattern = "*.yml";

        private const string FieldTypeId = "ab162cc0-dc80-4abf-8871-998ee5d7ba32";
        
        private const string TemplateSectionId = "e269fbb5-3750-427a-9149-7aa950b49301";

        public static void Load(string[] rootPaths, string[] excludePaths)
        {
            ProcessorUtils.GetRootGenerationDirectory();

            foreach (var path in rootPaths)
            {
                var directory = new DirectoryInfo(path);
                LoadFolder(directory, excludePaths);
            }
        }       

        private static void LoadFolder(DirectoryInfo folder, string[] excludePaths)
        {
            var repoItems = new ItemRepository();
            var repoSections = new FieldSectionRepository();
            var repoFields = new FieldRepository();

            if(excludePaths.Any(x => folder.FullName.IndexOf(x, StringComparison.InvariantCultureIgnoreCase) > 0))
            {
                Console.WriteLine($"[info] folder excluded: {folder.FullName}");
                return;
            }

            var files = folder.GetFiles(YamlSearchPattern) ?? [];
            foreach (var file in files) 
            {
                try
                {
                    var item = ItemParser.ParseFile<Item>(file.FullName);
                    if (item?.ID == null)
                    {
                        continue;
                    }

                    repoItems.TryAdd(new Guid(item.ID), item);

                    var matchIsField = item?.SharedFields?.Any(x => x?.ID == FieldTypeId) ?? false;
                    if (matchIsField)
                    {
                        // reparse as field
                        var field = ItemParser.ParseFile<Field>(file.FullName);
                        repoFields.TryAdd(new Guid(field.ID), field);
                    }

                    var matchIsTemplate = ProcessorUtils.DoesItemHaveBaseTemplates(item);
                    if (matchIsTemplate)
                    {
                        CreateTemplateEntry(folder, item);
                    }

                    var matchIsTemplateSection = item?.Template == TemplateSectionId;
                    if(matchIsTemplateSection)
                    {
                        var section = ItemParser.ParseFile<TemplateSection>(file.FullName);
                        repoSections.TryAdd(new Guid(section.ID), section);
                        repoItems.TryAdd(new Guid(section.ID), section);
                        section.FieldItems = LoadYamlFields(folder, item.RawName);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Process failed: {file.FullName}");
                }
            }

            foreach(var sub in folder.GetDirectories())
            {
                LoadFolder(sub, excludePaths);
            }
        }

        private static void CreateTemplateEntry(DirectoryInfo folder, Item item)
        {
            new ClassRepository().TryAdd(new Guid(item.ID), item);

            var templateFolders = folder.GetDirectories(item.RawName);
            if(templateFolders.Length != 1)
            {
                // no sections for template
                // may only have base templates (no fields)
                return;
            }

            var subFolder = templateFolders[0];

            item.SectionReferences = LoadYamlSections(subFolder);
        }

        private static List<Item>? LoadYamlSections(DirectoryInfo folder)
        {
            List<Item> results = new List<Item>();
            var files = folder.GetFiles(YamlSearchPattern) ?? [];
            foreach (var file in files)
            {
                var item = ItemParser.ParseFile<Item>(file.FullName);
                if (item?.ID == null)
                {
                    continue;
                }
                results.Add(item);
            }
            return results;
        }

        private static List<Item>? LoadYamlFields(DirectoryInfo folder, string name)
        {
            var subFolders = folder.GetDirectories(name);
            if(subFolders.Length != 1)
            {
                //throw new Exception("Expected fields folder. Something is wrong");
                Console.WriteLine($"[warning] Expected fields folder: {folder} name: {name}");
                return null;
            }

            List<Item> results = new List<Item>();
            var repoItems = new ItemRepository();
            var repoFields = new FieldRepository();

            var files = subFolders[0].GetFiles(YamlSearchPattern) ?? [];
            foreach (var file in files)
            {
                var item = ItemParser.ParseFile<Item>(file.FullName);
                if (item?.ID == null)
                {
                    continue;
                }

                var matchIsField = item?.SharedFields?.Any(x => x?.ID == FieldTypeId) ?? false;
                if (!matchIsField)
                {
                    throw new Exception("Expected field file. Something is wrong");
                }

                repoItems.TryAdd(new Guid(item.ID), item);

                var field = ItemParser.ParseFile<Field>(file.FullName);
                repoFields.TryAdd(new Guid(field.ID), field);

                results.Add(field);
            }

            return results;
        }


        public static List<string> GetBaseTemplateNames(Item item)
        {
            List<string> result = new List<string>();
            var repoItems = new ItemRepository();

            var ids = ProcessorUtils.GetBaseTemplateIds(item) ?? new List<Guid>();
            foreach (var sub in ids)
            {
                if (repoItems.TryGetValue(sub, out var subItem))
                {
                    result.Add(subItem.ClassName);
                }
            }
            return result;
        }
    }
}
