using DinaGameEngine.Models;

namespace DinaGameEngine.Services
{
    public partial class ProjectService
    {
        public void UpdateGameProjectUserFile(GameProjectModel gameProjectModel)
        {
            if (gameProjectModel.Scenes.Count > 0)
                _codeGenerator.UpdateStartupScene(gameProjectModel);
            else
                _codeGenerator.RemoveStartupScene(gameProjectModel);
        }
    }
}
