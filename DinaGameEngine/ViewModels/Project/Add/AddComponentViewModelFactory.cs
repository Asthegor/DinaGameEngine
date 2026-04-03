using DinaGameEngine.Abstractions;
using DinaGameEngine.Models;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddComponentViewModelFactory : IAddComponentViewModelFactory
    {
        public IAddComponentSpecificViewModel? Create(string componentType, GameProjectModel gameProjectModel, Action onValidityChanged)
        {
            return componentType switch
            {
                "Text" => new AddTextComponentViewModel(gameProjectModel.Fonts, gameProjectModel.Colors, onValidityChanged),
                "MenuManager" => new AddMenuManagerComponentViewModel(),
                _ => null
            };
        }
    }
}