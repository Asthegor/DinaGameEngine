using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;

using System.IO;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class FontViewModel : ItemViewModel
    {
        public FontViewModel(FontModel fontModel) : base(fontModel) { }
        public override string Icon => DinaIcon.Font.ToGlyph();
        public override string Name => ((FontModel)Model).Key;
        public override string Key => ((FontModel)Model).Key;
        public string FontDetails => $"{Path.GetFileName(((FontModel)Model).TtfRelativePath)} - {((FontModel)Model).Size}pt — {((FontModel)Model).Style}";
    }
}
