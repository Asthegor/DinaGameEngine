using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.Models.Helpers
{
    public static class MenuItemEventHelper
    {
        public static ComponentModel CreateEvent(MenuActionCategory category)
            => new()
            {
                Type = ComponentTypes.MenuItemEvent,
                Properties = { ["Category"] = category.ToString() }
            };

        public static ComponentModel CreateAction(MenuActionType action)
            => new() { Type = action.ToString() };

        public static MenuActionCategory GetCategory(ComponentModel eventComponent, MenuActionCategory defaultValue = default)
            => ComponentPropertyHelper.GetEnumProperty(eventComponent, "Category", defaultValue);

        public static MenuActionType GetActionType(ComponentModel actionComponent)
            => Enum.Parse<MenuActionType>(actionComponent.Type);

        public static ComponentModel? FindEvent(ComponentModel source, MenuActionCategory category)
            => source.SubComponents.FirstOrDefault(c => c.Type == ComponentTypes.MenuItemEvent && GetCategory(c) == category);
    }
}