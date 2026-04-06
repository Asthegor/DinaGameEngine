using DinaGameEngine.Models.Project;

using System.Drawing;
using System.Text.Json;

namespace DinaGameEngine.Models.Helpers
{
    public class ComponentPropertyHelper
    {
        public static string GetStringProperty(ComponentModel component, string key)
        {
            if (!component.Properties.TryGetValue(key, out var value))
                return string.Empty;
            return value is JsonElement je
                   ? je.GetString() ?? string.Empty
                   : value?.ToString() ?? string.Empty;
        }
        public static (int? X, int? Y) GetPointProperty(ComponentModel component, string key)
        {
            if (!component.Properties.TryGetValue(key, out var value))
                return (null, null);
            if (value is Point pt)
                return (pt.X, pt.Y);
            if (value is JsonElement je)
                return (je.GetProperty("X").GetInt32(), je.GetProperty("Y").GetInt32());
            return (null, null);
        }
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
            => GetIntProperty(source, key, (int?)defaultValue) ?? defaultValue;
        public static int? GetIntProperty(ComponentModel source, string key, int? defaultValue = null)
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
