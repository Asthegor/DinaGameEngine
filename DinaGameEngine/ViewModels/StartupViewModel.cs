using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Models;
using DinaGameEngine.Services;

using System.Collections.ObjectModel;
using System.Text.Json;

namespace DinaGameEngine.ViewModels
{
    public class StartupViewModel : ObservableObject
    {
        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private readonly ILogService _logService;

        public StartupViewModel(IProjectService projectService, IDialogService dialogService, IFileService fileService, ILogService logService)
        {
            _projectService = projectService;
            _dialogService = dialogService;
            _fileService = fileService;
            _logService = logService;

            NewProjectCommand = new RelayCommand(_ => NewProject());
            CancelNewProjectCommand = new RelayCommand(_ => CancelNewProject());

            BrowseFolderCommand = new RelayCommand(_ => BrowseFolder());
            ConfirmNewProjectCommand = new RelayCommand(_ => ConfirmNewProject(), _ => !string.IsNullOrEmpty(NewProjectName) && _fileService.DirectoryExists(NewProjectParentFolder));
            OpenProjectCommand = new RelayCommand(_ => OpenProject());
            OpenSelectedProjectCommand = new RelayCommand(_ => OpenSelectedProject(), _ => SelectedProject != null);
            TogglePinCommand = new RelayCommand(param => TogglePin(param));

            LoadRecentProjects();
        }

        public event EventHandler<GameProjectModel>? ProjectOpened;

        public ObservableCollection<RecentProjectGroupModel> ProjectGroups { get; } = [];
        private RecentProjectModel? _selectedProject;
        public RecentProjectModel? SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }

