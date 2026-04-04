using DinaGameEngine.Interfaces;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Add;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class ComponentPropertiesViewModelFactory : IComponentPropertiesViewModelFactory
    {
        public ComponentPropertiesViewModel? Create(string componentType, ComponentModel component, GameProjectModel gameProjectModel)
        {
            return componentType switch
            {
                "Text" => new TextComponentPropertiesViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, component),
                "MenuManager" => new MenuManagerComponentPropertiesViewModel(component),
                "MenuItem" => new MenuItemComponentPropertiesViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, component),
                "MenuTitle" => new MenuTitleComponentPropertiesViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, component),
                _ => null
            };
        }
    }
}
