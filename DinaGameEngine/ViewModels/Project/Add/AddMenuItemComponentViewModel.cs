using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Add
{

    public class AddMenuItemComponentViewModel : AddMenuManagerSubViewModel
    {
        private static int _nbMenuItems;

        public AddMenuItemComponentViewModel(IEnumerable<FontModel> availableFonts, IEnumerable<ColorModel> availableColors, Action? onValidityChanged = null) : base(availableFonts, availableColors, onValidityChanged)
        {
            CurrentIndex = _nbMenuItems++;
        }

        public int CurrentIndex { get; }
    }
}