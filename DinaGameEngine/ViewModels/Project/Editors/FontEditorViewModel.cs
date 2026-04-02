using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Add;
using DinaGameEngine.ViewModels.Project.Items;
using DinaGameEngine.ViewModels.Shared;
using DinaGameEngine.Views.Project.Add;

namespace DinaGameEngine.ViewModels.Project.Editors
{
    public class FontEditorViewModel : EditorViewModel<FontViewModel>
    {
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IProjectService _projectService;
        private readonly GameProjectModel _gameProjectModel;

        public FontEditorViewModel(IFileService fileService, IDialogService dialogService, ICodeGenerator codeGenerator, IProjectService projectService, GameProjectModel gameProjectModel)
            : base([.. gameProjectModel.Fonts])
        {
            _fileService = fileService;
            _dialogService = dialogService;
            _codeGenerator = codeGenerator;
            _projectService = projectService;
            _gameProjectModel = gameProjectModel;

            AddFontCommand = new RelayCommand(_ => OpenAddFontWindow());

            HeaderButtons = new ButtonBarViewModel();
            CreateButtons();

            ItemOpenRequested += (sender, _) =>
            {
                if (sender is FontModel fontModel)
                    OpenAddFontWindow(fontModel);
            };
            ItemDeleteRequested += (sender, _) =>
            {
                if (sender is FontModel fontModel)
                {
                    RemoveItem(fontModel);
                    _gameProjectModel.Fonts.Remove(fontModel);
                    _codeGenerator.RemoveFont(_gameProjectModel, fontModel);
                    _projectService.UpdateJsonProjectFile(_gameProjectModel);
                }
            };
        }

        public RelayCommand AddFontCommand { get; }
        public void OpenAddFontWindow(FontModel? fontModel = null)
        {
            bool fontConfirmed = false;

            var existingKeys = _gameProjectModel.Fonts.Where(c => c.Id != fontModel?.Id).Select(c => c.Key).ToList();
            var addFontViewModel = new AddFontViewModel(existingKeys, _gameProjectModel.RootPath, _fileService, _dialogService, fontModel);
            addFontViewModel.ItemConfirmed += (s, result) => fontConfirmed = result;

            var addFontWindow = new AddFontWindow { DataContext = addFontViewModel };
            addFontWindow.ShowDialog();

            if (fontConfirmed)
            {
                var existingFontModel = _gameProjectModel.Fonts.FirstOrDefault(font => font.Id == fontModel?.Id);
                if (existingFontModel != null)
                {
                    _codeGenerator.RemoveFont(_gameProjectModel, fontModel!);
                    RemoveItem(fontModel!);
                    existingFontModel.Key = addFontViewModel.Key;
                    existingFontModel.Size = addFontViewModel.Size;
                    existingFontModel.Spacing = addFontViewModel.Spacing;
                    existingFontModel.Style = addFontViewModel.Style;
                    existingFontModel.TtfRelativePath = $"../TTF_Files/{addFontViewModel.SelectedNamedItem!.Name}";
                    _codeGenerator.AddFont(_gameProjectModel, existingFontModel);
                    AddItem(existingFontModel);
                }
                else
                {
                    var newFontModel = new FontModel
                    {
                        Key = addFontViewModel.Key,
                        Size = addFontViewModel.Size,
                        Spacing = addFontViewModel.Spacing,
                        Style = addFontViewModel.Style,
                        TtfRelativePath = $"../TTF_Files/{addFontViewModel.SelectedNamedItem!.Name}",
                    };
                    _gameProjectModel.Fonts.Add(newFontModel);
                    _codeGenerator.AddFont(_gameProjectModel, newFontModel);
                    AddItem(newFontModel);
                }
                // Mise à jour de dina.project.json
                _projectService.UpdateJsonProjectFile(_gameProjectModel);
            }
        }
        public ButtonBarViewModel HeaderButtons { get; }
        private void CreateButtons()
        {
            HeaderButtons.Buttons.Clear();
            HeaderButtons.Buttons.Add(new ButtonDescriptor
            {
                Icon = DinaIcon.Add.ToGlyph(),
                Label = LocalizationManager.GetTranslation("FontEditor_AddFont"),
                Command = AddFontCommand,
                Role = ButtonRole.Primary
            });
        }

    }
}
