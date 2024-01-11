// See https://aka.ms/new-console-template for more information

using Yaml2TypeScript.Processor;

ProcessorUtils.RootGenerationPath = @"c:\data\projects\cnh\generatedTypeScript";

LoadYamls.Load(
    [ @"C:\data\projects\cnh\repo\src\items\Foundation", 
    @"C:\data\projects\cnh\repo\src\items\Feature",
    @"C:\data\projects\cnh\repo\src\items\Project\Custom\items\Custom Templates"]);

var filesWritten = GenerateTypeScriptFiles2.Generate();
Console.WriteLine();
Console.WriteLine($"Completed. {filesWritten} files written");
Console.WriteLine();