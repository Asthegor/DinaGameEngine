using DinaGameEngine.Models;

namespace DinaGameEngine.Services
{
    public partial class ProjectService
    {
        private void UpdateGameProjectUserFile(GameProjectModel gameProjectModel)
        {
            _codeGenerator.UpdateStartupScene(gameProjectModel);
        }
    }
}
