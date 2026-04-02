using DinaGameEngine.Models.Project;

using System.Text.Json;

namespace DinaGameEngine.Utils
{
    public class ComponentPropertyConverter
    {
        public static string GetReturnValueFrom<T>(T value)
        {
            if (value is int i)
                return i.ToString();
            if (value is float f)
                return f.ToString();
            if (value is double d)
                return d.ToString();

            if (value is bool b)
                return b.ToString().ToLower();

            if (typeof(T).IsEnum)
                return $"{value}";

            return value?.ToString() ?? string.Empty;
        }
        public static int GetIntProperty(ComponentModel source, string key, int defaultValue)
        {
            if (!source.Properties.TryGetValue(key, out var value))
                return defaultValue;
            if (value is JsonElement je)
            {
                if (je.ValueKind == JsonValueKind.Number)
                    return je.GetInt32();
                if (je.ValueKind == JsonValueKind.String)
                    return int.Parse(je.GetString()!);
            }
            return int.Parse(value.ToString()!);
        }

        public static bool GetBoolProperty(ComponentModel source, string key, bool defaultValue)
        {
            if (!source.Properties.TryGetValue(key, out var value))
                return defaultValue;
            if (value is JsonElement je)
            {
                if (je.ValueKind == JsonValueKind.True)
                    return true;
                if (je.ValueKind == JsonValueKind.False)
                    return false;
                if (je.ValueKind == JsonValueKind.String)
                    return bool.Parse(je.GetString()!);
            }
            return bool.Parse(value.ToString()!);
        }

        public static float GetFloatProperty(ComponentModel source, string key, float defaultValue)
        {
            if (!source.Properties.TryGetValue(key, out var value))
                return defaultValue;
            if (value is JsonElement je)
            {
                if (je.ValueKind == JsonValueKind.Number)
                    return je.GetSingle();
                if (je.ValueKind == JsonValueKind.String)
                    return float.Parse(je.GetString()!);
            }
            return float.Parse(value.ToString()!);
        }

        public static T GetEnumProperty<T>(ComponentModel source, string key, T defaultValue) where T : struct, Enum
        {
            if (!source.Properties.TryGetValue(key, out var value))
                return defaultValue;
            if (value is JsonElement je)
            {
                if (je.ValueKind == JsonValueKind.Number)
                    return (T)(object)je.GetInt32();
                if (je.ValueKind == JsonValueKind.String)
                    return Enum.Parse<T>(je.GetString()!);
            }
            return Enum.Parse<T>(value.ToString()!);
        }

    }
}
