using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;

namespace DinaGameEngine.ViewModels
{
    public class WindowMenuItemViewModel : ObservableObject
    {
        private bool _isActive;
        private bool _isClosable;
        private readonly Action<object> _activateAction;
        private readonly Action<object> _closeAction;

        public WindowMenuItemViewModel(string title, object viewModel, Action<object> activateAction, Action<object> closeAction, bool isClosable)
        {
            Title = title;
            ViewModel = viewModel;
            _activateAction = activateAction;
            _closeAction = closeAction;
            _isActive = false;
            _isClosable = isClosable;
            ActivateCommand = new RelayCommand(_ => _activateAction?.Invoke(ViewModel));
            CloseCommand = new RelayCommand(_ => _closeAction?.Invoke(ViewModel));
        }

        public string Title { get; set; } = string.Empty;
        public object? ViewModel { get; set; }
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
        public RelayCommand ActivateCommand { get; }
        public bool IsClosable
        {
            get => _isClosable;
            set => SetProperty(ref _isClosable, value);
        }
        public RelayCommand CloseCommand { get; }
        public string Icon => IsClosable ? DinaIcon.Close.ToGlyph() : string.Empty;
    }
}