        private void LoadRecentProjects()
        {

            // Lecture fichier "recentProjects.json"
            var appDirectory = _fileService.GetAppDataDirectory();
            var fullpath = _fileService.Combine(appDirectory, ProjectStructure.RecentProjectsFileName);

            // On s'arrête si le fichier n'existe pas
            if (!_fileService.FileExists(fullpath))
            {
                _logService.Warning($"Fichier '{ProjectStructure.RecentProjectsFileName}' inexistant.");
                return;
            }

            // lecture du fichier de configuration
            var jsonContent = _fileService.ReadAllText(fullpath);
            var listRecentProjects = JsonSerializer.Deserialize<List<RecentProjectModel>>(jsonContent);

            if (listRecentProjects == null || listRecentProjects.Count == 0)
            {
                _logService.Warning($"Fichier '{ProjectStructure.RecentProjectsFileName}' vide ou corrompu.");
                return;
            }

            // On efface la liste des projets
            ProjectGroups.Clear();


            // Traitement des projets épinglés
            var pinnedProjects = listRecentProjects.Where(p => p.IsPinned).OrderBy(p => p.PinOrder);
            if (pinnedProjects.Any())
            {
                // Crée la projectGroup des projets épinglés
                var pinnedProjectsGroup = new RecentProjectGroupModel { SectionName = LocalizationManager.GetTranslation("Startup_Pinned") };
                foreach (var project in pinnedProjects)
                    pinnedProjectsGroup.Projects.Add(project);
                // Ajoute la projectGroup des épinglés à la liste des sections
                ProjectGroups.Add(pinnedProjectsGroup);

                pinnedProjectsGroup.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(RecentProjectGroupModel.IsExpanded))
                        SaveSectionStates();
                };
            }

            // Traitement des projets non-épinglés
            var unpinnedProjects = listRecentProjects.Where(p => !p.IsPinned).OrderByDescending(p => p.LastOpenedAt);
            if (unpinnedProjects.Any())
            {
                // On boucle sur les projets non-épinglés pour trouver leurs sections
                foreach (var project in unpinnedProjects)
                {
                    var sectionName = GetSectionName(project.LastOpenedAt);
                    // On récupère le RecentProjectGroupModel de la projectGroup
                    // ou on en crée un nouveau s'il n'existe pas
                    var projectGroup = ProjectGroups.FirstOrDefault(p => p.SectionName == sectionName)
                                       ?? new RecentProjectGroupModel { SectionName = sectionName, Projects = [] };
                    // On ajoute le projet à la liste des projets de la projectGroup
                    projectGroup.Projects.Add(project);

                    // Si le groupe n'est pas présent dans ProjectGroups, on l'ajoute
                    if (!ProjectGroups.Contains(projectGroup))
                    {
                        projectGroup.PropertyChanged += (s, e) =>
                        {
                            if (e.PropertyName == nameof(RecentProjectGroupModel.IsExpanded))
                                SaveSectionStates();
                        };
                        ProjectGroups.Add(projectGroup);
                    }
                }
            }
            var nbProjects = listRecentProjects.Count;
            _logService.Info($"Chargement de la liste des projets récents réussie ({nbProjects} projet{(nbProjects > 1 ? "s" : "")} chargé{(nbProjects > 1 ? "s" : "")})");

            LoadSectionStates();
        }

        private static string GetSectionName(DateTime lastOpenedAt)
        {
            DateTime today = DateTime.Today;
            DateTime monday = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime firstDay = new DateTime(today.Year, today.Month, 1);

            if (lastOpenedAt.Date == today)
                return LocalizationManager.GetTranslation("Startup_Today");
            if (lastOpenedAt.Date == today.AddDays(-1))
                return LocalizationManager.GetTranslation("Startup_Yesterday");
            if (lastOpenedAt.Date >= monday)
                return LocalizationManager.GetTranslation("Startup_ThisWeek");
            if (today.DayOfWeek <= DayOfWeek.Tuesday && lastOpenedAt.Date >= monday.AddDays(-7))
                return LocalizationManager.GetTranslation("Startup_LastWeek");
            if (lastOpenedAt.Date >= firstDay)
                return LocalizationManager.GetTranslation("Startup_ThisMonth");
            return LocalizationManager.GetTranslation("Startup_Older");
        }


        public RelayCommand NewProjectCommand { get; }
        private void NewProject()
        {
            CurrentState = StartupState.NewProject;
            NewProjectName = string.Empty;
            NewProjectParentFolder = string.Empty;
        }
        public RelayCommand CancelNewProjectCommand { get; }
        private void CancelNewProject()
        {
            CurrentState = StartupState.RecentProjects;
        }

        public RelayCommand BrowseFolderCommand { get; }
        private void BrowseFolder()
        {
            var folder = _dialogService.OpenFolderDialog(LocalizationManager.GetTranslation("NewProject_Browse"));
            if (folder != null)
                NewProjectParentFolder = folder;
        }

        //private void CreateNewProject()
        //{
        //    var newProjectModel = _dialogService.ShowNewProjectDialog();
        //    if (newProjectModel == null)
        //        return;

        //    var gameProjectModel = _projectService.CreateProject(newProjectModel);
        //    if (gameProjectModel == null)
        //    {
        //        _dialogService.ShowError(LocalizationManager.GetTranslation("NewProject_Title"), LocalizationManager.GetTranslation("Error_NewProject"));
        //        return;
        //    }

        //    _logService.Info($"Création du projet '{gameProjectModel.Name}' réussie.");
        //    NotifyProjectOpened(gameProjectModel);
        //}

        public RelayCommand OpenProjectCommand { get; }
        private void OpenProject()
        {
            GameProjectModel? gameProjectModel;
            string? projectFolderPath;
            if (SelectedProject != null)
            {
                projectFolderPath = SelectedProject.ProjectFolderPath;
            }
            else
            {
                projectFolderPath = _dialogService.OpenFolderDialog(LocalizationManager.GetTranslation("Dialog_OpenProject"));
                if (projectFolderPath == null)
                    return; // L'utilisateur a annulé la commande
            }

            gameProjectModel = _projectService.OpenProject(projectFolderPath);
            if (gameProjectModel == null)
            {
                _dialogService.ShowError(LocalizationManager.GetTranslation("Dialog_OpenProject"),
                    LocalizationManager.GetTranslation("Error_OpenProject", ProjectStructure.ProjectFileName));
                return; // Le dossier sélectionné ne contient pas de dichier 
            }

            _logService.Info($"Ouverture du projet '{gameProjectModel.Name}' réussie");
            NotifyProjectOpened(gameProjectModel);
        }

        public RelayCommand OpenSelectedProjectCommand { get; }
        private void OpenSelectedProject()
        {
            var gameProjectModel = _projectService.OpenProject(SelectedProject!.ProjectFolderPath);
            if (gameProjectModel != null)
            {
                _logService.Info($"Ouverture du projet '{gameProjectModel.Name}' réussie");
                NotifyProjectOpened(gameProjectModel);
            }
        }

        public RelayCommand TogglePinCommand { get; }
        private void TogglePin(object? param)
        {
            if (param is not RecentProjectModel selectedProject)
                return;

            var listProjects = ProjectGroups.SelectMany(p => p.Projects);

            selectedProject.IsPinned = !selectedProject.IsPinned;

            // Récupération de la liste des projets épinglés à jour (ajout ou exclusion du projet sélectionné)
            var listPinnedProjects = listProjects.Where(p => p.IsPinned).OrderBy(p => p.PinOrder);

            if (selectedProject.IsPinned)
            {
                // PinOrder commence à 1
                selectedProject.PinOrder = listPinnedProjects.Count();
            }
            else
            {
                // Réinitialisation du PinOrder du projet sélectionné
                selectedProject.PinOrder = 0;

                // Mise à jour de PinOrder des projets épinglés
                var index = 1;
                foreach (var project in listPinnedProjects)
                {
                    if (project.PinOrder > index)
                        project.PinOrder = index;
                    index++;
                }
            }

            var jsonContent = JsonSerializer.Serialize<List<RecentProjectModel>>([.. listProjects]);

            var recentProjectFile = _fileService.Combine(_fileService.GetAppDataDirectory(), ProjectStructure.RecentProjectsFileName);
            _fileService.WriteAllText(recentProjectFile, jsonContent);

            if (selectedProject.IsPinned)
                _logService.Info($"{selectedProject.Name} épinglé (position: {selectedProject.PinOrder})");
            else
                _logService.Info($"{selectedProject.Name} désépinglé");

            LoadRecentProjects();
        }

        private void NotifyProjectOpened(GameProjectModel gameProjectModel)
        {
            ProjectOpened?.Invoke(this, gameProjectModel);
        }

        private void LoadSectionStates()
        {
            var sectionStatesFile = _fileService.Combine(_fileService.GetAppDataDirectory(), ProjectStructure.SectionStatesFileName);
            if (!_fileService.FileExists(sectionStatesFile))
                return;

            var jsonContent = _fileService.ReadAllText(sectionStatesFile);

            var sectionStates = JsonSerializer.Deserialize<Dictionary<string, bool>>(jsonContent);
            if (sectionStates != null)
            {
                foreach (var projectGroup in ProjectGroups)
                    projectGroup.IsExpanded = (sectionStates.TryGetValue(projectGroup.SectionName, out bool value)) ? value : true;
            }
        }
        private void SaveSectionStates()
        {
            Dictionary<string, bool> sectionStates = [];
            foreach (var projectGroup in ProjectGroups)
                sectionStates[projectGroup.SectionName] = projectGroup.IsExpanded;

            var jsonContent = JsonSerializer.Serialize<Dictionary<string, bool>>(sectionStates);

            var sectionStatesFile = _fileService.Combine(_fileService.GetAppDataDirectory(), ProjectStructure.SectionStatesFileName);
            _fileService.WriteAllText(sectionStatesFile, jsonContent);

            _logService.Info("Sauvegarde des états des sections effectuée");
        }

        private StartupState _currentState;
        public StartupState CurrentState
        {
            get => _currentState;
            set
            {
                SetProperty(ref _currentState, value);
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        public string WindowTitle => LocalizationManager.GetTranslation(CurrentState == StartupState.RecentProjects ? "App_Title" : "NewProject_Title");

        private string _newProjectName = string.Empty;
        public string NewProjectName
        {
            get => _newProjectName;
            set
            {
                SetProperty(ref _newProjectName, value);
                OnPropertyChanged(nameof(NewProjectFolderPreview));
            }
        }
        private string _newProjectParentFolder = string.Empty;
        public string NewProjectParentFolder
        {
            get => _newProjectParentFolder;
            set
            {
                SetProperty(ref _newProjectParentFolder, value);
                OnPropertyChanged(nameof(NewProjectFolderPreview));
            }
        }
        public string NewProjectFolderPreview => _fileService.Combine(NewProjectParentFolder, NewProjectName, NewProjectName.Replace(" ", ""));

        public RelayCommand ConfirmNewProjectCommand { get; }
        private void ConfirmNewProject()
        {
            var newProjectModel = new NewProjectModel
            {
                Name = NewProjectName,
                ParentFolderPath = NewProjectParentFolder
            };
            var gameProjectModel = _projectService.CreateProject(newProjectModel);
            if (gameProjectModel == null)
                return;

            CurrentState = StartupState.RecentProjects;
            NotifyProjectOpened(gameProjectModel);
        }
    }
}
