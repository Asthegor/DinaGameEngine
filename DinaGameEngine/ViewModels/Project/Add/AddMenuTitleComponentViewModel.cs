using DinaGameEngine.Models.Project;

using System;
using System.Collections.Generic;
using System.Text;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddMenuTitleComponentViewModel : AddMenuManagerSubViewModel
    {
        private static int _nbMenuTitles;

        public AddMenuTitleComponentViewModel(IEnumerable<FontModel> availableFonts, IEnumerable<ColorModel> availableColors, Action? onValidityChanged = null)
            : base(availableFonts, availableColors, onValidityChanged)
        {
            CurrentIndex = _nbMenuTitles++;
        }

        public int CurrentIndex { get; }
    }
}
