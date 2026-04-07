using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class SceneCardViewModel : ItemViewModel
    {
        private SceneModel _scene => (SceneModel)Model;
        public SceneCardViewModel(SceneModel sceneModel) : base(sceneModel)
        {
            SetAsStartupCommand = new RelayCommand(execute:     _ => StartupChangeRequested?.Invoke(this, EventArgs.Empty),
                                                   canExecute:  _ => !IsStartup);
        }
        public override string Icon => DinaIcon.LookupEntities.ToGlyph();
        public override string Name => _scene.Name;
        public override string Key => _scene.Key;
        public bool IsStartup
        {
            get => _scene.IsStartup;
            set
            {
                if (_scene.IsStartup == value)
                    return;
                _scene.IsStartup = value;
                OnPropertyChanged();
            }
        }

        public int ComponentsCount => _scene.Components.Count;
        public RelayCommand SetAsStartupCommand { get; }
        public event EventHandler? StartupChangeRequested;

    }
}
