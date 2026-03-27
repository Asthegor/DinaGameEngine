using DinaGameEngine.Common;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels
{
    public class SceneEditorViewModel : ObservableObject
    {
        private SceneModel _sceneModel;
        public SceneEditorViewModel(SceneModel sceneModel)
        {
            _sceneModel = sceneModel;
        }
        public Guid SceneId => _sceneModel.Id;
    }
}
