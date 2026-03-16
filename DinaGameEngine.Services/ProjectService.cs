using DinaGameEngine.Models;

using System.Text.Json;

namespace DinaGameEngine.Services
{
    public class ProjectService(IFileService fileService, ILogService logService) : IProjectService
    {
        private readonly IFileService _fileService = fileService;
        private readonly ILogService _logService = logService;

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
                gameProjectModel = JsonSerializer.Deserialize<GameProjectModel>(jsonRaw);
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
            _fileService.WriteAllText(filename, JsonSerializer.Serialize(gameProjectModel));

            UpdateRecentProjects(gameProjectModel);

            return gameProjectModel;
        }

        public GameProjectModel? CreateProject(NewProjectModel newProjectModel)
        {
            var projectFolder = _fileService.Combine(newProjectModel.ParentFolderPath, newProjectModel.Name);
            if (_fileService.DirectoryExists(projectFolder))
            {
                _logService.Warning($"Le répertoire '{projectFolder}' existe déjà.");
                return null;
            }

            _fileService.CreateDirectory(projectFolder);
            // TODO: créer la structure des fichiers du projet (.csproj, .cs, etc.)

            var gameProjectModel = new GameProjectModel
            {
                CreatedAt = DateTime.Now,
                DinaVersion = GetDinaVersion(),
                LastOpenedAt = DateTime.Now,
                Name = newProjectModel.Name,
                RootPath = projectFolder
            };

            var jsonSerialize = JsonSerializer.Serialize(gameProjectModel);
            var filename = _fileService.Combine(projectFolder, ProjectStructure.ProjectFileName);
            _fileService.WriteAllText(filename, jsonSerialize);

            UpdateRecentProjects(gameProjectModel);

            
            return gameProjectModel;
        }

        // Fonction permettant de récupérer la version de la DLL de DinaCSharp installée.
        private string GetDinaVersion()
        {
            // TODO: récupérer la version de la DLL de DInaCSharp
            return string.Empty;
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
                var datas = JsonSerializer.Deserialize<List<RecentProjectModel>>(jsonRaw);
                if (datas != null)
                {
                    recentProjectsList.AddRange(datas);
                    // Recherche et suppression du projet dans la liste
                    var oldRecentProjectModel = recentProjectsList.Find(p => p.Name == gameProjectModel.Name && p.ProjectFolderPath == gameProjectModel.RootPath);
                    if (oldRecentProjectModel != null)
                        recentProjectsList.Remove(oldRecentProjectModel);
                }
                else
                {
                    _logService.Warning($"Le fichier '{recentProjectsFileName}' est corrompu ou vide.");
                }
            }
            var recentProjectModel = CreateRecentProjectModel(gameProjectModel.Name, gameProjectModel.RootPath);
            recentProjectsList.Insert(0, recentProjectModel);

            // Limitation de la liste à 20 entrées
            if (recentProjectsList.Count > 20)
                recentProjectsList.RemoveRange(20, recentProjectsList.Count - 20);

            // Mise à jour de la liste des projets récents
            var jsonSerialized = JsonSerializer.Serialize(recentProjectsList);
            _fileService.WriteAllText(recentProjectsFileName, jsonSerialized);

            _logService.Info($"Mise à jour des projets récents effectuée.");
        }

        private RecentProjectModel CreateRecentProjectModel(string name,  string projectFilePath)
        {
            return new RecentProjectModel
            {
                LastOpenedAt = DateTime.Now,
                Name = name,
                ProjectFolderPath = projectFilePath
            };
        }
    }
}