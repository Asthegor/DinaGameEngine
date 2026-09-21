using DinaGameEngine.Commands;
using DinaGameEngine.Common;
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
        private bool _isTitlesExpanded = true;
        private bool _isItemsExpanded = true;
        public ComponentViewModel(ComponentModel model, ComponentPropertiesViewModel? propertiesViewModel)
            : base(model)
        {
            PropertiesViewModel = propertiesViewModel;

            ToggleExpandCommand = new RelayCommand(ToggleExpand);
            AddItemCommand = new RelayCommand(_ => AddItem(), _ => HasItems);
            AddTitleCommand = new RelayCommand(_ => AddTitle(), _ => HasItems);
            ToggleTitlesExpandCommand = new RelayCommand(ToggleTitlesExpand);
            ToggleItemsExpandCommand = new RelayCommand(ToggleItemsExpand);

            MenuTitles = [];
            int indexTitles = 0;
            var menuTitles = model.SubComponents.Where(c => c.Type == ComponentTypes.MenuTitle);
            foreach (var menuTitle in menuTitles)
            {
                var menuTitleViewModel = new MenuTitleViewModel(menuTitle);
                menuTitleViewModel.ItemSelected += OnMenuTitleSelected;
                menuTitleViewModel.ItemDeleted += OnMenuTitleDeleted;
                menuTitleViewModel.ItemMovedUp += OnMenuTitleMoveUp;
                menuTitleViewModel.ItemMovedDown += OnMenuTitleMoveDown;
                menuTitleViewModel.CanMoveUp = indexTitles != 0;
                menuTitleViewModel.CanMoveDown = indexTitles != menuTitles.Count() - 1;
                MenuTitles.Add(menuTitleViewModel);
                indexTitles++;
            }

            MenuItems = [];
            var indexItems = 0;
            var menuItems = model.SubComponents.Where(c => c.Type == ComponentTypes.MenuItem);
            foreach (var menuItem in menuItems)
            {
                var menuItemViewModel = new MenuItemViewModel(menuItem);
                menuItemViewModel.ItemSelected += OnMenuItemSelected;
                menuItemViewModel.ItemDeleted += OnMenuItemDeleted;
                menuItemViewModel.ItemMovedUp += OnMenuItemMoveUp;
                menuItemViewModel.ItemMovedDown += OnMenuItemMoveDown;
                menuItemViewModel.CanMoveUp = indexItems != 0;
                menuItemViewModel.CanMoveDown = indexItems != menuItems.Count() - 1;
                MenuItems.Add(menuItemViewModel);
                indexItems++;
            }

        }

        public override string Icon => string.Empty;
        public override string Name => ((ComponentModel)Model).Key;
        public override string Key => ((ComponentModel)Model).Key;
        public string Type => ((ComponentModel)Model).Type;
        public static string AddIcon => DinaIcon.Add.ToGlyph();
        public string ExpandIcon => IsExpanded ? DinaIcon.ChevronUp.ToGlyph() : DinaIcon.ChevronDown.ToGlyph();
        public ComponentPropertiesViewModel? PropertiesViewModel { get; }

        public bool HasItems => Type == ComponentTypes.MenuManager;
        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                SetProperty(ref _isExpanded, value);
                OnPropertyChanged(nameof(ExpandIcon));
            }
        }
        public RelayCommand ToggleExpandCommand { get; }
        private void ToggleExpand()
        {
            IsExpanded = !IsExpanded;
        }
        public void NotifyKeyChanged()
        {
            OnPropertyChanged(nameof(Key));
            OnPropertyChanged(nameof(Name));
        }


        #region Titles
        public ObservableCollection<MenuTitleViewModel> MenuTitles { get; set; }
        public string TitlesHeader => $"{LocalizationManager.GetTranslation("MenuManager_Titles_Header")} ({MenuTitles.Count})";
        public string TitlesExpandIcon => IsTitlesExpanded ? DinaIcon.ChevronUp.ToGlyph() : DinaIcon.ChevronDown.ToGlyph();
        public bool IsTitlesExpanded
        {
            get => _isTitlesExpanded;
            set
            {
                SetProperty(ref _isTitlesExpanded, value);
                OnPropertyChanged(nameof(TitlesExpandIcon));
            }
        }
        public RelayCommand AddTitleCommand { get; }
        private void AddTitle() => AddMenuTitleRequested?.Invoke(Model, EventArgs.Empty);
        public RelayCommand ToggleTitlesExpandCommand { get; }
        private void ToggleTitlesExpand() => IsTitlesExpanded = !IsTitlesExpanded;
        public event EventHandler? MenuTitleSelected;
        private void OnMenuTitleSelected(object? sender, EventArgs e)
        {
            if (sender is not MenuTitleViewModel vm)
                return;
            var previous = MenuTitles.FirstOrDefault(c => c.IsSelected);
            if (previous != null)
                previous.IsSelected = false;
            vm.IsSelected = true;
            MenuTitleSelected?.Invoke(vm, EventArgs.Empty);
        }
        public event EventHandler? BeforeMenuTitleRemoved;
        public event EventHandler? AfterMenuTitleRemoved;
        private void OnMenuTitleDeleted(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel menuTitleModel)
                return;
            var vm = MenuTitles.FirstOrDefault(m => (ComponentModel)m.Model == menuTitleModel);
            if (vm == null)
                return;
            BeforeMenuTitleRemoved?.Invoke(Model, EventArgs.Empty);
            MenuTitles.Remove(vm);
            ((ComponentModel)Model).SubComponents.Remove(menuTitleModel);
            AfterMenuTitleRemoved?.Invoke(Model, EventArgs.Empty);
            OnPropertyChanged(nameof(TitlesHeader));
            if (MenuTitles.Count > 0)
            {
                MoveComponentHelper.UpdateMoveFlags(MenuTitles, MenuTitles[0]);
                MoveComponentHelper.UpdateMoveFlags(MenuTitles, MenuTitles[^1]);
            }
        }
        public event EventHandler? AddMenuTitleRequested;
        public void AddMenuTitle(MenuTitleViewModel menuTitleViewModel)
        {
            menuTitleViewModel.ItemSelected += OnMenuTitleSelected;
            menuTitleViewModel.ItemDeleted += OnMenuTitleDeleted;
            menuTitleViewModel.ItemMovedUp += OnMenuTitleMoveUp;
            menuTitleViewModel.ItemMovedDown += OnMenuTitleMoveDown;
            MenuTitles.Add(menuTitleViewModel);
            OnPropertyChanged(nameof(TitlesHeader));
            MoveComponentHelper.UpdateMoveFlags(MenuTitles, MenuTitles[^1]);
            if (MenuTitles.Count > 1)
                MoveComponentHelper.UpdateMoveFlags(MenuTitles, MenuTitles[^2]);
        }
        #endregion

        #region MenuItems
        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }
        public string ItemsHeader => $"{LocalizationManager.GetTranslation("MenuManager_Items_Header")} ({MenuItems.Count})";
        public string ItemsExpandIcon => IsItemsExpanded ? DinaIcon.ChevronUp.ToGlyph() : DinaIcon.ChevronDown.ToGlyph();
        public bool IsItemsExpanded
        {
            get => _isItemsExpanded;
            set
            {
                SetProperty(ref _isItemsExpanded, value);
                OnPropertyChanged(nameof(ItemsExpandIcon));
            }
        }
        public RelayCommand AddItemCommand { get; }
        private void AddItem() => AddMenuItemRequested?.Invoke(Model, EventArgs.Empty);
        public RelayCommand ToggleItemsExpandCommand { get; }
        private void ToggleItemsExpand() => IsItemsExpanded = !IsItemsExpanded;

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

        public event EventHandler? MenuTitleMoveUp;
        private void OnMenuTitleMoveUp(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel menuTitleModel)
                return;
            MoveUp(MenuTitles, menuTitleModel, MenuTitleMoveUp);
        }
        public event EventHandler? MenuTitleMoveDown;
        private void OnMenuTitleMoveDown(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel menuTitleModel)
                return;
            MoveDown(MenuTitles, menuTitleModel, MenuTitleMoveDown);
        }

        public event EventHandler? MenuItemMoveUp;
        private void OnMenuItemMoveUp(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel menuItemModel)
                return;

            MoveUp(MenuItems, menuItemModel, MenuItemMoveUp);
        }

        public event EventHandler? MenuItemMoveDown;
        private void OnMenuItemMoveDown(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel menuItemModel)
                return;

            MoveDown(MenuItems, menuItemModel, MenuItemMoveDown);
        }
        private void MoveUp<T>(ObservableCollection<T> collection, ComponentModel item, EventHandler? movedEvent) where T : ItemViewModel
        {
            var vm = collection.FirstOrDefault(m => ((ComponentModel)m.Model) == item);
            if (vm == null)
                return;

            var swappedVm = MoveComponentHelper.MoveUpInCollection(collection, vm);
            if (swappedVm == null)
                return;

            MoveComponentModelsInSubComponents(item, (ComponentModel)swappedVm.Model);
            movedEvent?.Invoke(Model, EventArgs.Empty);
        }
        private void MoveDown<T>(ObservableCollection<T> collection, ComponentModel item, EventHandler? movedEvent) where T : ItemViewModel
        {
            var vm = collection.FirstOrDefault(m => ((ComponentModel)m.Model) == item);
            if (vm == null)
                return;

            var swappedVm = MoveComponentHelper.MoveDownInCollection(collection, vm);
            if (swappedVm == null)
                return;

            MoveComponentModelsInSubComponents(item, (ComponentModel)swappedVm.Model);
            movedEvent?.Invoke(Model, EventArgs.Empty);
        }
        private void MoveComponentModelsInSubComponents(ComponentModel modelToSwap, ComponentModel modelToBeSwapped)
        {
            var subComponents = ((ComponentModel)_model).SubComponents;
            var indexSubComponentToSwap = subComponents.IndexOf(modelToSwap);
            // Récupération de l'index du ComponentModel destination
            var indexSubComponentToBeSwapped = subComponents.IndexOf(modelToBeSwapped);
            // Swap des 2 ComponentModel
            (subComponents[indexSubComponentToBeSwapped], subComponents[indexSubComponentToSwap]) 
                = (subComponents[indexSubComponentToSwap], subComponents[indexSubComponentToBeSwapped]);
        }

        public event EventHandler? BeforeMenuItemRemoved;
        public event EventHandler? AfterMenuItemRemoved;
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
            OnPropertyChanged(nameof(ItemsHeader));

            if (MenuItems.Count > 0)
            {
                MoveComponentHelper.UpdateMoveFlags(MenuItems, MenuItems[0]);
                MoveComponentHelper.UpdateMoveFlags(MenuItems, MenuItems[^1]);
            }
        }
        public event EventHandler? AddMenuItemRequested;
        public void AddMenuItem(MenuItemViewModel menuItemViewModel)
        {
            menuItemViewModel.ItemSelected += OnMenuItemSelected;
            menuItemViewModel.ItemDeleted += OnMenuItemDeleted;
            menuItemViewModel.ItemMovedUp += OnMenuItemMoveUp;
            menuItemViewModel.ItemMovedDown += OnMenuItemMoveDown;
            MenuItems.Add(menuItemViewModel);
            OnPropertyChanged(nameof(ItemsHeader));
            MoveComponentHelper.UpdateMoveFlags(MenuItems, MenuItems[^1]);
            if (MenuItems.Count > 1)
                MoveComponentHelper.UpdateMoveFlags(MenuItems, MenuItems[^2]);
        }
        #endregion
    }
}