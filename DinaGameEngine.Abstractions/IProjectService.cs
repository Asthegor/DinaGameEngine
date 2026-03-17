using DinaGameEngine.Models;

namespace DinaGameEngine.Abstractions
{
    public interface IProjectService
    {
        GameProjectModel? OpenProject(string projectPath);
        GameProjectModel? CreateProject(NewProjectModel newProjectModel, List<TemplateMarkerModel> markers);
    }
}