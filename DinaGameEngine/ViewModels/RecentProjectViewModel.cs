using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.ViewModels
{
    public class RecentProjectViewModel : ObservableObject
    {
        private readonly RecentProjectModel _model;
        private readonly IFileService _fileService;
        private readonly IProjectService _projectService;

        public RecentProjectViewModel(RecentProjectModel model, IFileService fileService, IProjectService projectService)
        {
            _model = model;
            _fileService = fileService;
            _projectService = projectService;

            PinProjectCommand = new RelayCommand(ExecutePinProject);
            OpenRecentProjectCommand = new RelayCommand(ExecuteOpenProject);
        }

        public event EventHandler? PinChanged;
        public event EventHandler<ProjectOpenedEventArgs>? ProjectOpened;

        public string Name
        {
            get => _model.Name;
            set
            {
                if (_model.Name == value)
                    return;
                _model.Name = value;
                OnPropertyChanged();
            }
        }
        public string ProjectFolderPath
        {
            get => _model.ProjectFolderPath;
            set
            {
                if (_model.ProjectFolderPath == value)
                    return;
                _model.ProjectFolderPath = value;
                OnPropertyChanged();
            }
        }
        public DateTime LastOpenedAt
        {
            get
        => _model.LastOpenedAt;
            set
            {
                if (_model.LastOpenedAt == value)
                    return;
                _model.LastOpenedAt = value;
                OnPropertyChanged();
            }
        }
        public int PinOrder
        {
            get => _model.PinOrder;
            set
            {
                if (_model.PinOrder == value)
                    return;
                _model.PinOrder = value;
                OnPropertyChanged();
            }
        }
        public bool IsPinned
        {
            get => _model.IsPinned;
            private set
            {
                if (_model.IsPinned == value)
                    return;
                _model.IsPinned = value;
                OnPropertyChanged();
            }
        }
        public string IconPath => _fileService.Combine(ProjectFolderPath, ProjectStructure.IconFileName);


        public RelayCommand PinProjectCommand { get; }
        public RelayCommand OpenRecentProjectCommand { get; }

        private void ExecutePinProject()
        {
            IsPinned = !IsPinned;
            PinChanged?.Invoke(this, EventArgs.Empty);
        }
        private void ExecuteOpenProject()
        {
            var project = _projectService.OpenProject(_model.ProjectFolderPath);
            if (project == null)
                return;
            ProjectOpened?.Invoke(this, new ProjectOpenedEventArgs(project));
        }
    }
}