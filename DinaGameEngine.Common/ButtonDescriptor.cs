using DinaGameEngine.Common.Enums;

using System.Windows.Input;

namespace DinaGameEngine.Common
{
    public class ButtonDescriptor
    {
        public string Label { get; set; } = string.Empty;
        public ICommand? Command { get; set; }
        public ButtonRole Role { get; set; } = ButtonRole.Neutral;
        public string? Icon { get; set; }
    }
}
