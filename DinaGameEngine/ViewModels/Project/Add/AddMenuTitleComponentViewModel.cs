using DinaGameEngine.Models.Project;

using System;
using System.Collections.Generic;
using System.Text;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddMenuTitleComponentViewModel(IEnumerable<FontModel> availableFonts, IEnumerable<ColorModel> availableColors, Action? onValidityChanged = null) : AddMenuManagerSubViewModel(availableFonts, availableColors, onValidityChanged)
    {
        private static int _nbMenuTitles;

        public int CurrentIndex { get; } = _nbMenuTitles++;
    }
}
