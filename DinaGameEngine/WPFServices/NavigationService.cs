using DinaGameEngine.Abstractions;
using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Interfaces;
using DinaGameEngine.Models;
using DinaGameEngine.Services;
using DinaGameEngine.Templates;
using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels;
using DinaGameEngine.ViewModels.Project.Components;
using DinaGameEngine.Views;

using System.Windows;

namespace DinaGameEngine.WPFServices
{
    public class NavigationService : INavigationService
    {
        private IFileService _fileService;
        private IGeneratedFileChecker _generatedFileChecker;
        private ILogService _logService;
        private ITemplateExtractor _templateExtractor;
        private ICodeGenerator _codeGenerator;
        private IProjectService _projectService;
        private IDialogService _dialogService;
        private IComponentGeneratorRegistry _componentGeneratorRegistry;
        private IComponentPropertiesViewModelFactory _componentPropertiesViewModelFactory;
        private IAddComponentViewModelFactory _addComponentViewModelFactory;

        private DinaWindow? _currentWindow;
        public NavigationService(IFileService fileService, IGeneratedFileChecker generatedFileChecker, ILogService logService,
                                 ITemplateExtractor templateExtractor, ICodeGenerator codeGenerator,
                                 IProjectService projectService, IDialogService dialogService,
                                 IComponentGeneratorRegistry componentGeneratorRegistry,
                                 IComponentPropertiesViewModelFactory componentPropertiesViewModelFactory,
                                 IAddComponentViewModelFactory addComponentViewModelFactory)
        {
            _fileService = fileService;
            _generatedFileChecker = generatedFileChecker;
            _logService = logService;
            _templateExtractor = templateExtractor;
            _codeGenerator = codeGenerator;
            _projectService = projectService;
            _dialogService = dialogService;
            _componentGeneratorRegistry = componentGeneratorRegistry;
            _componentPropertiesViewModelFactory = componentPropertiesViewModelFactory;
            _addComponentViewModelFactory = addComponentViewModelFactory;
        }
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
