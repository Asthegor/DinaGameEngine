using DinaGameEngine.Models;

namespace DinaGameEngine.Common
{
    public class ProjectOpenedEventArgs(GameProjectModel project) : EventArgs
    {
        public GameProjectModel Project { get; } = project;
    }
}
