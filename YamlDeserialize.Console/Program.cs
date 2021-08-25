using System.IO;
using System.Reflection;
using YamlDeserializer.Serialization;
using YamlDeserializer.Serialization.NamingConventions;

namespace YamlDeserialize.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).Build();
            var sr = Assembly.GetExecutingAssembly().GetManifestResourceStream("YamlDeserialize.Console.rules.yml");
            var reader = new StreamReader(sr);
            var res = deserializer.Deserialize(reader);
         }
    }
}
