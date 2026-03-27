using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;

using System.Configuration;

namespace DinaGameEngine.ViewModels
{
    public class SceneCardViewModel : ObservableObject
    {
        private readonly SceneModel _sceneModel;
        private bool _isSelected;
        public SceneCardViewModel(SceneModel sceneModel)
        {
            _sceneModel = sceneModel;

            OpenCommand = new RelayCommand(_ => OpenScene());
            DeleteCommand = new RelayCommand(_ => DeleteScene());
            SelectCommand = new RelayCommand(_ => SelectScene());
            
            Buttons = new ButtonBarViewModel();
            CreateButtons();
        }
        public string Icon => DinaIcon.LookupEntities.ToGlyph();
        public string Name => _sceneModel.Name;
        public string Key => _sceneModel.Key;
        public int ComponentsCount => _sceneModel.Components.Count;
        public event EventHandler? SceneOpened;
        public event EventHandler? SceneDeleted;
        public event EventHandler? SceneSelected;

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
        public RelayCommand SelectCommand { get; }
        private void SelectScene()
        {
            SceneSelected?.Invoke(this, EventArgs.Empty);
        }

        public ButtonBarViewModel Buttons { get; }
        private void CreateButtons()
        {
            Buttons.Buttons.Clear();
            Buttons.Buttons.Add(new ButtonDescriptor { Icon = DinaIcon.Open.ToGlyph(), Command = OpenCommand, Role = ButtonRole.Primary });
            Buttons.Buttons.Add(new ButtonDescriptor { Icon = DinaIcon.Delete.ToGlyph(), Command = DeleteCommand, Role = ButtonRole.Secondary });
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
