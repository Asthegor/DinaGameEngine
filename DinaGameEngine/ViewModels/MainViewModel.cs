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
                {  DataContext=vm };
                vm.LanguageSelected += (s, e) => window.Close();
                window.ShowDialog();
            }
            _codeGenerator.GenerateAllFiles(gameProjectModel);
            _codeGenerator.AddAllComponents(gameProjectModel);

            LoadScenes();
        }

        private void NewProject()
        {
            throw new NotImplementedException();
        }
        private void LoadProject()
        {
            throw new NotImplementedException();
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

        private static void LoadScenes()
        {

        }
    }
}
