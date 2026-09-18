using DinaGameEngine.Models;

namespace DinaGameEngine.Abstractions
{
    public interface IAddComponentViewModelFactory
    {
        IAddComponentSpecificViewModel? Create(string componentType, GameProjectModel gameProjectModel, Action onValidityChanged);
    }
}
