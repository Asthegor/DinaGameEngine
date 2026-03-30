using Microsoft.Xna.Framework;

namespace __RootNamespace__.Core.Keys
{
    public class PaletteColors
    {

        // Définit les couleurs utilisées dans l'application
        // Valeur par défaut pour le titre, l'ombre du titre
        // et les items du menu désactivés

        // =[ZONE:PALETTE_COLORS]=
        public static readonly Color Transparent = Color.Transparent;

        #region Menu principal
        public static readonly Color MainMenu_Title = Color.MonoGameOrange;
        public static readonly Color MainMenu_Title_Shadow = Color.DarkGray;
        public static readonly Color MainMenu_MenuItem_Disabled = Color.DarkSlateGray;
        public static readonly Color MainMenu_MenuItem = Color.White;
        public static readonly Color MainMenu_MenuItem_Hovered = Color.Yellow;
        #endregion

        #region Écran des options
        public static readonly Color Options_Title = Color.MonoGameOrange;
        public static readonly Color Options_Title_Shadow = Color.DarkGray;
        public static readonly Color Options_Category = Color.LightGray;
        public static readonly Color Options_Label = Color.White;
        public static readonly Color Options_Button_Text = Color.White;
        public static readonly Color Options_Button_Background = Color.White * 0.25f;
        public static readonly Color Options_Button_Border = Color.White;
        public static readonly Color Options_Button_Back_Border = Color.White;
        public static readonly Color Options_Button_Back_Background = Color.White * 0.25f;
        public static readonly Color Options_Button_Back_Hovered = Color.Yellow;
        public static readonly Color Options_Button_Reset_Border = Color.DarkRed;
        public static readonly Color Options_Button_Reset_Background = Color.Red * 0.25f;
        public static readonly Color Options_Button_Reset_Hovered = Color.Orange;
        #endregion
        // =[/ZONE:PALETTE_COLORS]=
    }
}
