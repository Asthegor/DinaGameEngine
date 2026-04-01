namespace DinaGameEngine.CodeGeneration
{
    public class SectionParser
    {
        const string ZONE_OPEN = "// =[ZONE:{0}]=";
        const string ZONE_CLOSE = "// =[/ZONE:{0}]=";

        private List<string> _lines = [];

        public SectionParser(string fileContent)
        {
            _lines = [.. fileContent.Split(["\r\n", "\n"], StringSplitOptions.None)];
        }

        public void InsertBeforeZone(string zoneName, IEnumerable<string> lines, bool checkExistingLines = false)
        {
            var indexZoneClose = FindIndexZone(ZONE_CLOSE, zoneName);
            if (indexZoneClose < 0)
                return;
            if (checkExistingLines)
            {
                var indexZoneOpen = FindIndexZone(ZONE_OPEN, zoneName);
                var zoneLines = _lines.GetRange(indexZoneOpen, indexZoneClose - indexZoneOpen);
                foreach (var line in lines)
                {
                    if (zoneLines.Find(c => c.Contains(line)) == null)
                        _lines.Insert(indexZoneClose, line);
                }
            }
            else
            {
                _lines.InsertRange(indexZoneClose, lines);
            }
        }
        public void AddUsingIfMissing(string namespaceName)
        {
            var indexOpen = FindIndexZone(ZONE_OPEN, "USINGS");
            var indexClose = FindIndexZone(ZONE_CLOSE, "USINGS");
            for (int i = indexOpen; i < indexClose; i++)
            {
                if (_lines[i].Contains($"{namespaceName};"))
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
                    _lines.RemoveAt(index);
            }
        }
        public string GetContent() => string.Join(Environment.NewLine, _lines);
        public void InsertBeforeLine(string searchText, IEnumerable<string> lines)
        {
            var index = _lines.FindIndex(l => l.Contains(searchText));
            if (index < 0)
                return;
            _lines.InsertRange(index, lines);
        }
        public void RemoveLinesInRange(string startSearchText, string endSearchText, Func<string, bool> predicate)
        {
            var startIndex = _lines.FindIndex(l => l == startSearchText);
            if (startIndex < 0)
                return;
            var endIndex = _lines.FindIndex(startIndex, l => l == endSearchText);
            if (endIndex < 0)
                return;
            for (int i = endIndex; i >= startIndex; i--)
            {
                if (predicate(_lines[i]))
                    _lines.RemoveAt(i);
            }
        }
        public void RemoveField(string fieldName)
        {
            var indexOpen = FindIndexZone(ZONE_OPEN, "FIELDS");
            var indexClose = FindIndexZone(ZONE_CLOSE, "FIELDS");
            var isFieldPresent = false;
            var indexField = -1;
            for (int i = indexOpen; i < indexClose; i++)
            {
                if (_lines[i].Contains($" {fieldName} = "))
                {
                    isFieldPresent = true;
                    indexField = i;
                    break;
                }
            }
            if (!isFieldPresent)
                return;

            var endOfLines = _lines.GetRange(indexClose, _lines.Count - indexClose);
            if (!endOfLines.Any(line => line.Contains(fieldName)))
                _lines.RemoveAt(indexField);
        }
        private int FindIndexZone(string template, string zoneName) => _lines.FindIndex(l => l.TrimStart() == string.Format(template, zoneName));
        public bool ContainsOutsideZone(string zoneName, string searchText)
        {
            var indexOpen = FindIndexZone(ZONE_OPEN, zoneName);
            var indexClose = FindIndexZone(ZONE_CLOSE, zoneName);
            for (int i = 0; i < _lines.Count; i++)
            {
                if (i >= indexOpen && i <= indexClose)
                    continue;
                if (_lines[i].Contains(searchText))
                    return true;
            }
            return false;
        }
    }
}
