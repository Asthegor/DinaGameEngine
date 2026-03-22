using DinaGameEngine.Models;

namespace DinaGameEngine.Abstractions
{
    public interface IProjectService
    {
        public GameProjectModel? OpenProject(string projectPath);
        public Task<GameProjectModel?> CreateProject(NewProjectModel newProjectModel, List<TemplateMarkerModel> markers);

        public void UpdateJsonProjectFile(GameProjectModel gameModelProject);
    }
}