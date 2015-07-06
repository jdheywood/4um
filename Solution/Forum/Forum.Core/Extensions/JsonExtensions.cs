using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Forum.Core.Extensions
{
    public static class JsonExtensions
    {
        public static TEntity FromJson<TEntity>(this string json)
        {
            return JsonConvert.DeserializeObject<TEntity>(json);
        }

        public static TPropertyType GetPropertyValue<TPropertyType>(this JObject jsonObject, string propertyName)
            where TPropertyType : JToken
        {
            return (TPropertyType)jsonObject.Properties().Single(p => p.Name == propertyName).Value;
        }

        public static TValue GetValue<TValue>(this JProperty jsonProperty)
             where TValue : IConvertible
        {
            var jsonValue = jsonProperty.Value as JValue;

            return (jsonValue == null || jsonValue.Value == null)
                ? default(TValue)
                : (TValue)jsonValue.Value;
        }

        public static string ToJson(this object objectToSerialize)
        {
            return JsonConvert.SerializeObject(objectToSerialize);
        }

        public static string ToJson(this object objectToSerialize, bool ignoreNulls)
        {
            return
                ignoreNulls
                ? JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
                : objectToSerialize.ToJson();
        }
    }
}
