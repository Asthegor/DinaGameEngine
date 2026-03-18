using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;

namespace DinaGameEngine.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;
        private readonly IFileService _fileService;
        private readonly ILogService _logService;
        private readonly ITemplateExtractor _templateExtractor;

        public MainViewModel(IProjectService projectService, IDialogService dialogService, IFileService fileService, ILogService logService, ITemplateExtractor templateExtractor)
        {
            _projectService = projectService;
            _dialogService = dialogService;
            _fileService = fileService;
            _logService = logService;
            _templateExtractor = templateExtractor;

            LoadScenes();
        }

        private void LoadScenes()
        {

        }
    }
}
