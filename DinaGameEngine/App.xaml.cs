using DinaGameEngine.Abstractions;
using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Common;
using DinaGameEngine.Models;
using DinaGameEngine.Resources;
using DinaGameEngine.Services;
using DinaGameEngine.Templates;
using DinaGameEngine.ViewModels;
using DinaGameEngine.Views;
using DinaGameEngine.WPFServices;

using System.IO;
using System.Windows;

namespace DinaGameEngine
{
    public partial class App : Application
    {
        private IFileService _fileService;
        private IGeneratedFileChecker _generatedFileChecker;
        private ILogService _logService;
        private ITemplateExtractor _templateExtractor;
        private ICodeGenerator _codeGenerator;
        private IProjectService _projectService;
        private IDialogService _dialogService;
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Injection manuelle des dépendances
            _fileService = new FileService();
            _generatedFileChecker = new GeneratedFileChecker(_fileService);
            _logService = new LogService(_fileService);
            _templateExtractor = new TemplateExtractor(_logService);
            _codeGenerator = new CodeGenerator(_fileService, _logService, _generatedFileChecker);
            _projectService = new ProjectService(_fileService, _logService, _templateExtractor, _codeGenerator);
            _dialogService = new DialogService();

            LocalizationManager.Register(typeof(Strings));

            CheckLibsPath(_templateExtractor);

            ShowStartupWindow(StartupState.RecentProjects);
        }
        private void ShowStartupWindow(StartupState state)
        {
            _logService.Info($"ShowStartupWindow => {state}");
            var startupViewModel = new StartupViewModel(_projectService, _dialogService, _fileService, _logService, _templateExtractor)
            {
                CurrentState = state
            };
            var startupWindow = new StartupWindow(startupViewModel);

            startupViewModel.ProjectOpened += (sender, gameProjectModel) => OpenMainWindow(startupWindow, gameProjectModel);
            startupWindow.Show();
        }
        private void OpenMainWindow(Window windowToClose, GameProjectModel gameProjectModel)
        {
            var mainViewModel = new MainViewModel(_projectService, _dialogService, _fileService, _logService,
                                                  _templateExtractor, _codeGenerator, gameProjectModel);
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainViewModel.NewProjectRequested += (sender, e) => CloseMainWindowAndShowStartupWindow(mainWindow, StartupState.NewProject);
            mainViewModel.ProjectClosed += (sender, e) => CloseMainWindowAndShowStartupWindow(mainWindow, StartupState.RecentProjects);
            mainViewModel.ProjectLoaded += (sender, newGameProjectModel) => OpenMainWindow(mainWindow, newGameProjectModel);
            mainWindow.Show();
            windowToClose.Close();
        }
        private void CloseMainWindowAndShowStartupWindow(MainWindow mainWindow, StartupState startupState)
        {
            ShowStartupWindow(startupState);
            mainWindow.Close();
        }
        private static void CheckLibsPath(ITemplateExtractor templateExtractor)
        {
            var libsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Libs");
            var allFilesPresent = Directory.Exists(libsPath) &&
                                  templateExtractor.LibFiles
                                  .All(f => File.Exists(Path.Combine(libsPath, f)));

            if (!allFilesPresent)
            {
                Directory.CreateDirectory(libsPath);
                templateExtractor.ExtractLibs(libsPath);
            }
        }
    }
}