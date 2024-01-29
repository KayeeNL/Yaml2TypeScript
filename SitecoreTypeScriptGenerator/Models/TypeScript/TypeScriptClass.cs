
namespace SitecoreTypeScriptGenerator.Models.TypeScript
{
    internal class TypeScriptClass
    {
        public Guid SitecoreId { get; set; }
        public Guid[]? SitecoreInheritenceIds { get; set; }
        public Guid[]? SitecoreSectionRefernceIds { get; set; }
        public string? ClassName { get; set; }
        public string? FilePath { get; set; }

        public TypeScriptClass[]? InheritenceClasses { get; set; }
        public TypeScriptField[]? Fields { get; set; }
        public string? ImportString { get; set; }
        public string? RelativeFilePath { get; set; }

        public bool IsSectionNotClass { get; set; }
        
    }
}
