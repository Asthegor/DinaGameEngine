using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Common.Events;
using DinaGameEngine.Models;
using DinaGameEngine.Views;

using System.Collections.ObjectModel;
using System.Windows;

namespace DinaGameEngine.ViewModels
{
    public class StartupViewModel : ObservableObject
    {
        #region Champs privés
        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private readonly ILogService _logService;
        private readonly ITemplateExtractor _templateExtractor;

        private StartupState _currentState;

        private RecentProjectViewModel? _selectedProject;

        private string _newProjectName = string.Empty;
        private string _newProjectParentFolder = string.Empty;
        #endregion

        #region Constructeurs
        public StartupViewModel(IProjectService projectService, IDialogService dialogService, IFileService fileService, ILogService logService, ITemplateExtractor templateExtractor)
        {
            _projectService = projectService;
            _dialogService = dialogService;
            _fileService = fileService;
            _logService = logService;
            _templateExtractor = templateExtractor;

            NewProjectCommand = new RelayCommand(_ => NewProject());
            CancelNewProjectCommand = new RelayCommand(_ => CancelNewProject());

            BrowseFolderCommand = new RelayCommand(_ => BrowseFolder());
            ConfirmNewProjectCommand = new RelayCommand(execute: _ => ConfirmNewProject(),
                                                        canExecute: _ => Markers.Any() && Markers.All(m => !m.IsEmpty));
            OpenProjectCommand = new RelayCommand(_ => OpenProject());
            GoToMarkerValidationCommand = new RelayCommand(execute: _ => GoToMarkerValidation(),
                                                           canExecute: _ => !string.IsNullOrEmpty(NewProjectName) && _fileService.DirectoryExists(NewProjectParentFolder));
            GoToNewProjectCommand = new RelayCommand(_ => GoToNewProject());

            SelectProjectCommand = new RelayCommand(obj => SelectProject(obj));

            FooterButtons = new ButtonBarViewModel();
            UpdateFooterButtons();

            LoadRecentProjects();
        }
        #endregion

        #region Navigation
        public event EventHandler<NavigationRequestedEventArgs>? NavigationRequested;
        public StartupState CurrentState
        {
            get => _currentState;
            set
            {
                SetProperty(ref _currentState, value);
                OnPropertyChanged(nameof(WindowTitle));
                UpdateFooterButtons();
            }
        }
        public string WindowTitle
        {
            get
            {
                return CurrentState switch
                {
                    StartupState.RecentProjects => LocalizationManager.GetTranslation("App_Title"),
                    StartupState.NewProject => LocalizationManager.GetTranslation("NewProject_Title"),
                    StartupState.MarkerValidation => LocalizationManager.GetTranslation("Markers_Title"),
                    _ => throw new InvalidOperationException("")
                };
            }
        }
        private void UpdateFooterButtons()
        {
            FooterButtons.Buttons.Clear();
            switch (CurrentState)
            {
                case StartupState.RecentProjects:
                    FooterButtons.Buttons.Add(new ButtonDescriptor { Icon = "+", Label = LocalizationManager.GetTranslation("Startup_NewProject"), Command = GoToNewProjectCommand, Role = ButtonRole.Neutral });
                    FooterButtons.Buttons.Add(new ButtonDescriptor { Icon = "📂", Label = LocalizationManager.GetTranslation("Startup_Open"), Command = OpenProjectCommand, Role = ButtonRole.Secondary });
                    break;
                case StartupState.NewProject:
                    FooterButtons.Buttons.Add(new ButtonDescriptor { Label = LocalizationManager.GetTranslation("NewProject_Cancel"), Command = CancelNewProjectCommand, Role = ButtonRole.Secondary });
                    FooterButtons.Buttons.Add(new ButtonDescriptor { Label = LocalizationManager.GetTranslation("NewProject_Next"), Command = GoToMarkerValidationCommand, Role = ButtonRole.Primary });
                    break;
                case StartupState.MarkerValidation:
                    FooterButtons.Buttons.Add(new ButtonDescriptor { Label = LocalizationManager.GetTranslation("Markers_Previous"), Command = GoToNewProjectCommand, Role = ButtonRole.Secondary });
                    FooterButtons.Buttons.Add(new ButtonDescriptor { Label = LocalizationManager.GetTranslation("Markers_Create"), Command = ConfirmNewProjectCommand, Role = ButtonRole.Primary });
                    break;
            }
        }
        #endregion

