// See https://aka.ms/new-console-template for more information

using SitecoreTypeScriptGenerator;
using SitecoreTypeScriptGenerator.Processor;

ProcessorUtils.RootGenerationPath = ConfigLoader.GetRootGenerationPath() ?? string.Empty;

Console.WriteLine();
Console.WriteLine("[info] Loading Yaml files");
LoadYamls.Load(ConfigLoader.GetYamlLoadPaths(), ConfigLoader.GetYamlExcludePaths());
Console.WriteLine("[info] Yaml files loaded");

var filesWritten = GenerateTypeScriptFiles2.Generate();
Console.WriteLine();
Console.WriteLine($"Finshed. {filesWritten} files written");
Console.WriteLine();
Console.WriteLine("~~~ I recommend linting after generating these files!");
Console.WriteLine();