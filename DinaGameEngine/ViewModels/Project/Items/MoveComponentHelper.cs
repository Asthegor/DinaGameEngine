using System.Collections.ObjectModel;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class MoveComponentHelper
    {
        public static T? MoveUpInCollection<T>(ObservableCollection<T> collection, T item) where T : ItemViewModel
        {
            if (!item.CanMoveUp)
                return null;

            var index = collection.IndexOf(item);
            var swappedItem = collection[index - 1];
            collection.Move(index, index - 1);

            UpdateMoveFlags(collection, item);
            UpdateMoveFlags(collection, swappedItem);

            return swappedItem;
        }
        public static T? MoveDownInCollection<T>(ObservableCollection<T> collection, T item) where T : ItemViewModel
        {
            if (!item.CanMoveDown)
                return null;

            var index = collection.IndexOf(item);
            var swappedItem = collection[index + 1];
            collection.Move(index, index + 1);

            UpdateMoveFlags(collection, item);
            UpdateMoveFlags(collection, swappedItem);

            return swappedItem;
        }
        public static void UpdateMoveFlags<T>(ObservableCollection<T> collection, T item) where T : ItemViewModel
        {
            var idx = collection.IndexOf(item);
            item.CanMoveUp = idx > 0;
            item.CanMoveDown = idx < collection.Count - 1;
        }

    }
}
