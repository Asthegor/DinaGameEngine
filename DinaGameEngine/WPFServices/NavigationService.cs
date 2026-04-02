using DinaGameEngine.Abstractions;
using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Interfaces;
using DinaGameEngine.Models;
using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels;
using DinaGameEngine.ViewModels.Startup;
using DinaGameEngine.Views;

namespace DinaGameEngine.WPFServices
{
    public class NavigationService(IFileService fileService, IGeneratedFileChecker generatedFileChecker, ILogService logService,
                                   ITemplateExtractor templateExtractor, ICodeGenerator codeGenerator, IProjectService projectService,
                                   IDialogService dialogService, IComponentGeneratorRegistry componentGeneratorRegistry,
                                   IComponentPropertiesViewModelFactory componentPropertiesViewModelFactory,
                                   IAddComponentViewModelFactory addComponentViewModelFactory)
        : INavigationService
    {
        private readonly IFileService _fileService = fileService;
        private readonly IGeneratedFileChecker _generatedFileChecker = generatedFileChecker;
        private readonly ILogService _logService = logService;
        private readonly ITemplateExtractor _templateExtractor = templateExtractor;
        private readonly ICodeGenerator _codeGenerator = codeGenerator;
        private readonly IProjectService _projectService = projectService;
        private readonly IDialogService _dialogService = dialogService;
        private readonly IComponentGeneratorRegistry _componentGeneratorRegistry = componentGeneratorRegistry;
        private readonly IComponentPropertiesViewModelFactory _componentPropertiesViewModelFactory = componentPropertiesViewModelFactory;
        private readonly IAddComponentViewModelFactory _addComponentViewModelFactory = addComponentViewModelFactory;

        private DinaWindow? _currentWindow;

        public void Navigate(NavigationRequest request, object? parameter = null)
        {
            switch (request)
            {
                case NavigationRequest.ShowStartup:
                    CreateAndShowStartupWindow(StartupState.RecentProjects);
                    break;
                case NavigationRequest.ShowStartupNewProject:
                    CreateAndShowStartupWindow(StartupState.NewProject);
                    break;
                case NavigationRequest.OpenProject:
                    if (parameter is GameProjectModel gameProjectModel)
                        _currentWindow = OpenMainWindow(_currentWindow, gameProjectModel);
                    break;
            }
        }

        private void CreateAndShowStartupWindow(StartupState startupState)
        {
            var startupViewModel = new StartupViewModel(_projectService, _dialogService, _fileService, _logService, _templateExtractor)
            {
                CurrentState = startupState
            };
            var startupWindow = new StartupWindow(startupViewModel);
            startupViewModel.NavigationRequested += (s, e) => Navigate(e.Request, e.Parameter);
            startupWindow.Show();
            _currentWindow?.Close();
            _currentWindow = startupWindow;
        }
        private MainWindow OpenMainWindow(DinaWindow? windowToClose, GameProjectModel gameProjectModel)
        {
            var mainViewModel = new MainViewModel(_fileService, _logService, _projectService, _dialogService, 
                                                  _codeGenerator, _componentGeneratorRegistry, gameProjectModel,
                                                  _componentPropertiesViewModelFactory, _addComponentViewModelFactory);
            var mainWindow = new MainWindow { DataContext = mainViewModel };
            mainViewModel.NavigationRequested += (s, e) => Navigate(e.Request, e.Parameter);
            mainWindow.Show();
            windowToClose?.Close();
            return mainWindow;
        }
    }
}
