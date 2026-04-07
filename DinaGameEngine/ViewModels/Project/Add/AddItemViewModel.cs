using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.ViewModels.Shared;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public abstract class AddItemViewModel<T> : ObservableObject
    {
        private string _key = string.Empty;
        private string _keyErrorMessage = string.Empty;
        private bool _isKeyFocused;
        private NamedItem<T>? _selectedNamedItem;
        private readonly IEnumerable<string> _existingKeys;
        protected bool _isUpdatingFromNamedItem;

        protected AddItemViewModel(IEnumerable<string> existingKeys, string titleBaseKey, bool isEditMode)
        {
            _existingKeys = existingKeys;

            AddCommand = new RelayCommand(
                execute: _ => ConfirmItem(true),
                canExecute: _ => CanConfirm());
            CancelCommand = new RelayCommand(_ => ConfirmItem(false));

            FooterButtons = new ButtonBarViewModel();
            CreateButtons();

            WindowTitle = LocalizationManager.GetTranslation(isEditMode ? titleBaseKey + "_Edit" : titleBaseKey);
        }

        public string Key
        {
            get => _key;
            set
            {
                SetProperty(ref _key, value);
                ValidateKey();
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public string KeyErrorMessage
        {
            get => _keyErrorMessage;
            set
            {
                SetProperty(ref _keyErrorMessage, value);
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsKeyFocused
        {
            get => _isKeyFocused;
            set => SetProperty(ref _isKeyFocused, value);
        }

        public IEnumerable<NamedItem<T>> NamedItems { get; protected set; } = [];

        public NamedItem<T>? SelectedNamedItem
        {
            get => _selectedNamedItem;
            set
            {
                _selectedNamedItem = value;
                OnPropertyChanged();
                _isUpdatingFromNamedItem = true;
                OnSelectedNamedItemChanged(value);
                _isUpdatingFromNamedItem = false;
            }
        }

        protected virtual void OnSelectedNamedItemChanged(NamedItem<T>? item) { }

        public string WindowTitle { get; }
        public RelayCommand AddCommand { get; }
        public RelayCommand CancelCommand { get; }
        public ButtonBarViewModel FooterButtons { get; }

        public event EventHandler<bool>? ItemConfirmed;

        protected virtual bool CanConfirm()
            => !string.IsNullOrEmpty(Key) && string.IsNullOrEmpty(KeyErrorMessage);

        private void ConfirmItem(bool result)
        {
            ItemConfirmed?.Invoke(this, result);
        }

        private void ValidateKey()
        {
            KeyErrorMessage = _existingKeys.Contains(Key)
                ? LocalizationManager.GetTranslation("Key_AlreadyExist")
                : string.Empty;

            AddCommand.RaiseCanExecuteChanged();
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
                Label = LocalizationManager.GetTranslation("Dialog_Add"),
                Command = AddCommand,
                Role = ButtonRole.Primary
            });
        }
    }
}