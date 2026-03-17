using DinaGameEngine.Common;

using System.Collections.ObjectModel;

namespace DinaGameEngine.Models
{
    public class RecentProjectGroupModel
    {
        public string SectionName { get; set; } = string.Empty;
        public bool IsExpanded { get; set; }
        public ObservableCollection<RecentProjectModel> Projects { get; set; } = [];
    }
}
