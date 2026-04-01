using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public abstract class ComponentPropertiesViewModel : ObservableObject
    {
        protected readonly ComponentModel _component;
        private ComponentModel? _snapshot;

        protected ComponentPropertiesViewModel(ComponentModel component)
        {
            _component = component;
            _key = component.Key;

            ApplyCommand = new RelayCommand(_ => Apply(), _ => HasChanges && IsValid);
            CancelCommand = new RelayCommand(_ => Cancel(), _ => HasChanges);

            FooterButtons.Buttons.Add(new ButtonDescriptor
            {
                Label = LocalizationManager.GetTranslation("Dialog_Cancel"),
                Command = CancelCommand,
                Role = ButtonRole.Secondary
            });
            FooterButtons.Buttons.Add(new ButtonDescriptor
            {
                Label = LocalizationManager.GetTranslation("Dialog_Apply"),
                Command = ApplyCommand,
                Role = ButtonRole.Primary
            });

            TakeSnapshot();
        }

        protected bool HasChanges { get; set; }
        protected void NotifyChange(bool hasChanges = true)
        {
            HasChanges = hasChanges;
            ApplyCommand.RaiseCanExecuteChanged();
            CancelCommand.RaiseCanExecuteChanged();
        }
        public ComponentModel Component => _component;

        public void TakeSnapshot()
        {
            _snapshot = JsonHelper.Deserialize<ComponentModel>(JsonHelper.Serialize(_component));
            NotifyChange(false);
        }

        public void RestoreSnapshot()
        {
            if (_snapshot == null)
                return;
            _component.Key = _snapshot.Key;
            _component.Properties = new Dictionary<string, object>(_snapshot.Properties);
            _key = _snapshot.Key;
            OnPropertyChanged(nameof(Key));
            LoadFrom(_component);
            NotifyChange(false);
        }

        public abstract bool IsValid { get; }
        protected abstract void LoadFrom(ComponentModel source);

        private string _key = string.Empty;
        private string _keyErrorMessage = string.Empty;
        private bool _isKeyFocused;

        public string Key
        {
            get => _key;
            set
            {
                SetProperty(ref _key, value);
                NotifyChange();
            }
        }

        public string KeyErrorMessage
        {
            get => _keyErrorMessage;
            private set => SetProperty(ref _keyErrorMessage, value);
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

        public ButtonBarViewModel FooterButtons { get; } = new ButtonBarViewModel();

        private void ValidateKey()
        {
            KeyErrorMessage = string.IsNullOrEmpty(_key)
                ? LocalizationManager.GetTranslation("Key_Required")
                : string.Empty;
        }

        public RelayCommand ApplyCommand { get; }
        public RelayCommand CancelCommand { get; }



        public event EventHandler<ComponentModel>? Applied;
        public event EventHandler? Cancelled;

        private void Apply()
        {
            var oldSnapshot = _snapshot;         // ancienne version
            ApplyToModel();                      // met à jour _component avec les nouvelles valeurs
            Applied?.Invoke(this, oldSnapshot!); // transmet l'ancien état
            TakeSnapshot();
        }

        private void Cancel()
        {
            RestoreSnapshot();
            Cancelled?.Invoke(this, EventArgs.Empty);
        }
        public abstract void ApplyToModel();
    }
}