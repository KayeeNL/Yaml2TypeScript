


using Microsoft.Extensions.Configuration;

namespace SitecoreTypeScriptGenerator
{
    internal class ConfigLoader
    {
        internal static IConfiguration Config { get; private set; }
        private static AppConfigModel Model;

        private const string DefaultBaseItemClassName = "BaseItem";

        static ConfigLoader()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
            .Build();

            Console.WriteLine("[info] loading config file");

            string workingDirectory = System.Environment.CurrentDirectory;
            string generatedFilesOutputPath = Config["GeneratedFilesOutputPath"];
            if (generatedFilesOutputPath != null &&  generatedFilesOutputPath.StartsWith(".\\"))
            {
                generatedFilesOutputPath = Path.Combine(workingDirectory, generatedFilesOutputPath.Substring(2));
            }

            string overrideBaseItemClassName = Config["OverrideBaseItemClassName"];
            string overrideBaseItemImportPath = Config["OverrideBaseItemClassImportRelativePath"];

            Console.WriteLine($"overrideBaseItemClassName: {overrideBaseItemClassName}");
            Console.WriteLine($"overrideBaseItemImportPath: {overrideBaseItemImportPath}");

            if (string.IsNullOrWhiteSpace(overrideBaseItemClassName) &&
                string.IsNullOrWhiteSpace(overrideBaseItemImportPath))
            {
                var folderRoot = new DirectoryInfo(generatedFilesOutputPath);
                overrideBaseItemClassName = DefaultBaseItemClassName;
                overrideBaseItemImportPath = $"{folderRoot.Name}/{overrideBaseItemClassName}.type.ts";

                string baseClassFilePath = Path.Combine(generatedFilesOutputPath, $"{overrideBaseItemClassName}.type.ts");

                Console.WriteLine($"Generating default base class: {baseClassFilePath}");
                File.WriteAllText(baseClassFilePath, $"export type {overrideBaseItemClassName} = {{}};");
            }

            Model = new AppConfigModel
            {
                ApplicationWorkingDirectory = System.Environment.CurrentDirectory,
                GeneratedFilesOutputPath = generatedFilesOutputPath,
                YamlIncludePathsPiped = GetYamlLoadPaths(workingDirectory, Config.GetSection("YamlIncludePathsPiped")),
                YamlExcludePathsPiped = GetYamlLoadPaths(workingDirectory,Config.GetSection("YamlExcludePathsPiped")),
                OverrideBaseItemClassName = overrideBaseItemClassName,
                OverrideBaseItemClassImportRelativePath = overrideBaseItemImportPath
            };

            Console.WriteLine("[info] config file loaded");
        }

        public static string? GetRootGenerationPath()
        {
            return Model.GeneratedFilesOutputPath;
        }

        private static string?[]? GetYamlLoadPaths(string workingDirectory, IConfigurationSection configSection)
        {
            var paths = configSection?.GetChildren()?.Select(x => x.Value)?.ToArray() ?? new string?[0];
            
            if(!paths.Any()) 
            {
                return paths;
            }

            List<string> finalList = new List<string>();
            foreach(string p in paths)
            {
                if(p!= null && p.StartsWith(".\\"))
                {
                    finalList.Add(Path.Combine(workingDirectory, p.Substring(2)));
                }
                else
                {
                    finalList.Add(p);
                }
            }

            return finalList.ToArray();
        }

        public static string?[] GetYamlLoadPaths()
        {
            return Model.YamlIncludePathsPiped ?? new string?[0];
        }

        public static string?[] GetYamlExcludePaths()
        {
            return Model.YamlExcludePathsPiped ?? new string?[0];
        }

        public static string GetBaseItemClassName()
        {
            return Model?.OverrideBaseItemClassName ?? "Item";
        }

        public static string GetBaseItemClassImportRelativePath()
        {
            return Model.OverrideBaseItemClassImportRelativePath ?? "noPathSet/Missing.ts";
        }
    }
}
