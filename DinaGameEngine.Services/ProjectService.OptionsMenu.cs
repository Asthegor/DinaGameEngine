using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.Services
{
    public partial class ProjectService
    {
        private void AddOptionsMenuDefaults(GameProjectModel gameProjectModel)
        {
#if DEBUG // TODO: à retirer une fois avoir terminé les contrôles Slider, CheckBox, ListBox, Button, Group, ShadowText
            gameProjectModel.Scenes.Add(new SceneModel { Name = "Options", Class = "OptionsMenuScene", Key = "OptionsMenuScene", ToBeIncluded = false });
#else
            gameProjectModel.Scenes.Add(new SceneModel { Name = "Options", Class = "OptionsMenuScene", Key = "OptionsMenuScene" });
#endif

            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Title", R = 255, G = 165, B = 000, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Title_Shadow", R = 169, G = 169, B = 169, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Category", R = 211, G = 211, B = 211, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Label", R = 255, G = 255, B = 255, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Text", R = 255, G = 255, B = 255, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Background", R = 063, G = 063, B = 063, A = 063 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Border", R = 255, G = 255, B = 255, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Back_Border", R = 255, G = 255, B = 255, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Back_Background", R = 063, G = 063, B = 063, A = 063 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Back_Hovered", R = 255, G = 255, B = 000, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Reset_Border", R = 139, G = 000, B = 000, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Reset_Background", R = 063, G = 000, B = 000, A = 063 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "Options_Button_Reset_Hovered", R = 255, G = 165, B = 000, A = 255 });
        }
    }
}