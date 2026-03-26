using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DinaGameEngine.ViewModels
{
    public class ProjectHomeViewModel : ObservableObject
    {
        private GameProjectModel _gameProjectModel;
        public ProjectHomeViewModel(GameProjectModel gameProjectModel)
        {
            _gameProjectModel = gameProjectModel;
            Scenes.Clear();
            foreach (var scene in _gameProjectModel.Scenes)
                AddScene(scene);

            NavigationButtons = new ButtonBarViewModel { Orientation = Orientation.Vertical };
            CreateButtons();
        }

        public event EventHandler<ProjectView>? EditorRequested;

        public event EventHandler? SceneOpenRequested;
        public event EventHandler? SceneDeleteRequested;

        public ObservableCollection<SceneCardViewModel> Scenes { get; } = [];

        public void AddScene(SceneModel sceneModel)
        {
            var sceneCardViewModel = new SceneCardViewModel(sceneModel);
            sceneCardViewModel.SceneOpened += (sender, eventArgs) => SceneOpenRequested?.Invoke(sender, eventArgs);
            sceneCardViewModel.SceneDeleted += (sender, eventArgs) => SceneDeleteRequested?.Invoke(sender, eventArgs);
            Scenes.Add(sceneCardViewModel);
        }
        public void RemoveScene(SceneModel sceneModel)
        {
            var sceneCardViewModel = Scenes.FirstOrDefault(s => s.Name == sceneModel.Name);
            if (sceneCardViewModel != null)
                Scenes.Remove(sceneCardViewModel);
        }
        public ButtonBarViewModel NavigationButtons { get; }
        private void CreateButtons()
        {
            NavigationButtons.Buttons.Clear();
            NavigationButtons.Buttons.Add(CreateButtonDescriptor(DinaIcon.Locale, "Nav_Localization", ProjectView.Localization));
            NavigationButtons.Buttons.Add(CreateButtonDescriptor(DinaIcon.Font, "Nav_Fonts", ProjectView.Fonts));
            NavigationButtons.Buttons.Add(CreateButtonDescriptor(DinaIcon.Photo, "Nav_Images", ProjectView.Images));
            NavigationButtons.Buttons.Add(CreateButtonDescriptor(DinaIcon.Volume, "Nav_Audio", ProjectView.Audio));
            NavigationButtons.Buttons.Add(CreateButtonDescriptor(DinaIcon.Color, "Nav_Colors", ProjectView.Colors));
            NavigationButtons.Buttons.Add(CreateButtonDescriptor(DinaIcon.Game, "Nav_Inputs", ProjectView.Inputs));
            NavigationButtons.Buttons.Add(CreateButtonDescriptor(DinaIcon.Setting, "Nav_ProjectDefaultSettings", ProjectView.ProjectDefaultSettings));
        }
        private ButtonDescriptor CreateButtonDescriptor(DinaIcon icon, string localizationKey, ProjectView view)
        {
            return new ButtonDescriptor
            {
                Icon = icon.ToGlyph(),
                Label = LocalizationManager.GetTranslation(localizationKey),
                IconPosition = IconPosition.Right,
                LabelHorizontalAlignment = ControlHorizontalAlignment.Left,
                ContentHorizontalAlignment = ControlHorizontalAlignment.Stretch,
                Command = new RelayCommand(_ => EditorRequested?.Invoke(this, view))
            };
        }
    }
}
