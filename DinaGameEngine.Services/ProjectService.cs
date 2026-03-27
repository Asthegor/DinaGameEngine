using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;
using DinaGameEngine.Models.Startup;

using System.Diagnostics;

namespace DinaGameEngine.Services
{
    public class ProjectService(IFileService fileService, ILogService logService, ITemplateExtractor templateExtractor, ICodeGenerator codeGenerator) : IProjectService
    {
        private readonly IFileService _fileService = fileService;
        private readonly ILogService _logService = logService;
        private readonly ITemplateExtractor _templateExtractor = templateExtractor;
        private readonly ICodeGenerator _codeGenerator = codeGenerator;

        public GameProjectModel? OpenProject(string projectPath)
        {
            var filename = _fileService.Combine(projectPath, ProjectStructure.ProjectFileName);
            if (!_fileService.FileExists(filename))
            {
                _logService.Warning($"Fichier '{ProjectStructure.ProjectFileName}' inexistant dans le répertoire '{projectPath}'.");
                return null;
            }
            GameProjectModel? gameProjectModel;
            try
            {
                var jsonRaw = _fileService.ReadAllText(filename);
                gameProjectModel = JsonHelper.Deserialize<GameProjectModel>(jsonRaw);
                if (gameProjectModel == null)
                {
                    _logService.Error($"Le fichier '{filename}' est corrompu.");
                    return null;
                }
            }
            catch (Exception e)
            {
                _logService.Error($"Erreur lors de désérialisation du fichier '{filename}':{Environment.NewLine}{e.Message}");
                return null;
            }

            gameProjectModel.LastOpenedAt = DateTime.Now;

            UpdateJsonProjectFile(gameProjectModel);

            UpdateRecentProjects(gameProjectModel);

            return gameProjectModel;
        }

        public async Task<GameProjectModel?> CreateProject(NewProjectModel newProjectModel, List<TemplateMarkerModel> markers)
        {
            var projectFolder = _fileService.Combine(newProjectModel.ParentFolderPath, newProjectModel.Name);
            if (_fileService.DirectoryExists(projectFolder))
            {
                _logService.Warning($"Le répertoire '{projectFolder}' existe déjà.");
                return null;
            }

            _fileService.CreateDirectory(projectFolder);

            try
            {
                var result = await Task.Run(() => _templateExtractor.Extract(TemplateType.GameProject, projectFolder, markers));
                if (!result)
                    return null;

            }
            catch (Exception e)
            {
                _logService.Error(e.Message);
                Directory.Delete(projectFolder, true);
                _logService.Info($"Répertoire '{projectFolder}' supprimé");
                return null;
            }

            var rootNamespaceMarker = markers.First(m => m.Key == "__RootNamespace__");
            var solutionNameMarker = markers.First(m => m.Key == "__SolutionName__");
            var gameProjectNameMarker = markers.First(m => m.Key == "__GameProjectName__");
            var gameProjectModel = new GameProjectModel
            {
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.Now,
                DinaVersion = GetDinaVersion(),
                LastOpenedAt = DateTime.Now,
                SolutionName = solutionNameMarker.Value,
                ProjectName = gameProjectNameMarker.Value,
                RootPath = projectFolder,
                RootNamespace = rootNamespaceMarker.Value
            };
            gameProjectModel.Scenes.Add(new SceneModel { Name = "Game", Class = "GameScene", Key = "GameScene" });

            _codeGenerator.GenerateAllFiles(gameProjectModel);

            UpdateJsonProjectFile(gameProjectModel);

            UpdateRecentProjects(gameProjectModel);

            _logService.Info($"Projet '{gameProjectModel.SolutionName}' finalisé avec succès.");
            return gameProjectModel;
        }

        // Fonction permettant de récupérer la version de la DLL de DinaCSharp installée.
        private string GetDinaVersion()
        {
            var libsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libs", "DinaCSharp.dll");
            if (!File.Exists(libsPath))
            {
                _logService.Warning("DinaCSharp.dll introuvable dans Libs\\.");
                return string.Empty;
            }
            return FileVersionInfo.GetVersionInfo(libsPath).FileVersion ?? string.Empty;
        }

        private void UpdateRecentProjects(GameProjectModel gameProjectModel)
        {
            var appDataDirectory = _fileService.GetAppDataDirectory();
            if (!_fileService.DirectoryExists(appDataDirectory))
                _fileService.CreateDirectory(appDataDirectory);

            var recentProjectsFileName = _fileService.Combine(appDataDirectory, ProjectStructure.RecentProjectsFileName);

            List<RecentProjectModel> recentProjectsList = [];
            if (_fileService.FileExists(recentProjectsFileName))
            {
                var jsonRaw = _fileService.ReadAllText(recentProjectsFileName);
                var datas = JsonHelper.Deserialize<List<RecentProjectModel>>(jsonRaw);
                if (datas != null)
                {
                    recentProjectsList.AddRange(datas);
                    // Recherche et suppression du projet dans la liste
                    var oldRecentProjectModel = recentProjectsList.Find(p => p.Name == gameProjectModel.SolutionName && p.SolutionFolderPath == gameProjectModel.RootPath);
                    if (oldRecentProjectModel != null)
                        recentProjectsList.Remove(oldRecentProjectModel);
                }
                else
                {
                    _logService.Warning($"Le fichier '{recentProjectsFileName}' est corrompu ou vide.");
                }
            }
            var recentProjectModel = CreateRecentProjectModel(gameProjectModel);
            recentProjectsList.Insert(0, recentProjectModel);

            // Limitation de la liste à 20 entrées
            if (recentProjectsList.Count > 20)
                recentProjectsList.RemoveRange(20, recentProjectsList.Count - 20);

            // Mise à jour de la liste des projets récents
            var jsonSerialized = JsonHelper.Serialize(recentProjectsList);
            _fileService.WriteAllText(recentProjectsFileName, jsonSerialized);

            _logService.Info($"Mise à jour des projets récents effectuée.");
        }

        private RecentProjectModel CreateRecentProjectModel(GameProjectModel gameProjectModel)
        {
            return new RecentProjectModel
            {
                LastOpenedAt = DateTime.Now,
                Name = gameProjectModel.SolutionName,
                SolutionFolderPath = gameProjectModel.RootPath,
                ProjectFolderPath = _fileService.Combine(gameProjectModel.RootPath, gameProjectModel.ProjectName),
                IconPath = _fileService.Combine(gameProjectModel.RootPath, gameProjectModel.ProjectName, ProjectStructure.IconFileName)
            };
        }

        public void UpdateJsonProjectFile(GameProjectModel gameProjectModel)
        {
            var jsonSerialize = JsonHelper.Serialize(gameProjectModel);
            var projectFileName = _fileService.Combine(gameProjectModel.RootPath, ProjectStructure.ProjectFileName);
            _fileService.WriteAllText(projectFileName, jsonSerialize);
            _logService.Info($"Mise à jour du fichier '{ProjectStructure.ProjectFileName}' effectuée.");
        }

        public void RemoveSceneFromProject(GameProjectModel gameProjectModel, SceneModel sceneModel)
        {
            gameProjectModel.Scenes.Remove(sceneModel);
            UpdateJsonProjectFile(gameProjectModel);
        }
    }
}