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

        public MenuItemEventViewModel(MenuActionCategory category,
                                      Func<MenuActionType, IEnumerable<string>> resolveAvailableKeys,
                                      Action notifyChange)
        {
            Category = category;
            _resolveAvailableKeys = resolveAvailableKeys;
            _notifyChange = notifyChange;
            _eventModel = MenuItemEventHelper.CreateEvent(category);

            AddActionCommand = new RelayCommand(type => AddAction((MenuActionType)type!));
            ClearCommand = new RelayCommand(_ => Clear(), _ => Actions.Count > 0);
        }

        public MenuActionCategory Category { get; }
        public ObservableCollection<MenuItemEventActionViewModel> Actions { get; } = [];

        public bool IsValid => Actions.All(a => a.HasValue);

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
            if (eventModel != null)
            {
                foreach (var actionModel in eventModel.SubComponents)
                    Attach(new MenuItemEventActionViewModel(actionModel, _resolveAvailableKeys));
            }

            RaiseCollectionDependentChanges();
        }

        public ComponentModel ToModel()
        {
            _eventModel.SubComponents = [.. Actions.Select(a => (ComponentModel)a.Model)];
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