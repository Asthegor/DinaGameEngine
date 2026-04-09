using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class SceneCardViewModel : ItemViewModel
    {
        private SceneModel Scene => (SceneModel)Model;
        public SceneCardViewModel(SceneModel sceneModel) : base(sceneModel)
        {
            SetAsStartupCommand = new RelayCommand(execute:     _ => StartupChangeRequested?.Invoke(this, EventArgs.Empty),
                                                   canExecute:  _ => !IsStartup);
        }
        public override string Icon => DinaIcon.LookupEntities.ToGlyph();
        public override string Name => Scene.Name;
        public override string Key => Scene.Key;
        public bool IsStartup
        {
            get => Scene.IsStartup;
            set
            {
                if (Scene.IsStartup == value)
                    return;
                Scene.IsStartup = value;
                OnPropertyChanged();
            }
        }

        public int ComponentsCount => Scene.Components.Count;
        public RelayCommand SetAsStartupCommand { get; }
        public event EventHandler? StartupChangeRequested;

    }
}
