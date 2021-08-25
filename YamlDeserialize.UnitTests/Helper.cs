using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YamlDeserialize.UnitTests
{
    public class Helper
    {
        public static bool CompareDictionaries(IDictionary<object, object> model, IDictionary<object, object> dic2)
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

        private static bool CompareList(List<object> current, List<object> other, int index = 0)
        {
            var length = current.Count;
            for (int i = 0; i < length; i++)
            {
                if (current[i].GetType() != other[i].GetType())
                    return false;
                else if (current[i] is IDictionary<object, object> innerdic)
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
