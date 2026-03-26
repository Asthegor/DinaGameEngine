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
        public IconPosition IconPosition { get; set; } = IconPosition.Left;
        public bool IsIconLeft => !string.IsNullOrEmpty(Icon) && IconPosition == IconPosition.Left;
        public bool IsIconRight => !string.IsNullOrEmpty(Icon) && IconPosition == IconPosition.Right;
        public ControlVerticalAlignment LeftIconVerticalAlignment { get; set; } = ControlVerticalAlignment.Center;
        public ControlHorizontalAlignment LeftIconHorizontalAlignment { get; set; } = ControlHorizontalAlignment.Left;
        public ControlVerticalAlignment LabelVerticalAlignment { get; set; } = ControlVerticalAlignment.Center;
        public ControlHorizontalAlignment LabelHorizontalAlignment { get; set; } = ControlHorizontalAlignment.Center;
        public ControlVerticalAlignment RightIconVerticalAlignment { get; set; } = ControlVerticalAlignment.Center;
        public ControlHorizontalAlignment RightIconHorizontalAlignment { get; set; } = ControlHorizontalAlignment.Right;
        public ControlHorizontalAlignment ContentHorizontalAlignment { get; set; } = ControlHorizontalAlignment.Center;
        public ControlVerticalAlignment ContentVerticalAlignment { get; set; } = ControlVerticalAlignment.Center;
    }
}
