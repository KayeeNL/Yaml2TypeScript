// See https://aka.ms/new-console-template for more information

using SitecoreTypeScriptGenerator;
using SitecoreTypeScriptGenerator.Processor;

ProcessorUtils.RootGenerationPath = ConfigLoader.GetRootGenerationPath() ?? string.Empty;

LoadYamls.Load(ConfigLoader.GetYamlLoadPaths());

var filesWritten = GenerateTypeScriptFiles2.Generate();
Console.WriteLine();
Console.WriteLine($"Completed. {filesWritten} files written");
Console.WriteLine();
Console.WriteLine("! I recommend linting after generating these files!");