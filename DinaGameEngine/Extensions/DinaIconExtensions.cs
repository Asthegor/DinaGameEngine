using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.Extensions
{
    public static class DinaIconExtensions
    {
        public static string ToGlyph(this DinaIcon icon)
            => char.ConvertFromUtf32((int)icon);
    }
}
