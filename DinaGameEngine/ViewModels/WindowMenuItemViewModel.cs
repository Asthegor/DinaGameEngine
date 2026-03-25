using DinaGameEngine.Commands;
using DinaGameEngine.Common;

using System.Net.Http.Headers;

namespace DinaGameEngine.ViewModels
{
    public class WindowMenuItemViewModel : ObservableObject
    {
        private bool _isActive;
        private Action<object> _activateAction;

        public WindowMenuItemViewModel(string title, object viewModel, Action<object> activateAction)
        {
            Title = title;
            ViewModel = viewModel;
            _activateAction = activateAction;
            _isActive = true;
            ActivateCommand = new RelayCommand(_ => _activateAction?.Invoke(ViewModel));
        }

        public string Title { get; set; } = string.Empty;
        public object? ViewModel { get; set; }
        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }
        public RelayCommand ActivateCommand { get; }
    }
}
