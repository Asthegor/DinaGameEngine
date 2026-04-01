using DinaGameEngine.Abstractions;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Views;

using Microsoft.Win32;

namespace DinaGameEngine.WPFServices
{
    public class DialogService : IDialogService
    {
        public string? OpenFileDialog(string title, string filter)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = filter,
                Multiselect = false,
                Title = title
            };
            return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
        }

        public string? OpenFolderDialog(string title)
        {
            var openFolderDialog = new OpenFolderDialog
            {
                Title = title,
                Multiselect = false
            };
            return openFolderDialog.ShowDialog() == true ? openFolderDialog.FolderName : null;
        }

        public void ShowError(string title, string message)
        {
            DialogWindow.Show(message, title, DialogIcon.Error, DialogButtons.OK);
        }

        public void ShowInfo(string title, string message)
        {
            DialogWindow.Show(message, title, DialogIcon.Info, DialogButtons.OK);
        }

        public MessageResult ShowMessageDialog(string title, string message)
        {
            var dialogResult = DialogWindow.Show(message, title, DialogIcon.None, DialogButtons.YesNo);
            return dialogResult switch
            {
                DialogResult.Yes => MessageResult.Yes,
                DialogResult.No => MessageResult.No,
                _ => MessageResult.Cancel
            };
        }

        public void ShowWarning(string title, string message)
        {
            DialogWindow.Show(message, title, DialogIcon.Warning, DialogButtons.OK);
        }
    }
}