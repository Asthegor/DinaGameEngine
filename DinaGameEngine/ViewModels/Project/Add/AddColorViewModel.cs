using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Project;

using System.Windows.Media;


namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddColorViewModel : ObservableObject
    {
        private string _key = string.Empty;
        private byte _r;
        private byte _g;
        private byte _b;
        private byte _a;
        private Color? _selectedColor;
        private NamedColorItem? _selectedNamedColor;
        private bool _isUpdatingFromNamedColor;

        private IEnumerable<string> _existingKeys = [];
        private string _keyErrorMessage = string.Empty;
        private bool _isKeyFocused;

        public AddColorViewModel(IEnumerable<string> existingKeys, ColorModel? colorModel = null)
        {
            NamedColors = [.. typeof(Colors).GetProperties().Select(color => new NamedColorItem { Name = color.Name, Color = (Color)color.GetValue(null)! })];

            AddCommand = new RelayCommand(execute: _ => ConfirmedColor(true),
                                 canExecute: _ => !string.IsNullOrEmpty(Key) && string.IsNullOrEmpty(KeyErrorMessage));
            CancelCommand = new RelayCommand(_ => ConfirmedColor(false));

            FooterButtons = new ButtonBarViewModel();
            CreateButtons();

            _existingKeys = existingKeys;

            var titleKey ="AddColor_Title";
            if (colorModel != null)
            {
                Key = colorModel.Key;
                R = colorModel.R;
                G = colorModel.G;
                B = colorModel.B;
                A = colorModel.A;

                SelectedNamedColor = NamedColors.FirstOrDefault(c => c.Color.R == R && c.Color.G == G && c.Color.B == B && c.Color.A == A);

                titleKey += "_Edit";
            }
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
                AddCommand.RaiseCanExecuteChanged();
            }
        }
        public byte R
        {
            get => _r;
            set
            {
                SetProperty(ref _r, value);
                OnPropertyChanged(nameof(PreviewColor));
                if (!_isUpdatingFromNamedColor)
                    SelectedNamedColor = null;
            }
        }
        public byte G
        {
            get => _g;
            set
            {
                SetProperty(ref _g, value);
                OnPropertyChanged(nameof(PreviewColor));
                if (!_isUpdatingFromNamedColor)
                    SelectedNamedColor = null;
            }
        }
        public byte B
        {
            get => _b;
            set
            {
                SetProperty(ref _b, value);
                OnPropertyChanged(nameof(PreviewColor));
                if (!_isUpdatingFromNamedColor)
                    SelectedNamedColor = null;
            }
        }
        public byte A
        {
            get => _a;
            set
            {
                SetProperty(ref _a, value);
                OnPropertyChanged(nameof(PreviewColor));
                if (!_isUpdatingFromNamedColor)
                    SelectedNamedColor = null;
            }
        }
        public Color PreviewColor
        {
            get => Color.FromArgb(A, R, G, B);
            set
            {
                SelectedNamedColor = null;
                _isUpdatingFromNamedColor = true;
                R = value.R;
                G = value.G;
                B = value.B;
                A = value.A;
                _isUpdatingFromNamedColor = false;
                OnPropertyChanged();
            }
        }
        public Color? SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (_selectedColor == value)
                    return;
                SetProperty(ref _selectedColor, value);
                if (value != null)
                {
                    R = value.Value.R;
                    G = value.Value.G;
                    B = value.Value.B;
                    A = value.Value.A;
                }
            }
        }
        public NamedColorItem? SelectedNamedColor
        {
            get => _selectedNamedColor;
            set
            {
                _selectedNamedColor = value;
                OnPropertyChanged();
                _isUpdatingFromNamedColor = true;
                SelectedColor = value?.Color;
                _isUpdatingFromNamedColor = false;
            }
        }
        public IEnumerable<NamedColorItem> NamedColors { get; }
        public event EventHandler<bool>? ColorConfirmed;
        public RelayCommand AddCommand { get; }
        public RelayCommand CancelCommand { get; }
        private void ConfirmedColor(bool result)
        {
            ColorConfirmed?.Invoke(this, result);
        }
        public ButtonBarViewModel FooterButtons { get; }
        private void CreateButtons()
        {
            FooterButtons.Buttons.Clear();
            FooterButtons.Buttons.Add(new ButtonDescriptor { Label = LocalizationManager.GetTranslation("Dialog_Cancel"), Command = CancelCommand, Role = ButtonRole.Secondary });
            FooterButtons.Buttons.Add(new ButtonDescriptor { Label = LocalizationManager.GetTranslation("Dialog_Add"), Command = AddCommand, Role = ButtonRole.Primary });
        }

        public string KeyErrorMessage
        {
            get => _keyErrorMessage;
            set => SetProperty(ref _keyErrorMessage, value);
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
        private void ValidateKey()
        {
            if (_existingKeys.Contains(Key))
                KeyErrorMessage = LocalizationManager.GetTranslation("AddColor_Key_AlreadyExist");
            else
                KeyErrorMessage = string.Empty;

            AddCommand.RaiseCanExecuteChanged();
        }
        public string WindowTitle { get; set; }
    }
}
