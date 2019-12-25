using System.Collections.Generic;

namespace personalWebsiteBackend.Utils
{
    public static class DictionaryUtils
    {
        public static T GetSafe<T>(this IDictionary<string, T> dictionary, string key)
        {
            if (dictionary == null)
                return default(T);

            if (!dictionary.ContainsKey(key))
            {
                return default(T);
            }
            else
            {
                return dictionary[key];
            }
        }
    }
}