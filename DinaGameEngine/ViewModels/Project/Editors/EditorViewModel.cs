using DinaGameEngine.Common;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Items;

using System.Collections.ObjectModel;

namespace DinaGameEngine.ViewModels.Project.Editors
{
    public abstract class EditorViewModel<TViewModel> : ObservableObject
    {
        private ItemViewModel? _selectedItem;
        public EditorViewModel(List<ItemModel> items)
        {
            Items.Clear();
            foreach (var item in items)
                AddItem(item);
        }

        public event EventHandler? ItemOpenRequested;
        public event EventHandler? ItemDeleteRequested;

        public ObservableCollection<ItemViewModel> Items { get; set; } = [];
        public ItemViewModel? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem?.IsSelected = false;
                value?.IsSelected = true;
                SetProperty(ref _selectedItem, value);
            }
        }

        public void AddItem(ItemModel model)
        {
            var viewModel = (ItemViewModel)Activator.CreateInstance(typeof(TViewModel), model)!;
            ((ItemViewModel)viewModel).ItemOpened += (sender, eventArgs) => ItemOpenRequested?.Invoke(sender, eventArgs);
            viewModel.ItemDeleted += (sender, eventArgs) => ItemDeleteRequested?.Invoke(sender, eventArgs);
            viewModel.ItemSelected += (sender, eventArgs) => SelectedItem = (ItemViewModel)sender!;
            Items.Add(viewModel);
        }

        public void RemoveItem(ItemModel model)
        {
            var viewModel = Items.FirstOrDefault(i => i.ItemId == model.Id);
            if (viewModel != null)
                Items.Remove(viewModel);
        }
    }
}