        #region Projets récents
        public ObservableCollection<RecentProjectGroupViewModel> ProjectGroups { get; } = [];
        public RecentProjectViewModel? SelectedProject
        {
            get => _selectedProject;
            set => SetProperty(ref _selectedProject, value);
        }
        public RelayCommand SelectProjectCommand { get; }
        private void SelectProject(object? obj)
        {
            if (obj is RecentProjectViewModel recentProjectViewModel)
                SelectedProject = recentProjectViewModel;
        }
        private void LoadRecentProjects()
        {
            var appDirectory = _fileService.GetAppDataDirectory();
            var fullpath = _fileService.Combine(appDirectory, ProjectStructure.RecentProjectsFileName);
            ProjectGroups.Clear();

            if (!_fileService.FileExists(fullpath))
            {
                _logService.Warning($"Fichier '{ProjectStructure.RecentProjectsFileName}' inexistant.");
                return;
            }

            var jsonContent = _fileService.ReadAllText(fullpath);
            var listRecentProjects = JsonHelper.Deserialize<List<RecentProjectModel>>(jsonContent);

            if (listRecentProjects == null || listRecentProjects.Count == 0)
            {
                _logService.Warning($"Fichier '{ProjectStructure.RecentProjectsFileName}' vide ou corrompu.");
                return;
            }


            // Traitement des projets épinglés
            var pinnedProjects = listRecentProjects
                .Where(p => p.IsPinned)
                .OrderBy(p => p.PinOrder);

            if (pinnedProjects.Any())
            {
                var pinnedProjectsGroup = new RecentProjectGroupModel
                {
                    SectionName = LocalizationManager.GetTranslation("Startup_Pinned")
                };
                var pinnedProjectsGroupViewModel = new RecentProjectGroupViewModel(pinnedProjectsGroup);

                foreach (var project in pinnedProjects)
                    pinnedProjectsGroupViewModel.Projects.Add(CreateRecentProjectViewModel(project));

                pinnedProjectsGroupViewModel.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName == nameof(RecentProjectGroupViewModel.IsExpanded))
                        SaveSectionStates();
                };
                ProjectGroups.Add(pinnedProjectsGroupViewModel);
            }

            // Traitement des projets non-épinglés
            var unpinnedProjects = listRecentProjects
                .Where(p => !p.IsPinned)
                .OrderByDescending(p => p.LastOpenedAt);

            foreach (var project in unpinnedProjects)
            {
                var sectionName = GetSectionName(project.LastOpenedAt);

                var projectGroupViewModel = ProjectGroups
                    .FirstOrDefault(p => p.SectionName == sectionName)
                    ?? new RecentProjectGroupViewModel(
                        new RecentProjectGroupModel { SectionName = sectionName });

                projectGroupViewModel.Projects.Add(CreateRecentProjectViewModel(project));

                if (!ProjectGroups.Contains(projectGroupViewModel))
                {
                    projectGroupViewModel.PropertyChanged += (s, e) =>
                    {
                        if (e.PropertyName == nameof(RecentProjectGroupViewModel.IsExpanded))
                            SaveSectionStates();
                    };
                    ProjectGroups.Add(projectGroupViewModel);
                }
            }

            var nbProjects = listRecentProjects.Count;
            _logService.Info($"Chargement de la liste des projets récents réussie " +
                             $"({nbProjects} projet{(nbProjects > 1 ? "s" : "")} " +
                             $"chargé{(nbProjects > 1 ? "s" : "")})");

