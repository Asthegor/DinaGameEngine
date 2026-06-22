namespace DinaGameEngine.CodeGeneration
{
    public class SectionParser(string fileContent)
    {
        const string ZONE_OPEN = "// =[ZONE:{0}]=";
        const string ZONE_CLOSE = "// =[/ZONE:{0}]=";

        private readonly List<string> _lines = [.. fileContent.Split(["\r\n", "\n"], StringSplitOptions.None)];

        public void InsertIntoZone(string zoneName, IEnumerable<string> lines, bool checkExistingLines = false)
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
            InsertIntoZone("USINGS", [$"using {namespaceName};"]);
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
                if (_lines[i].Contains($" {fieldName}"))
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
        public bool IsPartialFunctionExisting(string functionSignature)
        {
            var indexStartZone = FindIndexZone(ZONE_OPEN, "PARTIAL_METHODS");
            var indexEndZone = FindIndexZone(ZONE_CLOSE, "PARTIAL_METHODS");
            var partialMethodLines = _lines.GetRange(indexStartZone, indexEndZone - indexStartZone);
            return partialMethodLines.Any(l => l.Contains(functionSignature));
        }
        public void RemovePartialFunction(string functionSignature)
        {
            var indexStartZone = FindIndexZone(ZONE_OPEN, "PARTIAL_METHODS");
            var indexEndZone = FindIndexZone(ZONE_CLOSE, "PARTIAL_METHODS");
            var isFunctionPresent = false;
            var indexStartFunction = -1;
            for (int index = indexStartZone; index < indexEndZone; index++)
            {
                if (_lines[index].Contains(functionSignature))
                {
                    isFunctionPresent = true;
                    indexStartFunction = index;
                    break;
                }
            }
            if (!isFunctionPresent)
                return;

            var indexEndFunction = -1;
            var brackets = -1;
            for(int index = indexStartFunction; index < indexEndZone; index++)
            {
                if (_lines[index].Contains('{'))
                {
                    if (brackets < 0)
                        brackets = 0;
                    brackets++;
                }
                if (_lines[index].Contains('}'))
                    brackets--;

                if (brackets == 0)
                {
                    indexEndFunction = index;
                    break;
                }
            }

            _lines.RemoveRange(indexStartFunction, indexEndFunction - indexStartFunction + 1);
        }
        private bool TryFindPartialFunctionBodyBounds(string functionSignature, out int indexBodyStart, out int indexReturn)
        {
            indexBodyStart = -1;
            indexReturn = -1;

            var indexStartZone = FindIndexZone(ZONE_OPEN, "PARTIAL_METHODS");
            var indexEndZone = FindIndexZone(ZONE_CLOSE, "PARTIAL_METHODS");

            var indexFunction = -1;
            for (int i = indexStartZone; i < indexEndZone; i++)
            {
                if (_lines[i].Contains(functionSignature))
                {
                    indexFunction = i;
                    break;
                }
            }
            if (indexFunction < 0)
                return false;

            // Trouver l'accolade ouvrante
            var indexOpen = -1;
            for (int i = indexFunction; i < indexEndZone; i++)
            {
                if (_lines[i].Contains('{'))
                {
                    indexOpen = i;
                    break;
                }
            }
            if (indexOpen < 0)
                return false;

            // Trouver la ligne return
            for (int i = indexOpen + 1; i < indexEndZone; i++)
            {
                if (_lines[i].Contains("return"))
                {
                    indexBodyStart = indexOpen + 1;
                    indexReturn = i;
                    return true;
                }
            }

            return false;
        }

        public void WriteInDelimitedPartialFunction(string functionSignature, IEnumerable<string> lines)
        {
            if (!TryFindPartialFunctionBodyBounds(functionSignature, out _, out var indexReturn))
                return;

            _lines.InsertRange(indexReturn, lines);
        }

        public bool IsPartialFunctionBodyEqual(string functionSignature, string action, string marker)
        {
            if (!TryFindPartialFunctionBodyBounds(functionSignature, out var indexBodyStart, out var indexReturn))
                return false;

            var currentBodyLines = _lines.GetRange(indexBodyStart, indexReturn - indexBodyStart);

            var (histOpen, histClose) = FindLocalZone(indexBodyStart, indexReturn, marker);
            if (histOpen >= 0 && histClose >= 0)
            {
                currentBodyLines.RemoveRange(histOpen - indexBodyStart, histClose - histOpen + 1);
            }

            var currentBody = NormalizeBody(currentBodyLines);

            var actionLines = action.Replace("\r\n", "\n").Split('\n');
            var normalizedAction = NormalizeBody(actionLines);

            return string.Equals(currentBody, normalizedAction, StringComparison.Ordinal);
        }

        private static string NormalizeBody(IEnumerable<string> lines)
        {
            var trimmedLines = lines.Select(l => l.Trim()).ToList();

            var start = 0;
            while (start < trimmedLines.Count && trimmedLines[start].Length == 0)
                start++;

            var end = trimmedLines.Count - 1;
            while (end >= start && trimmedLines[end].Length == 0)
                end--;

            if (start > end)
                return string.Empty;

            return string.Join("\n", trimmedLines.GetRange(start, end - start + 1));
        }
        private (int indexOpen, int indexClose) FindLocalZone(int searchStart, int searchEnd, string zoneName)
        {
            var openTemplate = string.Format(ZONE_OPEN, zoneName);
            var closeTemplate = string.Format(ZONE_CLOSE, zoneName);

            var indexOpen = -1;
            var indexClose = -1;
            for (int i = searchStart; i < searchEnd; i++)
            {
                if (indexOpen < 0 && _lines[i].TrimStart() == openTemplate)
                {
                    indexOpen = i;
                }
                else if (indexOpen >= 0 && _lines[i].TrimStart() == closeTemplate)
                {
                    indexClose = i;
                    break;
                }
            }
            return (indexOpen, indexClose);
        }

        public bool CommentAndReplacePartialFunctionBody(string functionSignature, IEnumerable<string> newLines, string marker)
        {
            if (!TryFindPartialFunctionBodyBounds(functionSignature, out var indexBodyStart, out var indexReturn))
                return false;

            var (indexHistOpen, indexHistClose) = FindLocalZone(indexBodyStart, indexReturn, marker);
            if (indexHistOpen >= 0 && indexHistClose >= 0)
            {
                _lines.RemoveRange(indexHistOpen, indexHistClose - indexHistOpen + 1);

                if (!TryFindPartialFunctionBodyBounds(functionSignature, out indexBodyStart, out indexReturn))
                    return false;
            }

            var hasContentToComment = _lines.Skip(indexBodyStart).Take(indexReturn - indexBodyStart).Any(l => !string.IsNullOrWhiteSpace(l));
            if (hasContentToComment)
            {
                var indentation = _lines[indexReturn][..(_lines[indexReturn].Length - _lines[indexReturn].TrimStart().Length)];

                _lines.Insert(indexBodyStart, $"{indentation}{string.Format(ZONE_OPEN, marker)}");
                indexReturn++;

                for (int i = indexBodyStart + 1; i < indexReturn; i++)
                    _lines[i] = $"{indentation}// {_lines[i].TrimStart()}";

                _lines.Insert(indexReturn, $"{indentation}{string.Format(ZONE_CLOSE, marker)}");
                _lines.Insert(indexReturn + 1, string.Empty);
            }

            WriteInDelimitedPartialFunction(functionSignature, newLines);
            return true;
        }
        public void UpdateStartupScene(string sceneKey)
        {
            var index = 0;
            var startupLine = string.Empty;
            for(index = 0; index < _lines.Count;index++)
            {
                if (_lines[index].Contains("_sceneManager.SetCurrentScene("))
                {
                    startupLine = _lines[index];
                    break;
                }
            }
            if (string.IsNullOrEmpty(startupLine))
            {
                // La ligne n'existe pas, on doit la créer
                var indexOnSetStartupScene = _lines.FindIndex(line => line.Contains("partial void OnSetStartupScene()"));
                if (indexOnSetStartupScene < 0)
                    return;

                if (!_lines[indexOnSetStartupScene].EndsWith('}'))
                {
                    if (!_lines[indexOnSetStartupScene].EndsWith('{'))
                        indexOnSetStartupScene++;
                    _lines.Insert(indexOnSetStartupScene + 1, CodeBuilder.AddLine($"_sceneManager.SetCurrentScene(SceneKeys.{sceneKey});", 3));
                }
                else
                {
                    // On supprime '}'
                    _lines[indexOnSetStartupScene] = _lines[indexOnSetStartupScene][..^1];
                    // On rajoute d'abord le '}' puis l'appel de SetCurrentScene
                    // Cela permet de les avoir dans l'ordre dans _lines car Insert insère à la position exacte.
                    _lines.Insert(indexOnSetStartupScene + 1, CodeBuilder.CloseBlock(2));
                    _lines.Insert(indexOnSetStartupScene + 1, CodeBuilder.AddLine($"_sceneManager.SetCurrentScene(SceneKeys.{sceneKey});", 3));
                }
            }
            else
            {
                var indexStartCurrentStartupScene = startupLine.IndexOf('(') + 1;
                var indexEndCurrentStartupScene = startupLine.IndexOf(')');
                var currentStartupScene = startupLine[indexStartCurrentStartupScene..indexEndCurrentStartupScene];

                _lines[index] = startupLine.Replace(currentStartupScene, $"SceneKeys.{sceneKey}");
            }
        }
        public void RemoveStartupScene()
        {
            var startupLine = string.Empty;
            int index;
            for (index = 0; index < _lines.Count; index++)
            {
                if (_lines[index].Contains("_sceneManager.SetCurrentScene("))
                {
                    startupLine = _lines[index];
                    break;
                }
            }
            if (string.IsNullOrEmpty(startupLine))
                return;

            _lines.RemoveAt(index);
        }

    }
}
