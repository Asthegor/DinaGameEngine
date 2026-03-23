using DinaGameEngine.Commands;
using DinaGameEngine.Common;

namespace DinaGameEngine.ViewModels
{
    public class DialogViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DialogIcon Icon { get; set; } = DialogIcon.None;
        public DialogButtons Buttons { get; set; } = DialogButtons.OK;
        public DialogResult Result { get; set; } = DialogResult.None;
        public RelayCommand ConfirmCommand { get; }
        public RelayCommand CancelCommand { get; }
        public RelayCommand YesCommand { get; }
        public RelayCommand NoCommand { get; }
        public RelayCommand CloseCommand { get; }

        public DialogViewModel()
        {
            ConfirmCommand = new RelayCommand(_ =>
            {
                Result = DialogResult.OK;
                CloseAction?.Invoke();
            });
            CancelCommand = new RelayCommand(_ =>
            {
                Result = DialogResult.Cancel;
                CloseAction?.Invoke();
            });
            YesCommand = new RelayCommand(_ =>
            {
                Result = DialogResult.Yes;
                CloseAction?.Invoke();
            });
            NoCommand = new RelayCommand(_ =>
            {
                Result = DialogResult.No;
                CloseAction?.Invoke();
            });
            CloseCommand = new RelayCommand(_ =>
            {
                Result = DialogResult.None;
                CloseAction?.Invoke();
            });
        }
        public Action? CloseAction { get; set; }
    }
}
