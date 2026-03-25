using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.Common.Events
{
    public class NavigationRequestedEventArgs(NavigationRequest request, object? parameter = null) : EventArgs
    {
        public NavigationRequest Request { get; } = request;
        public object? Parameter { get; } = parameter;
    }
}
