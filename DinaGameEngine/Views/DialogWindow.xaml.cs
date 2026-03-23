using DinaGameEngine.Common;
using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels;

namespace DinaGameEngine.Views
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : DinaWindow
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public static DialogResult Show(string message, string title, DialogIcon icon, DialogButtons buttons = DialogButtons.OK)
        {
            var dialogViewModel = new DialogViewModel
            {
                Message = message,
                Title = title,
                Icon = icon,
                Buttons = buttons
            };
            var dialogWindow = new DialogWindow
            {
                DataContext = dialogViewModel
            };
            dialogViewModel.CloseAction = () => dialogWindow.Close();
            dialogWindow.ShowDialog();
            return dialogViewModel.Result;
        }
    }
}
