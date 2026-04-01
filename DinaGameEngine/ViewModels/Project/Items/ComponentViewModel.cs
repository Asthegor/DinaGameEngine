using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Components;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class ComponentViewModel : ItemViewModel
    {
        public ComponentViewModel(ComponentModel model, ComponentPropertiesViewModel? propertiesViewModel) : base(model)
        {
            PropertiesViewModel = propertiesViewModel;
        }

        public override string Icon => string.Empty;
        public override string Name => ((ComponentModel)Model).Key;
        public override string Key => ((ComponentModel)Model).Key;
        public string Type => ((ComponentModel)Model).Type;
        public string DeleteIcon => DinaIcon.Delete.ToGlyph();
        public ComponentPropertiesViewModel? PropertiesViewModel { get; }
    }
}