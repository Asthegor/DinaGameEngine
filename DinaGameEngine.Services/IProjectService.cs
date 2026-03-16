using DinaGameEngine.Models;

namespace DinaGameEngine.Services
{
    public interface IProjectService
    {
        GameProjectModel? OpenProject(string projectPath);
        GameProjectModel? CreateProject(NewProjectModel newProjectModel);
    }
}