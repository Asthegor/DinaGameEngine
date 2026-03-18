using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;

using Microsoft.Win32;

using System.Windows;

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
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInfo(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public MessageResult ShowMessageDialog(string title, string message)
        {
            var messageBoxResult = MessageBox.Show(message, title, MessageBoxButton.YesNoCancel);
            return messageBoxResult switch
            {
                MessageBoxResult.Yes => MessageResult.Yes,
                MessageBoxResult.No => MessageResult.No,
                _ => MessageResult.Cancel
            };
        }
    }
}