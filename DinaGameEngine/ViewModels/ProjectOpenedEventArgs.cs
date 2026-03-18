using DinaGameEngine.Models;

namespace DinaGameEngine.ViewModels
{
    public class ProjectOpenedEventArgs(GameProjectModel project) : EventArgs
    {
        public GameProjectModel Project { get; } = project;
    }
}
