using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Models;
using DinaGameEngine.Services;

using System.ComponentModel;
using System.Globalization;

namespace DinaGameEngine.ViewModels
{
    public class LanguageSelectionViewModel : ObservableObject
    {
        private readonly ILogService _logService;
        private readonly IProjectService _projectService;
        private readonly IFileService _fileService;
        private readonly GameProjectModel _gameProjectModel;

        private LanguageDefinition? _selectedLanguage;
        public LanguageSelectionViewModel(ILogService logService, IProjectService projectService, IFileService fileService, GameProjectModel gameProjectModel)
        {
            _logService = logService;
            _projectService = projectService;
            _fileService = fileService;
            _gameProjectModel = gameProjectModel;

            Languages = LanguageDefinitions.Languages;
            SelectedLanguage = Languages.FirstOrDefault(l => l.Code == CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, Languages[0]);

            LanguageValidationCommand = new RelayCommand(execute: _ => ValidateLanguage(), 
                                                         canExecute: _ => SelectedLanguage != null);
        }

        public IReadOnlyList<LanguageDefinition> Languages { get; }

        public LanguageDefinition? SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (value == _selectedLanguage)
                    return;
                SetProperty(ref _selectedLanguage, value);
                LanguageValidationCommand?.RaiseCanExecuteChanged();
            }
        }

        public RelayCommand LanguageValidationCommand { get; }

        public event EventHandler<string>? LanguageSelected;
        private void ValidateLanguage()
        {
            var selectedLanguageCode = SelectedLanguage!.Code;
            _gameProjectModel.DefaultLanguage = selectedLanguageCode;
            _gameProjectModel.Languages.Add(selectedLanguageCode);

            var resourcesPath = _fileService.Combine(_gameProjectModel.RootPath, "Core", "Resources");
            _fileService.CreateDirectory(resourcesPath);

            var namespaceName = "Core.Resources";
            var className = "Strings";

            _fileService.CreateResxFile(_fileService.Combine(resourcesPath, "Strings.resx"), namespaceName, className);
            _fileService.CreateResxDesignerFile(_fileService.Combine(resourcesPath, "Strings.Designer.cs"), namespaceName, className);

            _projectService.UpdateJsonProjectFile(_gameProjectModel);
            LanguageSelected?.Invoke(this, selectedLanguageCode);
        }
    }
}
