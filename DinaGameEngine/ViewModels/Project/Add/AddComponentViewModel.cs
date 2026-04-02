using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.ViewModels.Shared;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddComponentViewModel : ObservableObject
    {
        private string _key = string.Empty;
        private string _keyErrorMessage = string.Empty;
        private bool _isKeyFocused;
        private readonly IEnumerable<string> _existingKeys;

        public AddComponentViewModel(IEnumerable<string> existingKeys, string titleKey)
        {
            _existingKeys = existingKeys;

            ConfirmCommand = new RelayCommand(
                execute: _ => Confirm(true),
                canExecute: _ => CanConfirm());
            CancelCommand = new RelayCommand(_ => Confirm(false));

            FooterButtons = new ButtonBarViewModel();
            CreateButtons();

            WindowTitle = LocalizationManager.GetTranslation(titleKey);
        }

        public string Key
        {
            get => _key;
            set
            {
                SetProperty(ref _key, value);
                if (!IsKeyFocused)
                    ValidateKey();
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeyErrorMessage
        {
            get => _keyErrorMessage;
            set
            {
                SetProperty(ref _keyErrorMessage, value);
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsKeyFocused
        {
            get => _isKeyFocused;
            set
            {
                SetProperty(ref _isKeyFocused, value);
                if (!value)
                    ValidateKey();
            }
        }

        public string WindowTitle { get; }
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }
        public ButtonBarViewModel FooterButtons { get; }

        public object? SpecificProperties { get; set; }

        public event EventHandler<bool>? ComponentConfirmed;

        private Func<bool>? _specificPropertiesValidator;
        public void RegisterValidator(Func<bool> validator)
        {
            _specificPropertiesValidator = validator;
        }
        protected virtual bool CanConfirm()
        {
            return !string.IsNullOrEmpty(Key)
                 && string.IsNullOrEmpty(KeyErrorMessage)
                 && (_specificPropertiesValidator == null || _specificPropertiesValidator());
        }
        private void Confirm(bool result)
        {
            ComponentConfirmed?.Invoke(this, result);
        }

        private void ValidateKey()
        {
            KeyErrorMessage = _existingKeys.Contains(Key)
                ? LocalizationManager.GetTranslation("Key_AlreadyExist")
                : string.Empty;
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        private void CreateButtons()
        {
            FooterButtons.Buttons.Clear();
            FooterButtons.Buttons.Add(new ButtonDescriptor
            {
                Label = LocalizationManager.GetTranslation("Dialog_Cancel"),
                Command = CancelCommand,
                Role = ButtonRole.Secondary
            });
            FooterButtons.Buttons.Add(new ButtonDescriptor
            {
                Label = LocalizationManager.GetTranslation("Dialog_OK"),
                Command = ConfirmCommand,
                Role = ButtonRole.Primary
            });
        }

    }
}