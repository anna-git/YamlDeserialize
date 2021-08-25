using System.IO;
using Xunit;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using System;

namespace YamlDeserialize.UnitTests
{
    public class NotSupported
    {
        /// <summary>
        /// Ours doesnt support ... or ---
        /// </summary>
        /// <param name="filename"></param>
        [Theory]
        [InlineData("basic.start")]
        [InlineData("basic.end")]
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
                Action act = () =>  ourDserializer.Deserialize(sr);
                Assert.Throws<YamlDeserializer.Core.SemanticErrorException>(act);
            }
        }


    }
}
