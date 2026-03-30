namespace DinaGameEngine.Models.Project
{
    public class ColorModel : ItemModel
    {
        public string Key { get; set; } = string.Empty;
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
    }
}
