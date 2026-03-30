using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;

using System.Windows.Media;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class ColorViewModel : ItemViewModel
    {
        public ColorViewModel(ColorModel model) : base(model)
        {
        }

        public override string Icon => DinaIcon.Color.ToGlyph();
        public override string Name => ((ColorModel)Model).Key;
        public override string Key => ((ColorModel)Model).Key;
        public Color PreviewColor => Color.FromArgb(((ColorModel)Model).A, ((ColorModel)Model).R, ((ColorModel)Model).G, ((ColorModel)Model).B);
    }
}
