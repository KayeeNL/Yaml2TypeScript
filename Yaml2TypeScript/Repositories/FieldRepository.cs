
using Yaml2TypeScript.Models.TypeScript;
using Yaml2TypeScript.Processor;

namespace Yaml2TypeScript.Repositories
{
    internal class FieldRepository : AbstractRepository<Item>
    {
        private static Dictionary<Guid, Item> Cache = new Dictionary<Guid, Item>();
        internal override Dictionary<Guid, Item> Items => Cache;

        internal override bool CreateFolder => false;

        public override void TryAdd(Guid id, Item item)
        {
            Field field = item as Field;
            if(field == null)
            {
                return;
            }

            var typeField = field?.SharedFields?.FirstOrDefault(x => x.ID == "ab162cc0-dc80-4abf-8871-998ee5d7ba32");

            field.FieldType = GetTypeFieldValue(typeField);

            base.TryAdd(id, field);
        }

        private FieldType GetTypeFieldValue(FieldReference? typeField)
        {
            string typeFieldValue = typeField?.Value ?? string.Empty;

            switch (typeFieldValue)
            {
                case "Image":
                    return FieldType.Image;
                case "Single-Line Text":
                    return FieldType.SingleLineText;
                case "Multi-Line Text":
                    return FieldType.MultiLineText;
                case "Rich Text":
                    return FieldType.RichText;
                case "General Link":
                case "File":
                    return FieldType.GeneralLink;
                case "Droplink":
                case "Droptree":
                    return FieldType.ItemReference;
                case "Treelist":
                case "Multilist":
                case "Multilist with Search":
                case "Droplist":
                case "TreelistEx":
                    return FieldType.ItemReferenceArray;
                case "Checkbox":
                    return FieldType.Checkbox;
                case "Integer":
                    return FieldType.Integer;
                case "Number":
                    return FieldType.Number;
                case "Date":
                case "Datetime":
                    return FieldType.Date;
                case "Lookup Name Lookup Value List":
                    // this field type is likely not used.
                    return FieldType.ItemReference;
                default:
                    Console.WriteLine($"Unknown field type: {typeFieldValue}");
                    return FieldType.Unknown;
            }
        }
    }
}
