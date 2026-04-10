using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class MenuTitleComponentPropertiesViewModel : ShadowTextComponentPropertiesViewModel
    {
        private bool _withShadow = false;

        public MenuTitleComponentPropertiesViewModel(IEnumerable<FontModel> availableFonts, IEnumerable<ColorModel> availableColors, ComponentModel component)
            : base(availableFonts, availableColors, component)
        {
            LoadFrom(component);
            NotifyChange(false);
        }

        public override bool IsValid => SelectedFont != null && SelectedColor != null;

        public bool WithShadow
        {
            get => _withShadow;
            set
            {
                SetProperty(ref _withShadow, value);
                NotifyChange();
            }
        }
        protected override void LoadFrom(ComponentModel source)
        {
            base.LoadFrom(source);

            WithShadow = ComponentPropertyHelper.GetBoolProperty(source, "WithShadow", false);
        }

        public override void ApplyToModel()
        {
            base.ApplyToModel();

            if (WithShadow)
            {
                _component.Properties["WithShadow"] = ComponentPropertyHelper.GetReturnValueFrom(WithShadow);
            }
            else
            {
                _component.Properties.Remove("WithShadow");
                _component.Properties.Remove("ShadowColor");
                _component.Properties.Remove("ShadowOffset");
            }
        }
    }
}