using DinaGameEngine.Models;

namespace DinaGameEngine.Abstractions
{
    public interface ICodeGenerator
    {
        void GenerateAllFiles(GameProjectModel gameProjectModel);
        void GenerateAllDesigners(GameProjectModel gameProjectModel);
        void GenerateNewScene(GameProjectModel gameProjectModel, SceneModel sceneName);
        void AddAllComponents(GameProjectModel gameProjectModel);
    }
}
