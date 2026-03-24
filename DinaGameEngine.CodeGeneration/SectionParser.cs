namespace DinaGameEngine.CodeGeneration
{
    public class SectionParser
    {
        const string ZONE_OPEN = "// =[ZONE:{0}]=";
        const string ZONE_CLOSE = "// =[/ZONE:{0}]=";
        const string SECTION_OPEN = "// =[COMPONENT:{0}:{1}:{2}:{3}]=";
        const string SECTION_CLOSE = "// =[/COMPONENT:{0}:{1}:{2}]=";
        const string AVAILABLE_FIELDS_OPEN = "// =[AVAILABLE_FIELDS]=";
        const string AVAILABLE_FIELDS_CLOSE = "// =[/AVAILABLE_FIELDS]=";


        private List<string> _lines = [];

        public SectionParser(string fileContent)
        {
            _lines = [.. fileContent.Split(["\r\n", "\n"], StringSplitOptions.None)];
        }

        public void InsertBeforeZone(string zoneName, IEnumerable<string> lines)
        {
            var index = FindIndexZone(ZONE_CLOSE, zoneName);
            if (index < 0)
                return;
            _lines.InsertRange(index, lines);
        }
        public void AddUsingIfMissing(string namespaceName)
        {
            var indexOpen = FindIndexZone(ZONE_OPEN, "USINGS");
            var indexClose = FindIndexZone(ZONE_CLOSE, "USINGS");
            for (int i = indexOpen; i < indexClose; i++)
            {
                if (_lines[i].Contains(namespaceName))
                    return;
            }
            InsertBeforeZone("USINGS", [$"using {namespaceName};"]);
        }
        public void RemoveFromZone(string zoneName, string identifier) => RemoveFromZone(zoneName, line => line.Contains(identifier));
        public void RemoveFromZone(string zoneName, Func<string, bool> predicate)
        {
            var indexOpen = FindIndexZone(ZONE_OPEN, zoneName);
            var indexClose = FindIndexZone(ZONE_CLOSE, zoneName);
            for (int index = indexClose - 1; index > indexOpen; index--)
            {
                if (predicate(_lines[index]))
                {
                    _lines.RemoveAt(index);
                    return;
                }
            }
        }
        public string GetContent() => string.Join(Environment.NewLine, _lines);

        private int FindIndexZone(string template, string zoneName) => _lines.FindIndex(l => l.TrimStart() == string.Format(template, zoneName));
    }
}
