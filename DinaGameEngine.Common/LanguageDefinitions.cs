namespace DinaGameEngine.Common
{
    public record CharacterRegionDefinition(int Start, int End);
    public record LanguageDefinition(string Code, string Name, int Index);
    public record LanguageRegionDefinition(int Index, int[] RegionIndices);

    public static class LanguageDefinitions
    {
        public static readonly CharacterRegionDefinition[] CharacterRegions =
        [
            new(32,    126),
            new(160,   255),
            new(256,   383),
            new(1024,  1279),
            new(12352, 12447),
            new(12448, 12543),
            new(19968, 40959),
            new(44032, 55203),
            new(1536,  1791)
        ];

        public static readonly LanguageDefinition[] Languages =
        [
            new("en", "English",    0),
            new("fr", "Français",   1),
            new("es", "Español",    1),
            new("de", "Deutsch",    1),
            new("it", "Italiano",   1),
            new("pt", "Português",  1),
            new("nl", "Nederlands", 1),
            new("ru", "Русский",    2),
            new("pl", "Polski",     3),
            new("tr", "Türkçe",     3),
            new("ja", "日本語",     4),
            new("zh", "中文",       5),
            new("ko", "한국어",     6),
            new("ar", "العربية",    7)
        ];

        public static readonly LanguageRegionDefinition[] LanguageRegions =
        [
            new(0, [0]),
            new(1, [0, 1]),
            new(2, [0, 3]),
            new(3, [0, 2]),
            new(4, [0, 4, 5, 6]),
            new(5, [0, 6]),
            new(6, [0, 7]),
            new(7, [0, 8])
        ];

        public static HashSet<int> GetActiveRegionIndices(HashSet<string> languages)
        {
            return [.. languages.Select(code => Languages.FirstOrDefault(l => l.Code == code))
                                .Where(l => l != null)
                                .SelectMany(l => LanguageRegions[l!.Index].RegionIndices)];
        }

        public static int[] GetNewRegionIndices(HashSet<string> currentLanguages, string newLanguageCode)
        {
            var newLang = Languages.FirstOrDefault(l => l.Code == newLanguageCode);
            if (newLang == null)
                return [];

            var activeRegions = GetActiveRegionIndices(currentLanguages);
            return [.. LanguageRegions[newLang.Index].RegionIndices.Where(r => !activeRegions.Contains(r))];
        }
    }
}