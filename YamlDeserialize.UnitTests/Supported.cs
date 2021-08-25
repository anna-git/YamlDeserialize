using System.IO;
using Xunit;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using YamlDotNet.Serialization.NodeTypeResolvers;

namespace YamlDeserialize.UnitTests
{
    public class Supported
    {
        [Theory]
        [InlineData("rules")]
        [InlineData("tags")]
        [InlineData("aliases")]
        [InlineData("basic")]
        [InlineData("comments")]
        public void Deserialize(string filename)
        {
            using (var sr = new StreamReader($"{filename}.yml"))
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).
                    Build();
                var resultObj = deserializer.Deserialize(sr);
                var result = resultObj as Dictionary<object, object>;
                sr.BaseStream.Seek(0, SeekOrigin.Begin);
                var ourDserializer = new YamlDeserializer.Serialization.DeserializerBuilder().WithNamingConvention(YamlDeserializer.Serialization.NamingConventions.CamelCaseNamingConvention.Instance).Build();
                var resultddObj = ourDserializer.Deserialize(sr);
                var resultdd = resultddObj as Dictionary<object, object>;
                Xunit.Assert.NotNull(result);
                Xunit.Assert.NotNull(resultdd);

                bool aresame = Helper.CompareDictionaries(result, resultdd);
                Xunit.Assert.True(aresame);
            }
        }

     
    }
}
