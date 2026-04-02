using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.CodeGeneration
{
    public partial class CodeGenerator(IFileService fileService, ILogService logService, IGeneratedFileChecker generatedFileChecker,
                         IComponentGeneratorRegistry componentGeneratorRegistry, IDialogService dialogService) : ICodeGenerator
    {
        private readonly string[] _resolutionPath = ["_720p", "_900p", "_1080p", "_1440p", "_2160p"];

        private readonly IFileService _fileService = fileService;
        private readonly ILogService _logService = logService;
        private readonly IGeneratedFileChecker _generatedFileChecker = generatedFileChecker;
        private readonly IComponentGeneratorRegistry _componentGeneratorRegistry = componentGeneratorRegistry;
        private readonly IDialogService _dialogService = dialogService;

        public void GenerateAllFiles(GameProjectModel gameProjectModel)
        {
            GenerateGameProjectDesigner(gameProjectModel);
            GenerateGameProjectUserFile(gameProjectModel);

            #region Génération des fichiers de clés
            GenerateSceneKeys(gameProjectModel);
            GeneratePaletteColors(gameProjectModel);
            GenerateFontKeys(gameProjectModel);
            #endregion

            foreach (var scene in gameProjectModel.Scenes)
                GenerateNewScene(gameProjectModel, scene);
        }
        public void GenerateAllDesigners(GameProjectModel gameProjectModel)
        {
            GenerateGameProjectDesigner(gameProjectModel);

            foreach (var scene in gameProjectModel.Scenes)
            {
                GenerateSceneDesigner(gameProjectModel, scene);
                UpdateSceneKeys(gameProjectModel, scene);
                UpdateGameProjectDesignerScenes(gameProjectModel, scene);

                foreach (var component in scene.Components)
                    AddComponent(gameProjectModel, scene, component);
            }

            GeneratePaletteColorDesigner(gameProjectModel);
            foreach (var color in gameProjectModel.Colors)
                AddColor(gameProjectModel, color);

            GenerateFontKeysDesigner(gameProjectModel);
            foreach (var font in gameProjectModel.Fonts)
                AddFontKey(gameProjectModel, font);
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

        public void AddComponent(GameProjectModel gameProjectModel, SceneModel sceneModel, ComponentModel component)
        {
            var generator = _componentGeneratorRegistry.GetGenerator(component.Type);
            if (generator == null)
            {
                _logService.Warning($"Générateur du componsant '{component.Type}' non trouvé.");
                return;
            }
            var designerFilePath = _fileService.Combine(gameProjectModel.RootPath, "Scenes", $"{sceneModel.Class}.Designer.cs");
            var sectionParser = CreateSectionParserFor(designerFilePath);
            generator.Add(sectionParser, component);
            _fileService.WriteAllText(designerFilePath, sectionParser.GetContent());

            var userFilePath = _fileService.Combine(gameProjectModel.RootPath, "Scenes", $"{sceneModel.Class}.cs");
            sectionParser = CreateSectionParserFor(userFilePath);
            generator.GenerateUserFileCommentField(sectionParser, component);
            _fileService.WriteAllText(userFilePath, sectionParser.GetContent());
        }

        public void RemoveComponent(GameProjectModel gameProjectModel, SceneModel sceneModel, ComponentModel component, bool showWarning = true)
        {
            var generator = _componentGeneratorRegistry.GetGenerator(component.Type);
            if (generator == null)
            {
                _logService.Warning($"Générateur du composant '{component.Type}' non trouvé.");
                return;
            }
            var designerFilePath = _fileService.Combine(gameProjectModel.RootPath, "Scenes", $"{sceneModel.Class}.Designer.cs");
            var sectionParserDesigner = CreateSectionParserFor(designerFilePath);
            // Suppression dans le Designer
            var removedFields = generator.Remove(sectionParserDesigner, component);
            _fileService.WriteAllText(designerFilePath, sectionParserDesigner.GetContent());

            var userFilePath = _fileService.Combine(gameProjectModel.RootPath, "Scenes", $"{sceneModel.Class}.cs");
            var sectionParserUser = CreateSectionParserFor(userFilePath);

            generator.RemoveUserFileCommentField(sectionParserUser, component);
            _fileService.WriteAllText(userFilePath, sectionParserUser.GetContent());

            var stillUsed = removedFields
                .Where(fieldName => sectionParserUser.ContainsOutsideZone("AVAILABLE_FIELDS", fieldName))
                .ToList();

            if (showWarning && stillUsed.Count != 0)
            {
                _dialogService.ShowWarning(
                    LocalizationManager.GetTranslation("Component_FieldStillUsed_Title"),
                    LocalizationManager.GetTranslation("Component_FieldStillUsed_Message",
                                                       string.Join(", ", stillUsed)));
            }
        }
    }
}
