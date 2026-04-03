namespace DinaGameEngine.Models.Project
{
    internal class MenuTitleModel : ItemModel
    {
        private static int _nbMenuTitle = 0;
        private int _titleIndex;
        public MenuTitleModel()
        {
            _titleIndex = _nbMenuTitle++;
            Key = $"menuTitle{_titleIndex}";
        }
        public string Key { get; set; }
        public string Font { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
