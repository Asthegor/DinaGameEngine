using System.Text.Json.Serialization;

namespace DinaGameEngine.Models.Startup
{
    public class RecentProjectModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SolutionFolderPath { get; set; } = string.Empty;
        public string ProjectFolderPath { get; set; } = string.Empty;
        public DateTime LastOpenedAt { get; set; } = DateTime.Now;
        public bool IsPinned { get; set; }
        public int PinOrder { get; set; }
        public string IconPath { get; set; } = string.Empty;
    }
}