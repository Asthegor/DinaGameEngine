using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.Services
{
    public interface IDialogService
    {
        public string? OpenFileDialog(string title, string filter);
        public string? OpenFolderDialog(string title);
        MessageResult ShowMessageDialog (string title, string message);
        void ShowError(string title, string message);
        NewProjectModel? ShowNewProjectDialog();
    }
}