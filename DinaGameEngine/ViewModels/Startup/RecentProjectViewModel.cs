using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Startup;

namespace DinaGameEngine.ViewModels.Startup
{
    public class RecentProjectViewModel : ObservableObject
    {
        private readonly RecentProjectModel _model;
        private readonly IFileService _fileService;
        private readonly IProjectService _projectService;
        private readonly IDialogService _dialogService;
        private bool _isSelected;

        public RecentProjectViewModel(RecentProjectModel model, IFileService fileService, IProjectService projectService, IDialogService dialogService)
        {
            _model = model;
            _fileService = fileService;
            _projectService = projectService;
            _dialogService = dialogService;

            PinProjectCommand = new RelayCommand(ExecutePinProject);
            OpenProjectCommand = new RelayCommand(ExecuteOpenProject);
            RemoveFromListCommand = new RelayCommand(ExecuteRemoveProjectFromList);
        }

        public event EventHandler? PinChanged;
        public event EventHandler<ProjectOpenedEventArgs>? ProjectOpened;
        public event EventHandler? ProjectRemoved;

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
        public string SolutionFolderPath
        {
            get => _model.SolutionFolderPath;
            set
            {
                if (_model.SolutionFolderPath == value)
                    return;
                _model.SolutionFolderPath = value;
                OnPropertyChanged();
            }
        }
        public DateTime LastOpenedAt
        {
            get => _model.LastOpenedAt;
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

        private void ExecutePinProject()
        {
            IsPinned = !IsPinned;
            PinChanged?.Invoke(this, EventArgs.Empty);
        }
        public RelayCommand OpenProjectCommand { get; }
        private void ExecuteOpenProject()
        {
            var project = _projectService.OpenProject(_model.SolutionFolderPath);
            if (project == null)
            {
                _dialogService.ShowError(LocalizationManager.GetTranslation("Dialog_OpenProject"),
                                         LocalizationManager.GetTranslation("Error_OpenProject", _model.Name));
                return;
            }
            ProjectOpened?.Invoke(this, new ProjectOpenedEventArgs(project));
        }
        public RelayCommand RemoveFromListCommand { get; }
        private void ExecuteRemoveProjectFromList()
        {
            ProjectRemoved?.Invoke(this, EventArgs.Empty);
        }
        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}