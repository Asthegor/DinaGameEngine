using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Components;

namespace DinaGameEngine.Interfaces
{
    public interface IComponentPropertiesViewModelFactory
    {
        ComponentPropertiesViewModel? Create(string componentType, ComponentModel component, GameProjectModel gameProjectModel);
    }
}
