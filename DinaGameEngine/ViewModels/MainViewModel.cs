using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Common.Events;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Editors;
using DinaGameEngine.Views;

using System.Collections.ObjectModel;
using System.Windows;

namespace DinaGameEngine.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private readonly ICodeGenerator _codeGenerator;
        private readonly GameProjectModel _gameProjectModel;

        private object? _currentViewModel;

        public MainViewModel(IProjectService projectService, IDialogService dialogService, IFileService fileService,
                             ICodeGenerator codeGenerator, GameProjectModel gameProjectModel)
        {
            _projectService = projectService;
            _dialogService = dialogService;
            _fileService = fileService;
            _codeGenerator = codeGenerator;
            _gameProjectModel = gameProjectModel;

            MainMenuFileNewProjectCommand = new RelayCommand(_ => NewProject());
            MainMenuFileLoadProjectCommand = new RelayCommand(_ => LoadProject());
            MainMenuFileSaveProjectCommand = new RelayCommand(_ => SaveProject());
            MainMenuFileCloseProjectCommand = new RelayCommand(_ => CloseProject());
            MainMenuFileCloseViewCommand = new RelayCommand(_ => CloseView(CurrentViewModel!));
            MainMenuFileCloseAllViewsCommand = new RelayCommand(_ => CloseAllViews());
            MainMenuFileQuitCommand = new RelayCommand(_ => Application.Current.Shutdown());
            MainMenuProjectAddSceneCommand = new RelayCommand(_ => AddNewScene());
            MainMenuProjectAddImageCommand = new RelayCommand(_ => AddNewImage());
            MainMenuProjectAddSoundCommand = new RelayCommand(_ => AddNewSound());
            MainMenuProjectAddFontCommand = new RelayCommand(_ => AddNewFont());
            MainMenuToolsShowTransitionsCommand = new RelayCommand(_ => ShowTransitions());
            MainMenuHelpShowNewsCommand = new RelayCommand(_ => ShowNews());
            MainMenuHelpShowAboutCommand = new RelayCommand(_ => ShowAbout());

            OpenWindows.CollectionChanged += (s, e) => OnPropertyChanged(nameof(HasMultipleViews));

            if (string.IsNullOrEmpty(_gameProjectModel.DefaultLanguage) || !LanguageDefinitions.Languages.Any(l => l.Code == _gameProjectModel.DefaultLanguage))
            {
                var vm = new LanguageSelectionViewModel(_projectService, _fileService, _gameProjectModel);
                var window = new LanguageSelectionWindow
                { DataContext = vm };
                vm.LanguageSelected += (s, e) => window.Close();
                window.ShowDialog();
            }
            _codeGenerator.GenerateAllDesigners(gameProjectModel);
            _codeGenerator.AddAllComponents(gameProjectModel);

            LoadScenes();

        }

        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                SetProperty(ref _currentViewModel, value);

                foreach (var item in OpenWindows)
                    item.IsActive = item.ViewModel == _currentViewModel;
            }
        }
        private void NewProject()
        {
            NavigationRequested?.Invoke(this, new NavigationRequestedEventArgs(NavigationRequest.ShowStartupNewProject));
        }
        private void LoadProject()
        {
            var path = _dialogService.OpenFolderDialog(LocalizationManager.GetTranslation("Dialog_OpenProject"));
            if (path == null)
                return;
            var gameProjectModel = _projectService.OpenProject(path);
            if (gameProjectModel == null)
            {
                _dialogService.ShowError(LocalizationManager.GetTranslation("Dialog_OpenProject"),
                                         LocalizationManager.GetTranslation("Error_OpenProject", ProjectStructure.ProjectFileName));
                return;
            }
            NavigationRequested?.Invoke(this, new NavigationRequestedEventArgs(NavigationRequest.OpenProject, gameProjectModel));
        }
        private void SaveProject()
        {
            _projectService.UpdateJsonProjectFile(_gameProjectModel);
            DialogWindow.Show(LocalizationManager.GetTranslation("MainMenu_File_Save_Message", _gameProjectModel.SolutionName),
                              LocalizationManager.GetTranslation("MainMenu_File_Save_Title", _gameProjectModel.SolutionName),
                              DialogIcon.Success, DialogButtons.OK);
        }
        private void CloseProject()
        {
            NavigationRequested?.Invoke(this, new NavigationRequestedEventArgs(NavigationRequest.ShowStartup));
        }
        private void AddNewScene()
        {
            bool sceneConfirmed = false;

            var addSceneViewModel = new AddSceneViewModel();
            addSceneViewModel.SceneConfirmed += (s, result) => sceneConfirmed = result;

            var addSceneWindow = new AddSceneWindow { DataContext = addSceneViewModel };
            addSceneWindow.ShowDialog();

            if (sceneConfirmed)
            {
                var sceneModel = new SceneModel { Name = addSceneViewModel.SceneName, Class = addSceneViewModel.ClassName, Key = addSceneViewModel.Key };
                _gameProjectModel.Scenes.Add(sceneModel);
                _codeGenerator.GenerateNewScene(_gameProjectModel, sceneModel);
                _projectService.UpdateJsonProjectFile(_gameProjectModel);

                OpenWindows.Select(w => w.ViewModel).OfType<ProjectHomeViewModel>().FirstOrDefault()?.AddScene(sceneModel);
            }

        }
        private void AddNewImage()
        {
            throw new NotImplementedException();
        }
        private void AddNewSound()
        {
            throw new NotImplementedException();
        }
        private void AddNewFont()
        {
            throw new NotImplementedException();
        }
        private void ShowTransitions()
        {
            throw new NotImplementedException();
        }
        private void ShowNews()
        {
            throw new NotImplementedException();
        }
        private void ShowAbout()
        {
            throw new NotImplementedException();
        }

        public RelayCommand MainMenuFileNewProjectCommand { get; }
        public RelayCommand MainMenuFileLoadProjectCommand { get; }
        public RelayCommand MainMenuFileSaveProjectCommand { get; }
        public RelayCommand MainMenuFileCloseProjectCommand { get; }
        public RelayCommand MainMenuFileQuitCommand { get; }
        public RelayCommand MainMenuProjectAddSceneCommand { get; }
        public RelayCommand MainMenuProjectAddImageCommand { get; }
        public RelayCommand MainMenuProjectAddSoundCommand { get; }
        public RelayCommand MainMenuProjectAddFontCommand { get; }
        public RelayCommand MainMenuToolsShowTransitionsCommand { get; }
        public RelayCommand MainMenuFileCloseViewCommand { get; }
        public RelayCommand MainMenuFileCloseAllViewsCommand { get; }
        public RelayCommand MainMenuHelpShowNewsCommand { get; }
        public RelayCommand MainMenuHelpShowAboutCommand { get; }
        public bool HasMultipleViews => OpenWindows.Count >= 2;
        public string WindowTitle => $"Dina Game Engine - {_gameProjectModel.SolutionName}";

        private void LoadScenes()
        {
            var projectHomeViewModel = new ProjectHomeViewModel(_gameProjectModel);
            projectHomeViewModel.SceneOpenRequested += OnSceneOpenRequested;
            projectHomeViewModel.SceneDeleteRequested += OnSceneDeleteRequested;
            projectHomeViewModel.EditorRequested += OnEditorRequested;
            CurrentViewModel = projectHomeViewModel;
            var title = _gameProjectModel.SolutionName;
            var windowMenuItemViewModel = new WindowMenuItemViewModel(title: title,
                                                                      viewModel: projectHomeViewModel,
                                                                      activateAction: (obj) => CurrentViewModel = obj,
                                                                      closeAction: _ => { },
                                                                      isClosable: false);
            if (OpenWindows.FirstOrDefault(w => w?.Title == title && w.ViewModel == projectHomeViewModel, null) == null)
                OpenWindows.Add(windowMenuItemViewModel);
        }

        private void OnEditorRequested(object? sender, ProjectView view)
        {
            var editorType = view switch
            {
                ProjectView.Localization => typeof(LocalizationEditorViewModel),
                ProjectView.Fonts => typeof(FontEditorViewModel),
                ProjectView.Images => typeof(ImageEditorViewModel),
                ProjectView.Audio => typeof(AudioEditorViewModel),
                ProjectView.Colors => typeof(ColorEditorViewModel),
                ProjectView.Inputs => typeof(InputEditorViewModel),
                ProjectView.ProjectDefaultSettings => typeof(ConfigEditorViewModel),
                _ => null
            };

            if (editorType == null)
                return;
            var existingWindow = OpenWindows.FirstOrDefault(w => w.ViewModel?.GetType() == editorType);
            if (existingWindow != null)
            {
                CurrentViewModel = existingWindow.ViewModel;
                return;
            }
            object? editorViewModel = view switch
            {
                ProjectView.Localization => new LocalizationEditorViewModel(),
                ProjectView.Fonts => new FontEditorViewModel(),
                ProjectView.Images => new ImageEditorViewModel(),
                ProjectView.Audio => new AudioEditorViewModel(),
                ProjectView.Colors => new ColorEditorViewModel(),
                ProjectView.Inputs => new InputEditorViewModel(),
                ProjectView.ProjectDefaultSettings => new ConfigEditorViewModel(),
                _ => null
            };

            if (editorViewModel == null)
                return;
            AddViewModelToOpenWindows(editorViewModel, title: LocalizationManager.GetTranslation($"Nav_{view}"));
        }

        private void OnSceneOpenRequested(object? sender, EventArgs e)
        {
            if (sender is SceneModel sceneModel)
            {
                var existingWindow = OpenWindows.FirstOrDefault(w => w.ViewModel is SceneEditorViewModel vm
                                                                      && vm.SceneId == sceneModel.Id);
                if (existingWindow != null)
                {
                    CurrentViewModel = existingWindow.ViewModel;
                    return;
                }

                var sceneEditorViewModel = new SceneEditorViewModel(sceneModel);
                AddViewModelToOpenWindows(sceneEditorViewModel, sceneModel.Name);
            }
        }
        private void OnSceneDeleteRequested(object? sender, EventArgs e)
        {
            if (sender is SceneModel sceneModel)
            {
                var result = DialogWindow.Show(LocalizationManager.GetTranslation("SceneDelete_Confirmation", sceneModel.Name, sceneModel.Class, sceneModel.Key),
                                               LocalizationManager.GetTranslation("SceneDelete_Confirmation_Title", sceneModel.Name),
                                               DialogIcon.Warning, DialogButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Suppression des fichiers
                    var sceneDesignerFile = _fileService.Combine(_gameProjectModel.RootPath, "Scenes", $"{sceneModel.Class}.Designer.cs");
                    _fileService.DeleteFile(sceneDesignerFile);
                    var sceneUserFile = _fileService.Combine(_gameProjectModel.RootPath, "Scenes", $"{sceneModel.Class}.cs");
                    _fileService.DeleteFile(sceneUserFile);

                    // Suppression de la scène dans SceneKey + GameProject.Designer
                    _codeGenerator.RemoveScene(_gameProjectModel, sceneModel);

                    // Mise à jour du fichier dina.project.json
                    _projectService.RemoveSceneFromProject(_gameProjectModel, sceneModel);

                    OpenWindows.Select(w => w.ViewModel).OfType<ProjectHomeViewModel>().FirstOrDefault()?.RemoveScene(sceneModel);
                }
            }
        }
        public ObservableCollection<WindowMenuItemViewModel> OpenWindows { get; } = [];

        public event EventHandler<NavigationRequestedEventArgs>? NavigationRequested;

        private void CloseView(object viewModel)
        {
            var viewToClose = OpenWindows.FirstOrDefault(w => w.ViewModel == viewModel);
            if (viewToClose != null)
            {
                OpenWindows.Remove(viewToClose);

                if (viewToClose.ViewModel == CurrentViewModel)
                {
                    var lastWindow = OpenWindows.ElementAt(OpenWindows.Count - 1);
                    lastWindow.IsActive = true;
                    CurrentViewModel = lastWindow.ViewModel;
                }
            }
        }
        private void CloseAllViews()
        {
            var toRemove = OpenWindows.Skip(1).ToList();
            foreach (var item in toRemove)
                OpenWindows.Remove(item);
        }

        private void AddViewModelToOpenWindows(object? viewModel, string title, bool isClosable = true)
        {
            if (viewModel == null)
                return;
            OpenWindows.Add(new WindowMenuItemViewModel(title,
                                                        viewModel,
                                                        activateAction: (obj) => CurrentViewModel = obj,
                                                        closeAction: obj => CloseView(viewModel),
                                                        isClosable));
            CurrentViewModel = viewModel;
        }
    }
}
