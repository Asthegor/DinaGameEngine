using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Interfaces;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Components
{
    public class ComponentPropertiesViewModelFactory : IComponentPropertiesViewModelFactory
    {
        public ComponentPropertiesViewModel? Create(string componentType, ComponentModel component, GameProjectModel gameProjectModel)
        {
            return componentType switch
            {
                ComponentTypes.Text => new TextComponentPropertiesViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, component),
                ComponentTypes.MenuManager => new MenuManagerComponentPropertiesViewModel(component),
                ComponentTypes.MenuItem => new MenuItemComponentPropertiesViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, component),
                ComponentTypes.MenuTitle => new MenuTitleComponentPropertiesViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, component),
                ComponentTypes.ShadowText => new ShadowTextComponentPropertiesViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, component),
                _ => null
            };
        }
    }
}
