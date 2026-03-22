using DinaGameEngine.Abstractions;
using DinaGameEngine.Models;

using System;
using System.Collections.Generic;
using System.Text;

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
            var index = FindIndexZone(ZONE_OPEN, zoneName);
            _lines.InsertRange(index, lines);
        }
        public void InsertField(string fieldDeclaration)
        {

        }
        public void AddSection(string type, string key, string step, string code)
        {

        }
        public void RemoveSection(string type, string key, string step)
        {

        }
        public void RemoveAllSectionForComponent(string type, string key)
        {

        }
        public void UpdateAvailableFields(string fieldComment)
        {

        }
        public void AddUsingIfMissing(string namespaceName)
        {
            var index = FindIndexZone(ZONE_OPEN, "USINGS");
            for (int i = index; i >= 0; i--)
            {
                if (_lines[i].Contains(namespaceName))
                    return;
            }
            InsertBeforeZone("USINGS", [$"using {namespaceName};"]);
        }
        public void AddPartialDeclaration(string signature)
        {

        }
        public void ValidateChecksums()
        {

        }
        public string GetContent() => string.Join(Environment.NewLine, _lines);

        private void FindClassClosingBrace()
        {

        }
        private void FindZone(string zoneName)
        {

        }
        private void ComputeChecksum(string input)
        {

        }

        private int FindIndexZone(string template, string zoneName) => _lines.FindIndex(l => l.TrimStart() == string.Format(template, zoneName));
    }
}
