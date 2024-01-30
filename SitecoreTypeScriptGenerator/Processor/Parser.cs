
using YamlDotNet.Serialization;

namespace SitecoreTypeScriptGenerator.Processor
{
    internal class ItemParser
    {
        private static readonly IDeserializer _deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();

        public static T? ParseFile<T>(string path, bool logDetails = false) where T : Item
        {
            if(!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            try
            {
                string yaml = File.ReadAllText(path);
                var item = _deserializer.Deserialize<T>(yaml);
                return item;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"[warning] failed to parse file: {path}");
                if (logDetails)
                {
                    Console.WriteLine();
                    Console.WriteLine(ex);
                    Console.WriteLine();
                }
            }

            return null;
        }
    }
}
