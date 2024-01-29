using System.Configuration;

namespace SitecoreTypeScriptGenerator
{
    internal class ConfigLoader
    {
        public static string? GetRootGenerationPath()
        {
            return ConfigurationManager.AppSettings["GeneratedFilesOutputPath"];
        }

        public static string[] GetYamlLoadPaths()
        {
            var rawValue = ConfigurationManager.AppSettings["YamlIncludePathsPiped"] ?? string.Empty;
            var entries = rawValue.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim());
            return entries.ToArray();
        }

        public static string GetBaseItemClassName()
        {
            return ConfigurationManager.AppSettings["BaseItemClassName"] ?? "Item";
        }

        public static string GetBaseItemClassImportRelativePath()
        {
            return ConfigurationManager.AppSettings["BaseItemClassImportRelativePath"] ?? "noPathSet/Missing.ts";
        }
    }
}
