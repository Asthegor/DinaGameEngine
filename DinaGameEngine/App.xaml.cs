using DinaGameEngine.Abstractions;
using DinaGameEngine.CodeGeneration;
using DinaGameEngine.CodeGeneration.ComponentGenerators;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Resources;
using DinaGameEngine.Services;
using DinaGameEngine.Templates;
using DinaGameEngine.ViewModels.Project.Add;
using DinaGameEngine.ViewModels.Project.Components;
using DinaGameEngine.WPFServices;

using System.IO;
using System.Windows;

using Application = System.Windows.Application;

namespace DinaGameEngine
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Injection manuelle des dépendances
            var fileService = new FileService();
            var dialogService = new DialogService();
            var generatedFileChecker = new GeneratedFileChecker(fileService);
            var logService = new LogService(fileService);
            var componentGeneratorRegistry = new ComponentGeneratorRegistry(logService);
            var templateExtractor = new TemplateExtractor(logService);
            var codeGenerator = new CodeGenerator(fileService, logService, generatedFileChecker, componentGeneratorRegistry, dialogService);
            var projectService = new ProjectService(fileService, logService, templateExtractor, codeGenerator);
            var componentPropertiesViewModelFactory = new ComponentPropertiesViewModelFactory();
            var addComponentViewModelFactory = new AddComponentViewModelFactory();
            var updateService = new DinaCSharpUpdateService(fileService, logService);

            // Enregistrement des composants
            componentGeneratorRegistry.Register(new TextComponentGenerator());
            componentGeneratorRegistry.Register(new MenuManagerComponentGenerator());
            componentGeneratorRegistry.Register(new ShadowTextComponentGenerator());



            LocalizationManager.Register(typeof(Strings));



            // Vérification de la présence des DLL de DinaCSharp et DLACrypto.
            string libsFolder = fileService.Combine(AppContext.BaseDirectory, "Libs");
            CheckLibsPath(libsFolder, templateExtractor);

            var navigationService = new NavigationService(fileService, generatedFileChecker, logService, templateExtractor,
                                                          codeGenerator, projectService, dialogService, componentGeneratorRegistry,
                                                          componentPropertiesViewModelFactory, addComponentViewModelFactory);
            navigationService.Navigate(NavigationRequest.ShowStartup);

            // Mise à jour des DLL de DinaCSharp et DLACrypto.
            _ = Task.Run(() => updateService.CheckAndUpdateLibsAsync(libsFolder));

        }
        private static void CheckLibsPath(string libsPath, TemplateExtractor templateExtractor)
        {
            var allFilesPresent = Directory.Exists(libsPath) && templateExtractor.LibFiles.All(f => File.Exists(Path.Combine(libsPath, f)));

            if (!allFilesPresent)
            {
                Directory.CreateDirectory(libsPath);
                templateExtractor.ExtractLibs(libsPath);
            }
        }
    }
}