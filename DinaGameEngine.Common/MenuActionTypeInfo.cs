using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.Common
{
    public static class MenuActionTypeInfo
    {
        // Absent de ce dictionnaire = valide dans les 4 catégories (types futurs répétables, §3.1)
        private static readonly Dictionary<MenuActionType, MenuActionCategory[]> _validCategories = new()
        {
            [MenuActionType.ChangeColor] = [MenuActionCategory.Selection, MenuActionCategory.Deselection],
            [MenuActionType.ChangeScene] = [MenuActionCategory.Activation],
        };

        private static readonly HashSet<MenuActionType> _singletons =
        [
            MenuActionType.ChangeColor,
            MenuActionType.ChangeScene,
        ];

        public static IEnumerable<MenuActionType> GetValidTypes(MenuActionCategory category)
            => Enum.GetValues<MenuActionType>()
                   .Where(t => !_validCategories.TryGetValue(t, out var categories) || categories.Contains(category));

        public static bool IsSingleton(MenuActionType type) => _singletons.Contains(type);
    }
}
