using DinaGameEngine.Common.Enums;

using System.Windows.Media;

namespace DinaGameEngine.Extensions
{
    public static class DinaColorExtensions
    {
        public static SolidColorBrush ToBrush(this DinaColor color)
        {
            SolidColorBrush brush = color switch
            {
                // Barre de titre / fenêtre
                DinaColor.WindowBorder => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.TitleBarBackground => new SolidColorBrush(Color.FromRgb(0xF8, 0xF8, 0xF8)),
                DinaColor.TitleBarForeground => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.TitleBarButtonIcon => new SolidColorBrush(Color.FromRgb(0x55, 0x55, 0x55)),
                DinaColor.TitleBarButtonHover => new SolidColorBrush(Color.FromRgb(0xE5, 0xE5, 0xE5)),
                DinaColor.CloseButtonHoverBackground => new SolidColorBrush(Color.FromRgb(0xE8, 0x11, 0x23)),
                DinaColor.CloseButtonHoverForeground => new SolidColorBrush(Colors.White),
                DinaColor.WindowInactiveBackground => new SolidColorBrush(Color.FromRgb(0xFF, 0xE0, 0xB2)),
                DinaColor.WindowInactiveForeground => new SolidColorBrush(Color.FromRgb(0xE6, 0x51, 0x00)),
                DinaColor.WindowInactiveBorder => new SolidColorBrush(Color.FromRgb(0xFF, 0x6D, 0x00)),

                // Cartes
                DinaColor.CardBackground => new SolidColorBrush(Colors.White),
                DinaColor.CardHoverBackground => new SolidColorBrush(Color.FromRgb(0xF9, 0xF9, 0xF9)),
                DinaColor.CardHoverBorder => new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)),
                DinaColor.CardSelectedBackground => new SolidColorBrush(Color.FromArgb(0x3F, 0xF0, 0xD0, 0xFF)),
                DinaColor.CardSelectedBorder => new SolidColorBrush(Colors.DarkViolet),

                // Boutons
                DinaColor.ButtonPrimaryBackground => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.ButtonPrimaryForeground => new SolidColorBrush(Colors.White),
                DinaColor.ButtonPrimaryHover => new SolidColorBrush(Color.FromRgb(0x53, 0x2E, 0x80)),
                DinaColor.ButtonSecondaryAccent => new SolidColorBrush(Colors.OrangeRed),
                DinaColor.ButtonSecondaryHover => new SolidColorBrush(Color.FromRgb(0xFF, 0xEC, 0xB3)),
                DinaColor.ButtonNeutralAccent => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.ButtonNeutralHover => new SolidColorBrush(Color.FromRgb(0xF3, 0xE5, 0xF5)),
                DinaColor.ButtonDisabledAccent => new SolidColorBrush(Colors.Gray),
                DinaColor.ButtonDisabledHover => new SolidColorBrush(Color.FromRgb(0xF5, 0xF5, 0xF5)),

                // Champs de saisie
                DinaColor.InputBorderDefault => new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)),
                DinaColor.InputBorderHover => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.InputBorderFocus => new SolidColorBrush(Colors.OrangeRed),
                DinaColor.InputBorderFocusReadOnly => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.InputBorderError => new SolidColorBrush(Colors.Red),
                DinaColor.InputDisabledBackground => new SolidColorBrush(Color.FromRgb(0xF9, 0xF9, 0xF9)),
                DinaColor.InputDisabledForeground => new SolidColorBrush(Colors.DimGray),

                // Onglets
                DinaColor.TabInactiveBackground => new SolidColorBrush(Color.FromArgb(0x3F, 0xF0, 0xD0, 0xFF)),
                DinaColor.TabInactiveBorder => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.TabInactiveForeground => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.TabActiveBackground => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.TabActiveForeground => new SolidColorBrush(Colors.White),
                DinaColor.TabCloseHover => new SolidColorBrush(Colors.OrangeRed),

                // Icônes de dialogue
                DinaColor.DialogIconInfo => new SolidColorBrush(Color.FromRgb(0x21, 0x96, 0xF3)),
                DinaColor.DialogIconWarning => new SolidColorBrush(Color.FromRgb(0xFF, 0x98, 0x00)),
                DinaColor.DialogIconError => new SolidColorBrush(Color.FromRgb(0xF4, 0x43, 0x36)),
                DinaColor.DialogIconSuccess => new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)),

                // Divers
                DinaColor.LabelForeground => new SolidColorBrush(Colors.DimGray),
                DinaColor.SubtleForeground => new SolidColorBrush(Color.FromRgb(0xAA, 0xAA, 0xAA)),
                DinaColor.PinIconInactive => new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)),
                DinaColor.PinIconRemoveHover => new SolidColorBrush(Colors.Red),
                DinaColor.PreviewPanelBackground => new SolidColorBrush(Color.FromRgb(0xF9, 0xF9, 0xF9)),
                DinaColor.PreviewPanelBorder => new SolidColorBrush(Color.FromRgb(0xEA, 0xEA, 0xEA)),
                DinaColor.PathPreviewForeground => new SolidColorBrush(Colors.DarkViolet),
                DinaColor.ColorPreviewBorder => new SolidColorBrush(Colors.Black),
                DinaColor.SceneEditorBackground => new SolidColorBrush(Colors.Black),
                DinaColor.SplitterBackground => new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)),

                _ => throw new ArgumentOutOfRangeException(nameof(color), color, null)
            };

            brush.Freeze();
            return brush;
        }
    }
}