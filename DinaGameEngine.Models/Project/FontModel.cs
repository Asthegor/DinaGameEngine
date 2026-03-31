using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.Models.Project
{
    public class FontModel : ItemModel
    {
        public string Key { get; set; } = string.Empty;
        public string TtfRelativePath { get; set; } = string.Empty;
        public float Size { get; set; }
        public SpriteFontStyle Style { get; set; }
        public float Spacing { get; set; }
    }
}
