using DinaGameEngine.Models;

using System.Collections.ObjectModel;

namespace DinaGameEngine.ViewModels
{
    public class ProjectHomeViewModel
    {
        private GameProjectModel _gameProjectModel;
        public ProjectHomeViewModel(GameProjectModel gameProjectModel)
        {
            _gameProjectModel = gameProjectModel;
            Scenes.Clear();
            foreach (var scene in _gameProjectModel.Scenes)
                AddScene(scene);

        }
        public event EventHandler? SceneOpenRequested;
        public event EventHandler? SceneDeleteRequested;

        public ObservableCollection<SceneCardViewModel> Scenes { get; } = [];

        public void AddScene(SceneModel sceneModel)
        {
            var sceneCardViewModel = new SceneCardViewModel(sceneModel);
            sceneCardViewModel.SceneOpened += (sender, eventArgs) => SceneOpenRequested?.Invoke(sender, eventArgs);
            sceneCardViewModel.SceneDeleted += (sender, eventArgs) => SceneDeleteRequested?.Invoke(sender, eventArgs);
            Scenes.Add(sceneCardViewModel);
        }
        public void RemoveScene(SceneModel sceneModel)
        {
            var sceneCardViewModel = Scenes.FirstOrDefault(s => s.Name == sceneModel.Name);
            if (sceneCardViewModel != null)
                Scenes.Remove(sceneCardViewModel);
        }
    }
}
