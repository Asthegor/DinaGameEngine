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
        void GeneratePaletteColors(GameProjectModel gameProjectModel);
        void AddColor(GameProjectModel gameProjectModel, ColorModel colorModel);
        void RemoveColor(GameProjectModel gameProjectModel, ColorModel colorModel);
        void GenerateFontKeys(GameProjectModel gameProjectModel);
        void AddFont(GameProjectModel gameProjectModel, FontModel existingFontModel);
        void RemoveFont(GameProjectModel gameProjectModel, FontModel fontModel);
        void AddComponent(GameProjectModel gameProjectModel, SceneModel sceneModel, ComponentModel component);
        void RemoveComponent(GameProjectModel gameProjectModel, SceneModel sceneModel, ComponentModel component);

    }
}
