using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Add
{

    public class AddMenuItemComponentViewModel(IEnumerable<FontModel> availableFonts, IEnumerable<ColorModel> availableColors, Action? onValidityChanged = null) : AddMenuManagerSubViewModel(availableFonts, availableColors, onValidityChanged)
    {
        private static int _nbMenuItems;

        public int CurrentIndex { get; } = _nbMenuItems++;
    }
}