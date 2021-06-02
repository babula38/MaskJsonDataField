using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace JsonMasking.Services
{
    /// <summary>
    /// Json Masking Extension
    /// </summary>
    public static class JsonMasking
    {
        /// <summary>
        /// Mask fields
        /// </summary>
        /// <param name="json">json to mask properties</param>
        /// <param name="blacklist">insensitive property array</param>
        /// <param name="mask">mask to replace property value</param>
        /// <returns></returns>
        public static string MaskFields(this string json, string[] blacklist, string mask)
        {
            if (string.IsNullOrWhiteSpace(json))
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

            var token = JToken.Parse(json);

            if (token is JArray)
            {
                throw new Exception("Array object is not supported");
            }

            if (token is JObject jObject)
            {
                return jObject.MaskFields(blacklist, mask);
            }

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

        public static string MaskFields(this object jsonObject, string[] sensitiveFields, string mask = "****")
        {
            if (jsonObject is null)
            {
                throw new ArgumentNullException(nameof(jsonObject));
            }

            if (sensitiveFields == null || sensitiveFields.Length == 0)
                return ((JContainer)JContainer.FromObject(jsonObject)).ToString();

            var jObject = (JContainer)JContainer.FromObject(jsonObject);

            foreach (var item in jObject.DescendantsAndSelf()
                                        .OfType<JProperty>()
                                        .Where(x => sensitiveFields.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)))
            {
                item.Value = mask;
            }

            return jObject.ToString();
        }

        public static string MaskFields(this object jsonObject, string sensitiveFields)
        {
            var fields = sensitiveFields.Split(";");

            return jsonObject.MaskFields(fields);
        }
    }

}