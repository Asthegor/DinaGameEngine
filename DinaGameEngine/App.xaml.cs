using DinaGameEngine.Abstractions;
using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Common;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Injection manuelle des dépendances
            IFileService fileService = new FileService();
            IGeneratedFileChecker generatedFileChecker = new GeneratedFileChecker(fileService);
            ILogService logService = new LogService(fileService);
            ITemplateExtractor templateExtractor = new TemplateExtractor(logService);
            ICodeGenerator codeGenerator = new CodeGenerator(fileService, logService, generatedFileChecker);
            IProjectService projectService = new ProjectService(fileService, logService, templateExtractor, codeGenerator);
            IDialogService dialogService = new DialogService();

            LocalizationManager.Register(typeof(Strings));

            CheckLibsPath(templateExtractor);

            var viewModel = new StartupViewModel(projectService, dialogService, fileService, logService, templateExtractor);
            var startupWindow = new StartupWindow(viewModel);

            viewModel.ProjectOpened += (sender, gameProjectModel) =>
            {
                var mainViewModel = new MainViewModel(projectService, dialogService, fileService, logService, templateExtractor, codeGenerator, gameProjectModel);

                var mainWindow = new MainWindow
                { DataContext = mainViewModel };
                mainWindow.Show();
                startupWindow.Close();
            };

            startupWindow.Show();
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