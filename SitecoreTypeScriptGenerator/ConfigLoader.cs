


using Microsoft.Extensions.Configuration;

namespace SitecoreTypeScriptGenerator
{
    internal class ConfigLoader
    {
        internal static IConfiguration Config { get; private set; }
        private static AppConfigModel Model;

        static ConfigLoader()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
            .Build();

            string workingDirectory = System.Environment.CurrentDirectory;
            string generatedFilesOutputPath = Config["GeneratedFilesOutputPath"];
            if(generatedFilesOutputPath != null &&  generatedFilesOutputPath.StartsWith(".\\"))
            {
                generatedFilesOutputPath = Path.Combine(workingDirectory, generatedFilesOutputPath.Substring(2));
            }

            Model = new AppConfigModel
            {
                ApplicationWorkingDirectory = System.Environment.CurrentDirectory,
                GeneratedFilesOutputPath = generatedFilesOutputPath,
                YamlIncludePathsPiped = GetYamlLoadPaths(workingDirectory, Config.GetSection("YamlIncludePathsPiped")),
                BaseItemClassName = Config["BaseItemClassName"],
                BaseItemClassImportRelativePath = Config["BaseItemClassImportRelativePath"]
            };
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

        public static string GetBaseItemClassName()
        {
            return Model?.BaseItemClassName ?? "Item";
        }

        public static string GetBaseItemClassImportRelativePath()
        {
            return Model.BaseItemClassImportRelativePath ?? "noPathSet/Missing.ts";
        }
    }
}
