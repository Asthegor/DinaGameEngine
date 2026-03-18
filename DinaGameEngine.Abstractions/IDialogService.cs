using DinaGameEngine.Common;

namespace DinaGameEngine.Abstractions
{
    public interface IDialogService
    {
        public string? OpenFileDialog(string title, string filter);
        public string? OpenFolderDialog(string title);
        public MessageResult ShowMessageDialog (string title, string message);
        public void ShowError(string title, string message);
        public void ShowInfo(string title, string message);

    }
}