using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

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
