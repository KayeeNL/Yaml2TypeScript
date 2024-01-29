using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SitecoreTypeScriptGenerator
{
    public class AppConfigModel
    {
        public string? GeneratedFilesOutputPath { get; set; }
        public string?[]? YamlIncludePathsPiped { get; set; }
        public string? BaseItemClassName { get; set; }
        public string? BaseItemClassImportRelativePath { get; set; }


        public string? ApplicationWorkingDirectory { get; set; }
    }
}
