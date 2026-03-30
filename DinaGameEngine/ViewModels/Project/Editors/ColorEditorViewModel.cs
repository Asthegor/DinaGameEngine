using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Add;
using DinaGameEngine.ViewModels.Project.Items;
using DinaGameEngine.Views.Project.Add;

namespace DinaGameEngine.ViewModels.Project.Editors
{
    public class ColorEditorViewModel : EditorViewModel<ColorViewModel>
    {
        private ICodeGenerator _codeGenerator;
        private IProjectService _projectService;
        private GameProjectModel _gameProjectModel;
        public ColorEditorViewModel(ICodeGenerator codeGenerator, IProjectService projectService, GameProjectModel gameProjectModel)
            : base([.. gameProjectModel.Colors])
        {
            _codeGenerator = codeGenerator;
            _projectService = projectService;
            _gameProjectModel = gameProjectModel;

            AddColorCommand = new RelayCommand(_ => OpenAddColorWindow());

            HeaderButtons = new ButtonBarViewModel();
            CreateButtons();

            ItemOpenRequested += (sender, _) =>
            {
                if (sender is ColorModel colorModel)
                    OpenAddColorWindow(colorModel);
            };
            ItemDeleteRequested += (sender, _) =>
            {
                if (sender is ColorModel colorModel)
                {
                    RemoveItem(colorModel);
                    _gameProjectModel.Colors.Remove(colorModel);
                    _codeGenerator.RemoveColor(_gameProjectModel, colorModel);
                    _projectService.UpdateJsonProjectFile(_gameProjectModel);
                }
            };
        }

        public RelayCommand AddColorCommand { get; }
        public void OpenAddColorWindow(ColorModel? colorModel = null)
        {
            bool colorConfirmed = false;

            var existingKeys = _gameProjectModel.Colors.Where(c => c.Id != colorModel?.Id).Select(c => c.Key).ToList();
            var addColorViewModel = new AddColorViewModel(existingKeys, colorModel);
            addColorViewModel.ColorConfirmed += (s, result) => colorConfirmed = result;

            var addColorWindow = new AddColorWindow { DataContext = addColorViewModel };
            addColorWindow.ShowDialog();

            if (colorConfirmed)
            {
                var existingColorModel = _gameProjectModel.Colors.FirstOrDefault(color => color.Id == colorModel?.Id);
                if (existingColorModel != null)
                {
                    _codeGenerator.RemoveColor(_gameProjectModel, colorModel!);
                    RemoveItem(colorModel!);
                    existingColorModel.Key = addColorViewModel.Key;
                    existingColorModel.R = addColorViewModel.R;
                    existingColorModel.G = addColorViewModel.G;
                    existingColorModel.B = addColorViewModel.B;
                    existingColorModel.A = addColorViewModel.A;
                    _codeGenerator.AddColor(_gameProjectModel, existingColorModel);
                    AddItem(existingColorModel);
                }
                else
                {
                    var newColorModel = new ColorModel
                    {
                        Key = addColorViewModel.Key,
                        R = addColorViewModel.R,
                        G = addColorViewModel.G,
                        B = addColorViewModel.B,
                        A = addColorViewModel.A
                    };
                    _gameProjectModel.Colors.Add(newColorModel);
                    _codeGenerator.AddColor(_gameProjectModel, newColorModel);
                    AddItem(newColorModel);
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
                Label = LocalizationManager.GetTranslation("ColorEditor_AddColor"),
                Command = AddColorCommand,
                Role = ButtonRole.Primary
            });
        }
    }
}
