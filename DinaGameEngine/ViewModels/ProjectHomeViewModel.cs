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
            {
                var sceneCardViewModel = new SceneCardViewModel(scene);

                sceneCardViewModel.SceneOpened += SceneCardViewModel_SceneOpened;
                sceneCardViewModel.SceneDeleted += SceneCardViewModel_SceneDeleted;

                Scenes.Add(sceneCardViewModel);
            }

        }
        public event EventHandler? SceneOpenRequested;
        public event EventHandler? SceneDeleteRequested;

        private void SceneCardViewModel_SceneDeleted(object? sender, EventArgs e)
        {
            SceneDeleteRequested?.Invoke(sender, e);
        }

        private void SceneCardViewModel_SceneOpened(object? sender, EventArgs e)
        {
            SceneOpenRequested?.Invoke(sender, e);
        }

        public ObservableCollection<SceneCardViewModel> Scenes { get; } = [];

        public void AddScene(SceneModel sceneModel)
        {
            var sceneCardViewModel = new SceneCardViewModel(sceneModel);
            sceneCardViewModel.SceneOpened += SceneCardViewModel_SceneOpened;
            sceneCardViewModel.SceneDeleted += SceneCardViewModel_SceneDeleted;
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
