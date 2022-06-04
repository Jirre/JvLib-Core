using System;
using Newtonsoft.Json;

namespace UnityEngine
{
    public static class ScriptableObjectExtensions
    {
        public static void FromJson(this ScriptableObject pTarget, string pJsonString, JsonSerializerSettings pSettings = null)
        {
            if (pSettings == null)
            {
                JsonConvert.PopulateObject(pJsonString, pTarget);
                return;
            }
            JsonConvert.PopulateObject(pJsonString, pTarget, pSettings);
        }

        public static string ToJSon(this ScriptableObject pTarget, JsonSerializerSettings pSettings = null)
        {
            return pSettings == null ? 
                JsonConvert.SerializeObject(pTarget) : 
                JsonConvert.SerializeObject(pTarget, pSettings);
        }

        public static string ToJson(this ScriptableObject pTarget, Formatting pFormatting, JsonSerializerSettings pSettings = null)
        {
            return pSettings == null
                ? JsonConvert.SerializeObject(pTarget, pFormatting)
                : JsonConvert.SerializeObject(pTarget, pFormatting, pSettings);
        }
        
        public static string ToJson(this ScriptableObject pTarget, Type pType, JsonSerializerSettings pSettings)
        {
            return JsonConvert.SerializeObject(pTarget, pType, pSettings);
        }
        
        public static string ToJson(this ScriptableObject pTarget, Type pType, Formatting pFormatting, JsonSerializerSettings pSettings)
        {
            return JsonConvert.SerializeObject(pTarget, pType, pFormatting, pSettings);
        }

        public static string ToJson(this ScriptableObject pTarget, params JsonConverter[] pConverters)
        {
            return JsonConvert.SerializeObject(pTarget, pConverters);
        }
    }
}
