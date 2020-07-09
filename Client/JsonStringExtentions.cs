using Newtonsoft.Json;
using System;

namespace Client
{
    public static class JsonStringExtentions
    {
        public static T ToTypedObject<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string ToJsonString(this object @object)
        {
            if (@object is string) throw new ArgumentException("value can not be string", nameof(@object));
            return JsonConvert.SerializeObject(@object);
        }
    }
}
