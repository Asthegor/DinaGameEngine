using DinaGameEngine.Common;

namespace DinaGameEngine.Models
{
    public class GameProjectModel : ObservableObject
    {
        private string _solutionName = string.Empty;
        private string _projectName = string.Empty;
        private string _dinaVersion = "1.0.0";
        private DateTime _createdAt;
        private DateTime _lastOpenedAt;
        private string _rootPath = string.Empty;
        private string _rootNamespace = string.Empty;
        private string _defaultLanguage = string.Empty;

        public Guid Id { get; set; }
        public string SolutionName
        {
            get => _solutionName;
            set => SetProperty(ref _solutionName, value);
        }
        public string ProjectName
        {
            get => _projectName;
            set => SetProperty(ref _projectName, value);
        }
        public string RootNamespace
        {
            get => _rootNamespace;
            set => SetProperty(ref _rootNamespace, value);
        }

        // Dossier racine du projet de jeu
        public string RootPath
        {
            get => _rootPath;
            set => SetProperty(ref _rootPath, value);
        }

        public string DinaVersion
        {
            get => _dinaVersion;
            set => SetProperty(ref _dinaVersion, value);
        }

        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        public DateTime LastOpenedAt
        {
            get => _lastOpenedAt;
            set => SetProperty(ref _lastOpenedAt, value);
        }
        public string DefaultLanguage
        {
            get => _defaultLanguage;
            set => SetProperty(ref _defaultLanguage, value);
        }
        public HashSet<string> Languages { get; set; } = [];
        public HashSet<SceneModel> Scenes { get; set; } = [];
    }
}