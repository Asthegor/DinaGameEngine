using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.Abstractions
{
    public interface ICodeGenerator
    {
        void GenerateAllFiles(GameProjectModel gameProjectModel);
        void GenerateAllDesigners(GameProjectModel gameProjectModel);
        void GenerateNewScene(GameProjectModel gameProjectModel, SceneModel sceneName);
        void AddAllComponents(GameProjectModel gameProjectModel);
        void RemoveScene(GameProjectModel gameProjectModel, SceneModel scene);
    }
}
