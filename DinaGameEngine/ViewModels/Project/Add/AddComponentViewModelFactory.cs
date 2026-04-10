using DinaGameEngine.Abstractions;
using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddComponentViewModelFactory : IAddComponentViewModelFactory
    {
        public IAddComponentSpecificViewModel? Create(string componentType, GameProjectModel gameProjectModel, Action onValidityChanged)
        {
            return componentType switch
            {
                ComponentTypes.Text => new AddTextComponentViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, onValidityChanged),
                ComponentTypes.MenuManager => new AddMenuManagerComponentViewModel(),
                ComponentTypes.MenuItem => new AddMenuItemComponentViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, onValidityChanged),
                ComponentTypes.MenuTitle => new AddMenuTitleComponentViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, onValidityChanged),
                ComponentTypes.ShadowText => new AddShadowTextViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, onValidityChanged),
                _ => null
            };
        }
    }
}