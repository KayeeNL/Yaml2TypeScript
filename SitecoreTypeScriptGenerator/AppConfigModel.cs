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

        public string? OverrideBaseItemClassName { get; set; }
        public string? OverrideBaseItemClassImportRelativePath { get; set; }


        public string? ApplicationWorkingDirectory { get; set; }
    }
}
