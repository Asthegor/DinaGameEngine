namespace DinaGameEngine.Models.Project
{
    public class MenuItemModel : ItemModel
    {
        private static int _nbMenuItem = 0;
        private int _itemIndex;
        public MenuItemModel()
        {
            _itemIndex = _nbMenuItem++;
            Key = $"menuItem{_itemIndex}";
        }
        public string Key { get; set; }
        public string Font { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string HAlign { get; set; } = string.Empty; // DinaHorizontalAlignment
        public string VAlign { get; set; } = string.Empty; // DinaVerticalAlignment
        public string State { get; set; } = string.Empty; // MenuItemState

    }
}
