using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

using System.Text;

namespace DinaGameEngine.CodeGeneration
{
    public partial class CodeGenerator : ICodeGenerator
    {
        private string[] _resolutionPath = { "_720p", "_900p", "_1080p", "_1440p", "_2160p" };

        private readonly IFileService _fileService;
        private readonly ILogService _logService;
        private readonly IGeneratedFileChecker _generatedFileChecker;
        public CodeGenerator(IFileService fileService, ILogService logService, IGeneratedFileChecker generatedFileChecker)
        {
            _fileService = fileService;
            _logService = logService;
            _generatedFileChecker = generatedFileChecker;
        }

        public void GenerateAllFiles(GameProjectModel gameProjectModel)
        {
            GenerateGameProjectDesigner(gameProjectModel);
            GenerateGameProjectUserFile(gameProjectModel);

            foreach (var scene in gameProjectModel.Scenes)
                GenerateNewScene(gameProjectModel, scene);

            #region Génération des fichiers de clés
            GeneratePaletteColors(gameProjectModel);
            GenerateFontKeys(gameProjectModel);
            #endregion
        }
        public void GenerateAllDesigners(GameProjectModel gameProjectModel)
        {
            GenerateGameProjectDesigner(gameProjectModel);

            foreach (var scene in gameProjectModel.Scenes)
            {
                GenerateSceneDesigner(gameProjectModel, scene);
                UpdateGameProjectDesignerScenes(gameProjectModel, scene);
            }

            GeneratePaletteColorDesigner(gameProjectModel);
            foreach (var color in gameProjectModel.Colors)
                AddColor(gameProjectModel, color);

            GenerateFontKeysDesigner(gameProjectModel);
            foreach (var font in gameProjectModel.Fonts)
                AddFont(gameProjectModel, font);

        }

        private SectionParser CreateSectionParserFor(string fileFullName)
        {
            var fileContent = _fileService.ReadAllText(fileFullName);
            if (string.IsNullOrEmpty(fileContent))
            {
                _logService.Error($"Fichier '{fileFullName}' vide ou corrompu.");
                throw new Exception($"Fichier '{fileFullName}' vide ou corrompu.");
            }
            return new SectionParser(fileContent);
        }

    }
}
