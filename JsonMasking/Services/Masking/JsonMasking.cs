using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace JsonMasking.Services.Masking
{
    public static class JsonMasking
    {
        public static string MaskFields(this string json, string[] blacklist, string mask)
        {
            if (string.IsNullOrWhiteSpace(json) == true)
            {
                throw new ArgumentNullException(nameof(json));
            }

            if (blacklist == null)
            {
                throw new ArgumentNullException(nameof(blacklist));
            }

            if (blacklist.Any() == false)
            {
                return json;
            }

            return MaskFields(blacklist);

            return json;
        }

        //public string MaskFields(object obj, string[] blacklist, string mask)
        //{
        //    return Mask(obj, blacklist, mask);
        //}

        //public string MaskArrayFields(object obj, string[] blacklist, string mask)
        //{

        //    var jsonArrayObject = JArray.FromObject(obj);

        //    var x = jsonArrayObject[0] as JObject;

        //    var jsonObject = JObject.FromObject(x);
        //    var jsonObject = obj as JToken;

        //    MaskFieldsFromJToken(jsonObject, blacklist, mask);

        //    return jsonObject.ToString();
        //}

        //private void MaskFieldsFromJToken(JToken token, string[] blacklist, string mask)
        //{
        //    var container = token as JContainer;
        //    if (container == null)
        //    {
        //        return; // abort recursive
        //    }

        //    var removeList = new List<JToken>();
        //    foreach (var jtoken in container.Children())
        //    {
        //        if (jtoken is JProperty prop)
        //        {
        //            var matching = blacklist.Any(item =>
        //            {
        //                return
        //                    Regex.IsMatch(prop.Path, WildCardToRegular(item), RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        //            });

        //            if (matching)
        //            {
        //                removeList.Add(jtoken);
        //            }
        //        }

        //        call recursive
        //        MaskFieldsFromJToken(jtoken, blacklist, mask);
        //    }

        //    replace
        //    foreach (var el in removeList)
        //    {
        //        var prop = (JProperty)el;
        //        prop.Value = mask;
        //    }
        //}

        //private string WildCardToRegular(string value)
        //{
        //    return "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
        //}

        public static string MaskFields(this object jsonObject, string[] sensetiveFields, string mask = "****")
        {
            if (jsonObject is null)
            {
                throw new ArgumentNullException(nameof(jsonObject));
            }

            if (sensetiveFields is null)
            {
                throw new ArgumentNullException(nameof(sensetiveFields));
            }

            return domask(jsonObject, sensetiveFields, mask);
        }

        private static string domask(object jsonObject, string[] sensetiveFields, string mask)
        {
            if (!sensetiveFields.Any())
            {
                return ((JContainer)JToken.FromObject(jsonObject)).ToString();
            }

            var jObject = (JContainer)JToken.FromObject(jsonObject);

            foreach (var item in jObject.DescendantsAndSelf()
                                        .OfType<JProperty>()
                                        .Where(x => sensetiveFields.Contains(x.Name)))
            {
                item.Value = mask;
            }

            return jObject.ToString();
        }
    }

}