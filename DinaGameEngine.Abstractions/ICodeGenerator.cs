using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.Abstractions
{
    public interface ICodeGenerator
    {
        void GenerateAllFiles(GameProjectModel gameProjectModel);
        void GenerateAllDesigners(GameProjectModel gameProjectModel);
        void GenerateNewScene(GameProjectModel gameProjectModel, SceneModel scene);
        void RemoveScene(GameProjectModel gameProjectModel, SceneModel scene);
        void GeneratePaletteColor(GameProjectModel gameProjectModel);
        void AddColor(GameProjectModel gameProjectModel, ColorModel colorModel);
        void RemoveColor(GameProjectModel gameProjectModel, ColorModel colorModel);
    }
}
