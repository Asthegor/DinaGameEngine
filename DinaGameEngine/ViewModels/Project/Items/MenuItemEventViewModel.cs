using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

using System.Collections.ObjectModel;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class MenuItemEventViewModel : ObservableObject
    {
        private readonly ComponentModel _eventModel;
        private readonly Func<MenuActionType, IEnumerable<string>> _resolveAvailableKeys;
        private readonly Action _notifyChange;
        private readonly bool _showIncluded;
        private bool _isIncluded;

        public MenuItemEventViewModel(MenuActionCategory category,
                                      Func<MenuActionType, IEnumerable<string>> resolveAvailableKeys,
                                      Action notifyChange, bool showIncluded = false)
        {
            Category = category;
            _resolveAvailableKeys = resolveAvailableKeys;
            _notifyChange = notifyChange;
            _eventModel = MenuItemEventHelper.CreateEvent(category);
            _showIncluded = showIncluded;

            AddActionCommand = new RelayCommand(type => AddAction((MenuActionType)type!));
            ClearCommand = new RelayCommand(_ => Clear(), _ => Actions.Count > 0);
        }

        public MenuActionCategory Category { get; }
        public ObservableCollection<MenuItemEventActionViewModel> Actions { get; } = [];

        // Si l'action n'est pas incluse, ses actions n'ont pas à être valides.
        public bool IsValid => (ShowIncluded && !IsIncluded) || Actions.All(a => a.HasValue);
        public bool ShowIncluded => _showIncluded;
        public bool IsIncluded
        {
            get => _isIncluded;
            set
            {
                if (_isIncluded == value)
                    return;
                _isIncluded = value;
                OnPropertyChanged(nameof(IsIncluded));
                OnPropertyChanged(nameof(IsValid));
                _notifyChange();
            }
        }
        public IEnumerable<MenuActionType> AvailableTypesToAdd =>
            MenuActionTypeInfo.GetValidTypes(Category)
                .Where(t => !MenuActionTypeInfo.IsSingleton(t) || !Actions.Any(a => a.ActionType == t));

        public bool CanAdd => AvailableTypesToAdd.Any();

        public RelayCommand AddActionCommand { get; }
        public RelayCommand ClearCommand { get; }

        public void LoadFrom(ComponentModel source)
        {
            Actions.Clear();

            var eventModel = source.SubComponents.FirstOrDefault(c => c.Type == ComponentTypes.MenuItemEvent
                                                                      && MenuItemEventHelper.GetCategory(c) == Category);
            IsIncluded = eventModel is not null && ComponentPropertyHelper.GetBoolProperty(eventModel, "Included", false);
            if (eventModel is not null)
            {
                foreach (var actionModel in eventModel.SubComponents)
                    Attach(new MenuItemEventActionViewModel(actionModel, _resolveAvailableKeys));
                
            }

            RaiseCollectionDependentChanges();
        }

        public ComponentModel ToModel()
        {
            _eventModel.SubComponents = [.. Actions.Select(a => (ComponentModel)a.Model)];
            if (ShowIncluded && IsIncluded)
                _eventModel.Properties["Included"] = true;
            else
                _eventModel.Properties.Remove("Included");
            return _eventModel;
        }

        private void AddAction(MenuActionType type)
        {
            Attach(new MenuItemEventActionViewModel(MenuItemEventHelper.CreateAction(type), _resolveAvailableKeys));
            RaiseCollectionDependentChanges();
            _notifyChange();
        }

        private void Attach(MenuItemEventActionViewModel action)
        {
            action.ItemMovedUp += (_, __) => { MoveComponentHelper.MoveUpInCollection(Actions, action); _notifyChange(); };
            action.ItemMovedDown += (_, __) => { MoveComponentHelper.MoveDownInCollection(Actions, action); _notifyChange(); };
            action.ItemDeleted += (_, __) => RemoveAction(action);
            action.Changed += (_, __) => _notifyChange();

            Actions.Add(action);
            MoveComponentHelper.UpdateMoveFlags(Actions, action);
        }

        private void RemoveAction(MenuItemEventActionViewModel action)
        {
            Actions.Remove(action);
            foreach (var remaining in Actions)
                MoveComponentHelper.UpdateMoveFlags(Actions, remaining);
            RaiseCollectionDependentChanges();
            _notifyChange();
        }

        private void Clear()
        {
            Actions.Clear();
            RaiseCollectionDependentChanges();
            _notifyChange();
        }

        private void RaiseCollectionDependentChanges()
        {
            OnPropertyChanged(nameof(IsValid));
            OnPropertyChanged(nameof(AvailableTypesToAdd));
            OnPropertyChanged(nameof(CanAdd));
            ClearCommand.RaiseCanExecuteChanged();
        }
    }
}