using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models;
using DinaGameEngine.ViewModels.Project.Editors;
using DinaGameEngine.ViewModels.Project.Items;
using DinaGameEngine.ViewModels.Shared;

using System.Windows.Controls;

namespace DinaGameEngine.ViewModels.Project
{
    public class ProjectHomeViewModel : EditorViewModel<SceneCardViewModel>
    {
        public ProjectHomeViewModel(GameProjectModel gameProjectModel) : base([.. gameProjectModel.Scenes])
        {
            ToggleNavigationCommand = new RelayCommand(_ => NavigationButtons!.IsCollapsed = !NavigationButtons.IsCollapsed);

            NavigationButtons = new ButtonBarViewModel { Orientation = Orientation.Vertical };
            CreateButtons();

            NavigationButtons.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(ButtonBarViewModel.IsCollapsed))
                    OnPropertyChanged(nameof(CollapsedIcon));
            };
            foreach(var viewModel in Items)
                ((SceneCardViewModel)viewModel).StartupChangeRequested += OnStartupChangeRequested;
            Items.CollectionChanged += (s, e) =>
            {
                if (e.NewItems == null)
                    return;
                foreach (SceneCardViewModel vm in e.NewItems.Cast<SceneCardViewModel>())
                    vm.StartupChangeRequested += OnStartupChangeRequested;
            };
        }

        public event EventHandler<ProjectView>? EditorRequested;

        public ButtonBarViewModel NavigationButtons { get; }
        public string CollapsedIcon => (NavigationButtons.IsCollapsed ? DinaIcon.ClosePane : DinaIcon.OpenPane).ToGlyph();
        public RelayCommand ToggleNavigationCommand { get; }
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
                Command = new RelayCommand(_ => EditorRequested?.Invoke(this, view)),
                LabelWeight = TextWeight.Normal,
                IconWeight = TextWeight.Normal,
            };
        }
        public event EventHandler? StartupSceneChangeRequested;
        private void OnStartupChangeRequested(object? sender, EventArgs e)
        {
            if (sender is not SceneCardViewModel newStartup)
                return;

            foreach (SceneCardViewModel vm in Items.Cast<SceneCardViewModel>())
                vm.IsStartup = false;

            newStartup.IsStartup = true;
            StartupSceneChangeRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}
