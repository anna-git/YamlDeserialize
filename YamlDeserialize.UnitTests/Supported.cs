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
        [InlineData("basic.start")]
        [InlineData("basic.end")]
        public void Deserialize(string filename)
        {
            Dictionary<object, object> yamlDotNetResult = null;
            Dictionary<object, object> ddyamlDotNetResult = null;
            using (var sr = new StreamReader($"{filename}.yml"))
            {
                var deserializer = new DeserializerBuilder().WithNamingConvention(CamelCaseNamingConvention.Instance).
                    Build();
                var resultObj = deserializer.Deserialize(sr);
                 yamlDotNetResult = resultObj as Dictionary<object, object>;
            }
            using (var sr = new StreamReader($"{filename}.yml"))
            {
                var ourDserializer = new YamlDeserializer.Serialization.DeserializerBuilder().WithNamingConvention(YamlDeserializer.Serialization.NamingConventions.CamelCaseNamingConvention.Instance).Build();
                var resultddObj = ourDserializer.Deserialize(sr);
                ddyamlDotNetResult = resultddObj as Dictionary<object, object>;
            }
            Xunit.Assert.NotNull(yamlDotNetResult);
            Xunit.Assert.NotNull(ddyamlDotNetResult);

            bool aresame = Helper.CompareDictionaries(yamlDotNetResult, ddyamlDotNetResult);
            Xunit.Assert.True(aresame);
        }
    }
}
