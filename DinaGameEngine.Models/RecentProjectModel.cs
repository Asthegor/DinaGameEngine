using DinaGameEngine.Common;

using System.Text.Json.Serialization;

namespace DinaGameEngine.Models
{
    public class RecentProjectModel : ObservableObject
    {
        private string _name = string.Empty;
        private string _projectFolderPath = string.Empty;
        private DateTime _lastOpenedAt;
        private bool _isPinned;
        private int _pinOrder;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string ProjectFolderPath
        {
            get => _projectFolderPath;
            set => SetProperty(ref _projectFolderPath, value);
        }

        public DateTime LastOpenedAt
        {
            get => _lastOpenedAt;
            set => SetProperty(ref _lastOpenedAt, value);
        }

        public bool IsPinned
        {
            get => _isPinned;
            set => SetProperty(ref _isPinned, value);
        }

        public int PinOrder
        {
            get => _pinOrder;
            set => SetProperty(ref _pinOrder, value);
        }

        [JsonIgnore]
        public string IconPath => Path.Combine(_projectFolderPath, ProjectStructure.IconFileName);
    }
}