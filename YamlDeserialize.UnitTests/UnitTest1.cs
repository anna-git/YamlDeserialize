using System.IO;
using Xunit;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using System.Collections.Generic;
using YamlDotNet.Serialization.NodeTypeResolvers;

namespace YamlDeserialize.UnitTests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("rules")]
        [InlineData("tags")]
        [InlineData("test")]
        public void DeserializeSimple(string filename)
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

                bool v = CompareDictionaries(result, resultdd);
                Xunit.Assert.True(v);
            }
        }

        public bool CompareDictionaries(IDictionary<object, object> model, IDictionary<object, object> dic2)
        {
            if (model.Count != dic2.Count)
            {
                return false;
            }
            for (int i = 0; i < model.Count; i++)
            {
                var current = model.ElementAt(i);
                var other = dic2.ElementAt(i);
                if (string.Compare(current.Key.ToString(), other.Key.ToString()) != 0)
                {
                    return false;
                }
                if (current.Value is string s && other.Value is string s2)
                {
                    if (s != s2)
                        return false;
                }
                else
                {
                    if (current.Value.GetType() != other.Value.GetType())
                        return false;
                    if (current.Value is List<object> cv)
                    {
                        var res = CompareList(cv, (List<object>)other.Value);
                        if (!res)
                            return false;
                    }
                    if (current.Value is IDictionary<object, object> cdic)
                    {
                        var res = CompareDictionaries(cdic, (IDictionary<object, object>)other.Value);
                        if (!res)
                            return false;
                    }
                }
            }
            return true;
        }

        private bool CompareList(List<object> current, List<object> other, int index = 0)
        {
            var length = current.Count;
            for (int i = 0; i < length; i++)
            {
                if (current[i].GetType() != other[i].GetType())
                    return false;
                else if(current[i] is IDictionary<object, object> innerdic)
                {
                    var res = CompareDictionaries(innerdic, other[i] as IDictionary<object, object>);
                    if (!res)
                        return false;
                }
                else if (current[i] is string ci)
                {
                    if (string.Compare(ci, (string)other[i], false) != 0)
                        return false;
                }
                else if (current[i] != other[i])
                    return false;
            }
            return true;
        }
    }
}
