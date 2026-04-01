using DinaGameEngine.Models.Project;

namespace DinaGameEngine.Abstractions
{
    public interface IAddComponentSpecificViewModel
    {
        bool IsValid { get; }
        void ApplyToModel(ComponentModel component);
    }
}
