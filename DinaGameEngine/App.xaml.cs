using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Resources;
using DinaGameEngine.Services;
using DinaGameEngine.Templates;
using DinaGameEngine.ViewModels;
using DinaGameEngine.Views;
using DinaGameEngine.WPFServices;

using System.Windows;

namespace DinaGameEngine
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Injection manuelle des dépendances
            IFileService fileService = new FileService();
            ILogService logService = new LogService(fileService);
            IDialogService dialogService = new DialogService();
            ITemplateExtractor templateExtractor = new TemplateExtractor(logService);
            IProjectService projectService = new ProjectService(fileService, logService, templateExtractor);

            LocalizationManager.Register(typeof(Strings));

            var viewModel = new StartupViewModel(projectService, dialogService, fileService, logService, templateExtractor);
            var startupWindow = new StartupWindow(viewModel);

            startupWindow.Show();
        }
    }
}