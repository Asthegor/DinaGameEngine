using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.Abstractions
{
    public interface INavigationService
    {
        void Navigate(NavigationRequest request, object? parameter = null);
    }
}
