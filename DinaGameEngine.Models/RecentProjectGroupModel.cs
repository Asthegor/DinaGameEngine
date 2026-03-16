using DinaGameEngine.Common;

using System.Collections.ObjectModel;

namespace DinaGameEngine.Models
{
    public class RecentProjectGroupModel : ObservableObject
    {
        private string _sectionName = string.Empty;
        private bool _isExpanded = true;
        private ObservableCollection<RecentProjectModel> _projects = [];

        public string SectionName
        {
            get => _sectionName;
            set => SetProperty(ref _sectionName, value);
        }
        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }
        public ObservableCollection<RecentProjectModel> Projects
        { 
            get => _projects;
            set => SetProperty(ref _projects, value);
        }
    }
}
