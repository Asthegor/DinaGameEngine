namespace DinaGameEngine.Models
{
    public class RecentProjectModel
    {
        public string Name { get; set; } = string.Empty;
        public string ProjectFolderPath { get; set; } = string.Empty;
        public DateTime LastOpenedAt { get; set; } = DateTime.Now;
        public bool IsPinned { get; set; }
        public int PinOrder { get; set; }
    }
}