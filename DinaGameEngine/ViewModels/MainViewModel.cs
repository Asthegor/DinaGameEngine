using DinaGameEngine.Abstractions;
using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Models;
using DinaGameEngine.Views;

using System.Windows;

namespace DinaGameEngine.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private readonly ILogService _logService;
        private readonly ITemplateExtractor _templateExtractor;
        private readonly ICodeGenerator _codeGenerator;

        private readonly GameProjectModel _gameProjectModel;

        private Dictionary<Type, object?> _viewModels = [];

        public MainViewModel(IProjectService projectService, IDialogService dialogService, IFileService fileService,
                             ILogService logService, ITemplateExtractor templateExtractor, ICodeGenerator codeGenerator,
                             GameProjectModel gameProjectModel)
        {
            _projectService = projectService;
            _dialogService = dialogService;
            _fileService = fileService;
            _logService = logService;
            _templateExtractor = templateExtractor;
            _codeGenerator = codeGenerator;
            _gameProjectModel = gameProjectModel;

            MainMenuFileNewProjectCommand = new RelayCommand(_ => NewProject());
            MainMenuFileLoadCommand = new RelayCommand(_ => LoadProject());
            MainMenuFileSaveCommand = new RelayCommand(_ => SaveProject());
            MainMenuFileCloseCommand = new RelayCommand(_ => CloseProject());
            MainMenuFileQuitCommand = new RelayCommand(_ => Application.Current.Shutdown());
            MainMenuProjectAddSceneCommand = new RelayCommand(_ => AddNewScene());
            MainMenuProjectAddImageCommand = new RelayCommand(_ => AddNewImage());
            MainMenuProjectAddSoundCommand = new RelayCommand(_ => AddNewSound());
            MainMenuProjectAddFontCommand = new RelayCommand(_ => AddNewFont());
            MainMenuToolsShowTransitionsCommand = new RelayCommand(_ => ShowTransitions());
            MainMenuHelpShowNewsCommand = new RelayCommand(_ => ShowNews());
            MainMenuHelpShowAboutCommand = new RelayCommand(_ => ShowAbout());


            if (string.IsNullOrEmpty(_gameProjectModel.DefaultLanguage) || !LanguageDefinitions.Languages.Any(l => l.Code == _gameProjectModel.DefaultLanguage))
            {
                var vm = new LanguageSelectionViewModel(_logService, _projectService, _fileService, _gameProjectModel);
                var window = new LanguageSelectionWindow
                { DataContext = vm };
                vm.LanguageSelected += (s, e) => window.Close();
                window.ShowDialog();
            }
            _codeGenerator.GenerateAllDesigners(gameProjectModel);
            _codeGenerator.AddAllComponents(gameProjectModel);

            LoadScenes();
        }

        private object? _currentViewModel;
        public object? CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                SetProperty(ref _currentViewModel, value);
                if (_currentViewModel != null)
                    _viewModels[_currentViewModel.GetType()] = _currentViewModel;
            }
        }
        private void NewProject()
        {
            throw new NotImplementedException();
        }
        private void LoadProject()
        {
        }
        private void SaveProject()
        {
            throw new NotImplementedException();
        }
        private void CloseProject()
        {
            throw new NotImplementedException();
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

                if (_viewModels.TryGetValue(typeof(ProjectHomeViewModel), out var vm) && vm is ProjectHomeViewModel projectHomeViewModel)
                    projectHomeViewModel.AddScene(sceneModel);
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
        public RelayCommand MainMenuFileLoadCommand { get; }
        public RelayCommand MainMenuFileSaveCommand { get; }
        public RelayCommand MainMenuFileCloseCommand { get; }
        public RelayCommand MainMenuFileQuitCommand { get; }
        public RelayCommand MainMenuProjectAddSceneCommand { get; }
        public RelayCommand MainMenuProjectAddImageCommand { get; }
        public RelayCommand MainMenuProjectAddSoundCommand { get; }
        public RelayCommand MainMenuProjectAddFontCommand { get; }
        public RelayCommand MainMenuToolsShowTransitionsCommand { get; }
        public RelayCommand MainMenuHelpShowNewsCommand { get; }
        public RelayCommand MainMenuHelpShowAboutCommand { get; }

        public string WindowTitle => $"Dina Game Engine - {_gameProjectModel.SolutionName}";

        private void LoadScenes()
        {
            var projectHomeViewModel = new ProjectHomeViewModel(_gameProjectModel);
            projectHomeViewModel.SceneOpenRequested += OnSceneOpenRequested;
            projectHomeViewModel.SceneDeleteRequested += OnSceneDeleteRequested;
            CurrentViewModel = projectHomeViewModel;
        }

        private void OnSceneOpenRequested(object? sender, EventArgs e)
        {
            // TODO : ouvrir la scène dans la zone centrale
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

                    if (_viewModels.TryGetValue(typeof(ProjectHomeViewModel), out var vm) && vm is ProjectHomeViewModel projectHomeViewModel)
                        projectHomeViewModel.RemoveScene(sceneModel);
                }
            }
        }
    }
}