            LoadSectionStates();
        }
        private RecentProjectViewModel CreateRecentProjectViewModel(RecentProjectModel model)
        {
            var vm = new RecentProjectViewModel(model, _fileService, _projectService);
            vm.PinChanged += OnPinChanged;
            vm.ProjectOpened += OnProjectOpened;
            vm.ProjectRemoved += OnProjectRemoved;
            return vm;
        }
        private void OnProjectOpened(object? sender, ProjectOpenedEventArgs e)
        {
            _logService.Info($"Ouverture du projet '{e.Project.SolutionName}' réussie");
            NavigationRequested?.Invoke(this, new NavigationRequestedEventArgs(NavigationRequest.OpenProject, e.Project));
        }
        private void OnProjectRemoved(object? sender, EventArgs e)
        {
            if (sender is not RecentProjectViewModel vm)
                return;

            var listProjectGroups = ProjectGroups.SelectMany(p => p.Projects).ToList();
            var newListProjectGroups = listProjectGroups.Where(p => !(p.Name == vm.Name && p.ProjectFolderPath == vm.ProjectFolderPath)).ToList();
            var listModels = newListProjectGroups.Select(p => new RecentProjectModel
            {
                Name = p.Name,
                SolutionFolderPath = p.SolutionFolderPath,
                ProjectFolderPath = p.ProjectFolderPath,
                LastOpenedAt = p.LastOpenedAt,
                IsPinned = p.IsPinned,
                PinOrder = p.PinOrder
            }).ToList();

            var jsonContent = JsonHelper.Serialize(listModels);

            var recentProjectFile = _fileService.Combine(_fileService.GetAppDataDirectory(), ProjectStructure.RecentProjectsFileName);
            _fileService.WriteAllText(recentProjectFile, jsonContent);

            _logService.Info($"Projet '{vm.Name}' supprimé de la liste.");

            LoadRecentProjects();
        }
        private void OnPinChanged(object? sender, EventArgs e)
        {
            if (sender is not RecentProjectViewModel vm)
                return;

            var listProjects = ProjectGroups.SelectMany(g => g.Projects).ToList();

            var listPinnedProjects = listProjects.Where(p => p.IsPinned).OrderBy(p => p.PinOrder);

            if (vm.IsPinned)
            {
                vm.PinOrder = listPinnedProjects.Count();
            }
            else
            {
                vm.PinOrder = 0;
                var index = 1;
                foreach (var project in listPinnedProjects)
                {
                    if (project.PinOrder > index)
                        project.PinOrder = index;
                    index++;
                }
            }

            var listModels = listProjects.Select(p => new RecentProjectModel
            {
                Name = p.Name,
                SolutionFolderPath = p.SolutionFolderPath,
                ProjectFolderPath = p.ProjectFolderPath,
                LastOpenedAt = p.LastOpenedAt,
                IsPinned = p.IsPinned,
                PinOrder = p.PinOrder
            }).ToList();
            var jsonContent = JsonHelper.Serialize(listModels);

            var recentProjectFile = _fileService.Combine(_fileService.GetAppDataDirectory(), ProjectStructure.RecentProjectsFileName);
            _fileService.WriteAllText(recentProjectFile, jsonContent);

            _logService.Info(vm.IsPinned ? $"{vm.Name} épinglé (position: {vm.PinOrder})" : $"{vm.Name} désépinglé");

            LoadRecentProjects();
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
        private void LoadSectionStates()
        {
            var sectionStatesFile = _fileService.Combine(_fileService.GetAppDataDirectory(), ProjectStructure.SectionStatesFileName);
            if (!_fileService.FileExists(sectionStatesFile))
                return;

            var jsonContent = _fileService.ReadAllText(sectionStatesFile);

            var sectionStates = JsonHelper.Deserialize<Dictionary<string, bool>>(jsonContent);
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

            var jsonContent = JsonHelper.Serialize<Dictionary<string, bool>>(sectionStates);

            var sectionStatesFile = _fileService.Combine(_fileService.GetAppDataDirectory(), ProjectStructure.SectionStatesFileName);
            _fileService.WriteAllText(sectionStatesFile, jsonContent);

            _logService.Info("Sauvegarde des états des sections effectuée");
        }
        public ButtonBarViewModel FooterButtons { get; }
        #endregion

        #region Ouverture de projet
        public RelayCommand OpenProjectCommand { get; }
        private void OpenProject()
        {
            GameProjectModel? gameProjectModel;
            string? solutionFolderPath;
            if (SelectedProject != null)
            {
                solutionFolderPath = SelectedProject.SolutionFolderPath;
            }
            else
            {
                solutionFolderPath = _dialogService.OpenFolderDialog(LocalizationManager.GetTranslation("Dialog_OpenProject"));
                if (solutionFolderPath == null)
                    return; // L'utilisateur a annulé la commande
            }

            gameProjectModel = _projectService.OpenProject(solutionFolderPath);
            if (gameProjectModel == null)
            {
                _dialogService.ShowError(LocalizationManager.GetTranslation("Dialog_OpenProject"),
                    LocalizationManager.GetTranslation("Error_OpenProject", ProjectStructure.ProjectFileName));
                return; // Le dossier sélectionné ne contient pas de dichier 
            }

            _logService.Info($"Ouverture du projet '{gameProjectModel.SolutionName}' réussie");
            NavigationRequested?.Invoke(this, new NavigationRequestedEventArgs(NavigationRequest.OpenProject, gameProjectModel));
        }
        #endregion

        #region Nouveau projet
        public string NewProjectName
        {
            get => _newProjectName;
            set
            {
                SetProperty(ref _newProjectName, value);
                OnPropertyChanged(nameof(NewProjectFolderPreview));
            }
        }
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
        public RelayCommand GoToMarkerValidationCommand { get; }
        private void GoToMarkerValidation()
        {
            var newProjectModel = new NewProjectModel
            {
                Name = NewProjectName,
                ParentFolderPath = NewProjectParentFolder
            };

            var markers = _templateExtractor.GetMarkers(TemplateType.GameProject, newProjectModel);
            if (markers == null)
                return;

            Markers.Clear();
            foreach (var marker in markers)
                Markers.Add(new TemplateMarkerViewModel(marker));

            CurrentState = StartupState.MarkerValidation;
        }
        #endregion

        #region Validation des marqueurs
        public ObservableCollection<TemplateMarkerViewModel> Markers { get; } = [];
        public RelayCommand GoToNewProjectCommand { get; }
        private void GoToNewProject()
        {
            Markers.Clear();
            CurrentState = StartupState.NewProject;
        }
        public RelayCommand ConfirmNewProjectCommand { get; }
        private async void ConfirmNewProject()
        {
            var rootNamespaceMarker = Markers.First(m => m.Key == "__RootNamespace__");
            var newProjectModel = new NewProjectModel
            {
                Name = NewProjectName,
                ParentFolderPath = NewProjectParentFolder,
                RootNamespace = rootNamespaceMarker.Value
            };

            var markers = Markers.Select(vm => new TemplateMarkerModel
            {
                Key = vm.Key,
                Value = vm.Value
            }).ToList();

            var creatingWindow = new CreatingProjectWindow
            {
                Owner = Application.Current.MainWindow,
                DataContext = new { Message = LocalizationManager.GetTranslation("CreatingProject_Message", NewProjectName) }
            };
            creatingWindow.Show();

            var gameProjectModel = await _projectService.CreateProject(newProjectModel, markers);

            creatingWindow.Close();

            if (gameProjectModel == null)
                return;

            _dialogService.ShowInfo(
                LocalizationManager.GetTranslation("CreatingProject_Title"),
                LocalizationManager.GetTranslation("CreatingProject_Success", gameProjectModel.SolutionName));

            CurrentState = StartupState.RecentProjects;
            NavigationRequested?.Invoke(this, new NavigationRequestedEventArgs(NavigationRequest.OpenProject, gameProjectModel));
        }
        #endregion
    }
}
