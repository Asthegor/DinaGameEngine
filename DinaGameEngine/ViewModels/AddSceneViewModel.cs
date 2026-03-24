using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.ViewModels
{
    public class AddSceneViewModel : ObservableObject
    {
        public AddSceneViewModel()
        {
            AddCommand = new RelayCommand(execute: _ => ConfirmedScene(true),
                                             canExecute: _ => !string.IsNullOrEmpty(SceneName));
            CancelCommand = new RelayCommand(_ => ConfirmedScene(false));

            FooterButtons = new ButtonBarViewModel();
            UpdateFooterButtons();

        }
        private string _sceneName = string.Empty;
        public string SceneName
        {
            get => _sceneName;
            set
            {
                SetProperty(ref _sceneName, value);
                OnPropertyChanged(nameof(ClassName));
                OnPropertyChanged(nameof(Key));
                AddCommand.RaiseCanExecuteChanged();
            }
        }
        public string ClassName => $"{SceneName.ToPascalCase()}Scene";
        public string Key => SceneName.ToPascalCase();

        public event EventHandler<bool>? SceneConfirmed;
        public RelayCommand AddCommand { get; }
        public RelayCommand CancelCommand { get; }
        private void ConfirmedScene(bool result)
        {
            SceneConfirmed?.Invoke(this, result);
        }

        public ButtonBarViewModel FooterButtons { get; }
        private void UpdateFooterButtons()
        {
            FooterButtons.Buttons.Clear();
            FooterButtons.Buttons.Add(new ButtonDescriptor { Label = LocalizationManager.GetTranslation("Dialog_Cancel"), Command = CancelCommand, Role = ButtonRole.Secondary });
            FooterButtons.Buttons.Add(new ButtonDescriptor { Label = LocalizationManager.GetTranslation("Dialog_Add"), Command = AddCommand, Role = ButtonRole.Primary });
        }

    }
}
