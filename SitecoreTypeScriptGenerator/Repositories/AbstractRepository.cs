
using SitecoreTypeScriptGenerator.Processor;

namespace SitecoreTypeScriptGenerator.Repositories
{
    internal abstract class AbstractRepository<T> where T : Item
    {
        private const string TemplatesRoot = "/sitecore/templates";

        internal abstract bool CreateFolder { get; }

        internal abstract Dictionary<Guid, T> Items { get; }
        public virtual void TryAdd(Guid id, T item)
        {
            SetCalculatedModel(ProcessorUtils.GetRootGenerationDirectory(), item);

            Items.TryAdd(id, item);
        }

        public virtual IEnumerable<T> GetValues()
        {
            return Items.Values;
        }

        public virtual bool TryGetValue(Guid id, out T value)
        {
            return Items.TryGetValue(id, out value);
        }

        public virtual void SetCalculatedModel(DirectoryInfo directoryInfo, Item item)
        {
            // set all names first
            item.RawName = GetRawName(item);
            item.Name = GetName(item);
            item.ClassName = GetClassName(item);
            
            // set all paths after names have been set
            item.FileName = GetFileName(item);
            item.FileFullPath = GetFileFullPath(directoryInfo, item, CreateFolder);
            item.ImportRelativePath = GetImportPath(directoryInfo, item);
        }

        private string GetClassName(Item item)
        {
            string name = GetName(item);
            if (name.StartsWith("_"))
            {
                name = $"I{name.Substring(1)}";
            }
            return name;
        }

        private string GetRawName(Item item)
        {
            var tokens = item.Path.Split('/');
            return tokens[tokens.Length - 1];
        }

        private string GetName(Item item)
        {
            string rawName = GetRawName(item) ?? string.Empty;
            return rawName.Replace(" ", string.Empty).Replace("-", "_");
        }

        private string GetFileName(Item template)
        {
            var tokens = template?.Path?.Replace(TemplatesRoot, string.Empty)?.Split("/");
            if (tokens?.Length == 0)
            {
                throw new Exception("invalid template path");
            }

            return $"{template.ClassName}.type.ts";
        }

        private string GetFileFullPath(DirectoryInfo rootPath, Item template, bool createFolder)
        {
            var tokens = template?.Path?.Replace(TemplatesRoot, string.Empty)?.Split("/");
            if (tokens?.Length == 0)
            {
                throw new Exception("invalid template path");
            }

            string folderPath = string.Join("/", tokens.Take(tokens.Length - 1));
            string fileName = $"{template.ClassName}.type.ts";

            string fullPath = Path.Join(rootPath.FullName, folderPath.Replace("/", "\\"));
            var newDirectory = new DirectoryInfo(fullPath);
            if (createFolder && !newDirectory.Exists)
            {
                newDirectory.Create();
            }
            return Path.Combine(newDirectory.FullName, fileName);
        }

        private string GetImportPath(DirectoryInfo rootPath, Item item)
        {
            string relativePath = item.FileFullPath.Replace(rootPath.Parent.FullName, string.Empty).Replace("\\", "/");
            if (relativePath.StartsWith("/"))
            {
                relativePath = relativePath.Substring(1);
            }
            return relativePath;
        }
    }
}
