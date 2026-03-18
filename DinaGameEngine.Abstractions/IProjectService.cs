using DinaGameEngine.Models;

namespace DinaGameEngine.Abstractions
{
    public interface IProjectService
    {
        GameProjectModel? OpenProject(string projectPath);
        Task<GameProjectModel?> CreateProject(NewProjectModel newProjectModel, List<TemplateMarkerModel> markers);
    }
}