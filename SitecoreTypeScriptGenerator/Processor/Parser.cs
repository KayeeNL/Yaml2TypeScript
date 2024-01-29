
using YamlDotNet.Serialization;

namespace SitecoreTypeScriptGenerator.Processor
{
    internal class ItemParser
    {
        private static readonly IDeserializer _deserializer = new DeserializerBuilder().IgnoreUnmatchedProperties().Build();

        public static T? ParseFile<T>(string path) where T : Item
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
            catch(Exception)
            {
                Console.WriteLine($"failed to parse file: {path}");
            }

            return null;
        }
    }
}
