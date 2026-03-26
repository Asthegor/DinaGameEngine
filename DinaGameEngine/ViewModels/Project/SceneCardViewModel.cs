using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels
{
    public class SceneCardViewModel
    {
        private readonly SceneModel _sceneModel;
        public SceneCardViewModel(SceneModel sceneModel)
        {
            _sceneModel = sceneModel;

            OpenCommand = new RelayCommand(_ => OpenScene());
            DeleteCommand = new RelayCommand(_ => DeleteScene());

            Buttons = new ButtonBarViewModel();
            CreateButtons();
        }

        public string Name => _sceneModel.Name;
        public string Key => _sceneModel.Key;
        public int ComponentsCount => _sceneModel.Components.Count;
        public event EventHandler? SceneOpened;
        public event EventHandler? SceneDeleted;

        public RelayCommand OpenCommand { get; }
        private void OpenScene()
        {
            SceneOpened?.Invoke(_sceneModel, EventArgs.Empty);
        }
        public RelayCommand DeleteCommand { get; }
        private void DeleteScene()
        {
            SceneDeleted?.Invoke(_sceneModel, EventArgs.Empty);
        }

        public ButtonBarViewModel Buttons { get; }
        private void CreateButtons()
        {
            Buttons.Buttons.Clear();
            Buttons.Buttons.Add(new ButtonDescriptor { Icon = DinaIcon.Open.ToGlyph(), Command = OpenCommand, Role = ButtonRole.Primary });
            Buttons.Buttons.Add(new ButtonDescriptor { Icon = DinaIcon.Delete.ToGlyph(), Command = DeleteCommand, Role = ButtonRole.Secondary });
        }
    }
}
