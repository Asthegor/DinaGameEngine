using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.Services
{
    public partial class ProjectService
    {
        private void AddFontDefaults(GameProjectModel gameProjectModel)
        {
            gameProjectModel.Fonts.Add(new FontModel { Key = "Default", Size = 12, Spacing = 0, Style = SpriteFontStyle.Regular, TtfRelativePath = "../TTF_Files/Roboto-Regular.ttf" });
        }
    }
}