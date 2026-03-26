using DinaGameEngine.Common;
using DinaGameEngine.Models.Startup;

using System.Collections.ObjectModel;

namespace DinaGameEngine.ViewModels
{
    public class RecentProjectGroupViewModel(RecentProjectGroupModel model) : ObservableObject
    {
        private readonly RecentProjectGroupModel _model = model;

        public string SectionName
        {
            get => _model.SectionName;
            set
            {
                if (_model.SectionName == value)
                    return;
                _model.SectionName = value;
                OnPropertyChanged();
            }
        }
        public bool IsExpanded
        {
            get => _model.IsExpanded;
            set
            {
                if (_model.IsExpanded == value)
                    return;
                _model.IsExpanded = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<RecentProjectViewModel> Projects { get; } = [];
    }
}
