using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddMenuManagerComponentViewModel(Action? onValidityChanged = null)
        : ObservableObject, IAddComponentSpecificViewModel
    {
        private readonly Action? _onValidityChanged = onValidityChanged;

        public bool IsValid => true;

        public void ApplyToModel(ComponentModel component)
        {
            // TODO: à modifier quand on rajoutera les options du MenuManager
        }
    }
}
