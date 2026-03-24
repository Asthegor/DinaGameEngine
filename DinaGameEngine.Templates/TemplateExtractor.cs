using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.Templates
{
    public class TemplateExtractor(ILogService logService) : ITemplateExtractor
    {
        private readonly ILogService _logService = logService;

        private const string _templateProjectPrefix = "DinaGameEngine.Templates.GameProject.";
        private const string _templateProjectMarkersFile = "DinaGameEngine.Templates.GameTemplateMarkers.json";
        private static readonly string[] _templateProjectSpecialFiles = ["Directory.Build.props", "DinaCSharp.deps.json"];
        private static readonly string[] _templateProjectBinaryFiles = ["Icon.ico", "Icon.bmp", "DinaCSharp.dll", "DLACrypto.dll"];

        public List<TemplateMarkerModel>? GetMarkers<T>(TemplateType type, T model)
        {
            _logService.Info("GetMarkers appelé");
            switch (type)
            {
                case TemplateType.GameProject:
                    if (model is NewProjectModel newProjectModel)
                        return GetTemplateProjectMarkers(newProjectModel);
                    _logService.Error($"Pour une Scene, le modèle doit être '{typeof(NewProjectModel).Name}'");
                    return null;
                case TemplateType.Scene:
                    if (model is object sceneModel)
                        return GetTemplateSceneMarkers(sceneModel);
                    _logService.Error($"Pour une Scene, le modèle doit être '{typeof(object).Name}'");
                    return null;
            }

            return null;
        }
        private List<TemplateMarkerModel>? GetTemplateProjectMarkers(NewProjectModel model)
        {
            var assembly = GetType().Assembly;
            var resourceStream = assembly.GetManifestResourceStream(_templateProjectMarkersFile);

            if (resourceStream == null)
            {
                _logService.Error($"Fichier '{_templateProjectMarkersFile}' inexistant.");
                return null;
            }
            _logService.Info("Flux ouvert avec succès");

            try
            {
                var jsonContent = new StreamReader(resourceStream).ReadToEnd();
                var listTemplateMarkerJsonEntry = JsonHelper.Deserialize<List<TemplateMarkerJsonEntry>>(jsonContent);

                if (listTemplateMarkerJsonEntry == null)
                {
                    _logService.Error($"Fichier '{_templateProjectMarkersFile}' vide.");
                    return null;
                }
                List<TemplateMarkerModel> listTemplateMarkerModel = [];
                foreach (var entry in listTemplateMarkerJsonEntry)
                {
                    var property = model.GetType().GetProperty(entry.Source);
                    string defaultValue = property?.GetValue(model)?.ToString() ?? string.Empty;

                    listTemplateMarkerModel.Add(new TemplateMarkerModel
                    {
                        Key = entry.Key,
                        Label = entry.Label,
                        DefaultValue = defaultValue,
                        Value = defaultValue,
                    });
                }
                return listTemplateMarkerModel;
            }
            catch (Exception e)
            {
                _logService.Error($"Fichier '{_templateProjectMarkersFile}' corrompu :{Environment.NewLine}{e.Message}");
            }
            return null;
        }
        private static List<TemplateMarkerModel>? GetTemplateSceneMarkers(object obj)
        {
            if (obj is TemplateMarkerModel model)
                return [model];
            return null;
        }
        public bool Extract(TemplateType type, string rootPath, List<TemplateMarkerModel> markers)
        {
            _logService.Info("Extract appelé");
            var dict = markers.ToDictionary(m => m.Key, m => m.Value);
            return type switch
            {
                TemplateType.GameProject => ExtractGameProject(rootPath, dict),
                TemplateType.Scene => ExtractScene(rootPath, dict),
                _ => throw new NotImplementedException()
            };
        }

        private bool ExtractGameProject(string rootPath, Dictionary<string, string> dict)
        {
            try
            {
                _logService.Info("Extraction du template GameProject");

                var assembly = GetType().Assembly;
                var listResources = assembly.GetManifestResourceNames().Where(name => name.StartsWith(_templateProjectPrefix, StringComparison.OrdinalIgnoreCase));
                _logService.Info($"Ressources trouvées : {listResources.Count()}", 1);

                foreach (var resourceName in listResources)
                {
                    _logService.Info($"Ressource en cours : {resourceName}", 1);
                    var resourceNameWithoutPrefix = resourceName.Replace(_templateProjectPrefix, "");

                    var fileInfoModel = GetFileInfo(resourceNameWithoutPrefix, rootPath, dict);
                    var stream = assembly.GetManifestResourceStream(resourceName);
                    if (stream == null)
                        continue;
                    if (_templateProjectBinaryFiles.Any(b => fileInfoModel.FileName.EndsWith(b)))
                    {
                        // Fichier binaire
                        Directory.CreateDirectory(Path.GetDirectoryName(fileInfoModel.FullName)!);
                        using var fileStream = File.Create(fileInfoModel.FullName);
                        stream.CopyTo(fileStream);
                    }
                    else
                    {
                        // Fichier texte
                        string content = new StreamReader(stream).ReadToEnd();
                        content = ReplaceMarkers(content, dict, out var contentCounts);

                        foreach (var count in contentCounts)
                            _logService.Info($"  {count.Key} : {count.Value} remplacement(s)", 2);

                        Directory.CreateDirectory(Path.GetDirectoryName(fileInfoModel.FullName)!);
                        File.WriteAllText(fileInfoModel.FullName, content);
                    }
                    _logService.Info($"Fichier écrit : {fileInfoModel.FileName}", 1);
                }

            }
            catch (Exception e)
            {
                _logService.Error(e.Message);
                if (Directory.Exists(rootPath))
                    Directory.Delete(rootPath, true);
                return false;
            }

            _logService.Info("Extraction du template GameProject terminée");
            return true;
        }
        private static bool ExtractScene(string rootPath, Dictionary<string, string> dict)
        {
            if (string.IsNullOrEmpty(rootPath) || dict.Count == 0)
                return false;
            return false;
        }
        private static bool IsSpecialFile(string path, out string specialFileName)
        {
            foreach (var specialFile in _templateProjectSpecialFiles)
            {
                if (path.EndsWith(specialFile, StringComparison.OrdinalIgnoreCase))
                {
                    specialFileName = specialFile;
                    return true;
                }
            }
            specialFileName = string.Empty;
            return false;
        }
        private static string ExtractFromRight(string input, string separator, out string extracted)
        {
            var index = input.LastIndexOf(separator);
            if (index < 0)
            {
                extracted = input;
                return string.Empty;
            }
            extracted = input[(index + separator.Length)..];
            return input[..index];
        }
        private static string BuildPathFromDot(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            return Path.Combine(path.Split('.'));
        }

        private static FileInfoModel GetFileInfo(string resourceName, string rootPath, Dictionary<string, string> dict)
        {
            string filename;
            string pathWithPoint;
            // Test des fichiers spéciaux
            if (IsSpecialFile(resourceName, out string specialFileName))
            {
                pathWithPoint = ExtractFromRight(resourceName, specialFileName, out _).TrimEnd('.');
                filename = specialFileName;
            }
            else
            {
                var pathWithFileNameWithoutExtension = ExtractFromRight(resourceName, ".", out var extension);
                pathWithPoint = ExtractFromRight(pathWithFileNameWithoutExtension, ".", out var filenameWithoutExtension).TrimEnd('.');
                filename = $"{filenameWithoutExtension}.{extension}";
            }
            var pathWithMarkers = pathWithPoint.StartsWith(".config") ? pathWithPoint : BuildPathFromDot(pathWithPoint);
            var path = ReplaceMarkers(pathWithMarkers, dict, out _);

            filename = ReplaceMarkers(filename, dict, out _);

            return new FileInfoModel(rootPath)
            {
                Directory = path,
                FileName = filename
            };
        }
        private static string ReplaceMarkers(string input, Dictionary<string, string> markers, out Dictionary<string, int> counts)
        {
            counts = [];
            foreach (var marker in markers)
            {
                counts[marker.Key] = CountOccurences(input, marker.Key);
                input = input.Replace(marker.Key, marker.Value, StringComparison.OrdinalIgnoreCase);
            }
            return input;
        }
        private static int CountOccurences(string input, string searched)
        {
            int searchedLength = searched.Length;
            int index = 0;
            int count = 0;
            while ((index = input.IndexOf(searched, index, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                index += searchedLength;
            }
            return count;
        }

        private static readonly string[] _libFiles = ["DinaCSharp.dll", "DinaCSharp.xml", "DinaCSharp.deps.json", "DLACrypto.dll"];
        public IReadOnlyList<string> LibFiles => _libFiles;
        public bool ExtractLibs(string outputPath)
        {
            var assembly = GetType().Assembly;
            foreach (var filename in _libFiles)
            {
                var resourceName = $"{_templateProjectPrefix}{filename}";
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    _logService.Error($"Fichier '{filename}' manquant dans le template");
                    return false;
                }
                var fullname = Path.Combine(outputPath, filename);
                using var fileStream = File.Create(fullname);
                stream.CopyTo(fileStream);
                _logService.Info($" Fichier '{filename}' copié");
            }
            return true;
        }


        private class TemplateMarkerJsonEntry
        {
            public string Key { get; set; } = string.Empty;
            public string Source { get; set; } = string.Empty;
            public string Label { get; set; } = string.Empty;
        }
    }
}
