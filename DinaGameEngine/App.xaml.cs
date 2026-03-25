using DinaGameEngine.Abstractions;
using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Injection manuelle des dépendances
            var fileService = new FileService();
            var generatedFileChecker = new GeneratedFileChecker(fileService);
            var logService = new LogService(fileService);
            var templateExtractor = new TemplateExtractor(logService);
            var codeGenerator = new CodeGenerator(fileService, logService, generatedFileChecker);
            var projectService = new ProjectService(fileService, logService, templateExtractor, codeGenerator);
            var dialogService = new DialogService();

            LocalizationManager.Register(typeof(Strings));
            
            // Vérification de la présence des DLL de DinaCSharp et DLACrypto.
            CheckLibsPath(templateExtractor);

            var navigationService = new NavigationService(fileService, generatedFileChecker, logService, templateExtractor,
                                                          codeGenerator, projectService, dialogService);
            navigationService.Navigate(NavigationRequest.ShowStartup);
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