using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Components;

using System.Collections.ObjectModel;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class ComponentViewModel : ItemViewModel
    {
        private bool _isExpanded;

        public ComponentViewModel(ComponentModel model, ComponentPropertiesViewModel? propertiesViewModel)
            : base(model)
        {
            PropertiesViewModel = propertiesViewModel;

            ToggleExpandCommand = new RelayCommand(ToggleExpand);
            AddItemCommand = new RelayCommand(_ => AddItem(), _ => HasItems);

            MenuItems = [];
            foreach (var menuItem in model.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var menuItemViewModel = new MenuItemViewModel(menuItem);
                menuItemViewModel.ItemSelected += OnMenuItemSelected;
                menuItemViewModel.ItemDeleted += OnMenuItemDeleted;
                MenuItems.Add(menuItemViewModel);
            }

        }

        public override string Icon => string.Empty;
        public override string Name => ((ComponentModel)Model).Key;
        public override string Key => ((ComponentModel)Model).Key;
        public string Type => ((ComponentModel)Model).Type;
        public static string AddIcon => DinaIcon.Add.ToGlyph();
        public string ExpandIcon => IsExpanded ? DinaIcon.ChevronUp.ToGlyph() : DinaIcon.ChevronDown.ToGlyph();
        public ComponentPropertiesViewModel? PropertiesViewModel { get; }

        public bool HasItems => Type == "MenuManager";
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                SetProperty(ref _isExpanded, value);
                OnPropertyChanged(nameof(ExpandIcon));
            }
        }
        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

        public event EventHandler? MenuItemSelected;
        private void OnMenuItemSelected(object? sender, EventArgs e)
        {
            if (sender is not MenuItemViewModel vm)
                return;
            var previous = MenuItems.FirstOrDefault(c => c.IsSelected);
            if (previous != null)
                previous.IsSelected = false;
            vm.IsSelected = true;
            MenuItemSelected?.Invoke(vm, EventArgs.Empty);
        }
        private void OnMenuItemDeleted(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel menuItemModel)
                return;

            var vm = MenuItems.FirstOrDefault(m => (ComponentModel)m.Model == menuItemModel);
            if (vm == null)
                return;

            BeforeMenuItemRemoved?.Invoke(Model, EventArgs.Empty);
            MenuItems.Remove(vm);
            ((ComponentModel)Model).SubComponents.Remove(menuItemModel);
            AfterMenuItemRemoved?.Invoke(Model, EventArgs.Empty);
        }

        public RelayCommand ToggleExpandCommand { get; }
        private void ToggleExpand()
        {
            IsExpanded = !IsExpanded;
        }

        public RelayCommand AddItemCommand { get; }
        private void AddItem() => AddMenuItemRequested?.Invoke(Model, EventArgs.Empty);

        public event EventHandler? AfterMenuItemAdded;
        public event EventHandler? BeforeMenuItemRemoved;
        public event EventHandler? AfterMenuItemRemoved;
        public event EventHandler? AddMenuItemRequested;

        public void AddMenuItem(MenuItemViewModel menuItemViewModel)
        {
            menuItemViewModel.ItemSelected += OnMenuItemSelected;
            menuItemViewModel.ItemDeleted += OnMenuItemDeleted;
            MenuItems.Add(menuItemViewModel);
        }
    }
}