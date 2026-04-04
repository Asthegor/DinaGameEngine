using DinaGameEngine.Abstractions;
using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Interfaces;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Project.Add;
using DinaGameEngine.ViewModels.Project.Components;
using DinaGameEngine.ViewModels.Project.Items;
using DinaGameEngine.Views.Project.Add;

using System.Collections.ObjectModel;

namespace DinaGameEngine.ViewModels.Project.Editors
{
    public class SceneEditorViewModel : ObservableObject
    {
        private readonly ILogService _logService;
        private readonly IComponentGeneratorRegistry _componentGeneratorRegistry;
        private readonly SceneModel _sceneModel;
        private readonly GameProjectModel _gameProjectModel;
        private readonly IComponentPropertiesViewModelFactory _propertiesViewModelFactory;
        private readonly IAddComponentViewModelFactory _addComponentViewModelFactory;
        private readonly ICodeGenerator _codeGenerator;
        private readonly IProjectService _projectService;

        private string _filterText = string.Empty;

        public SceneEditorViewModel(ILogService logService, IComponentGeneratorRegistry componentGeneratorRegistry,
                                    SceneModel sceneModel, GameProjectModel gameProjectModel,
                                    IComponentPropertiesViewModelFactory propertiesViewModelFactory,
                                    IAddComponentViewModelFactory addComponentViewModelFactory,
                                    ICodeGenerator codeGenerator, IProjectService projectService)
        {
            _logService = logService;
            _componentGeneratorRegistry = componentGeneratorRegistry;
            _sceneModel = sceneModel;
            _gameProjectModel = gameProjectModel;
            _propertiesViewModelFactory = propertiesViewModelFactory;
            _addComponentViewModelFactory = addComponentViewModelFactory;
            _codeGenerator = codeGenerator;
            _projectService = projectService;

            foreach (var component in _sceneModel.Components)
            {
                var propertiesVm = _propertiesViewModelFactory.Create(component.Type, component, _gameProjectModel);
                var vm = new ComponentViewModel(component, propertiesVm);
                vm.ItemSelected += OnComponentSelected;
                vm.ItemDeleted += OnComponentDeleted;
                vm.BeforeMenuItemRemoved += OnMenuItemBeforeChanged;
                vm.AfterMenuItemRemoved += OnMenuItemChanged;
                vm.AddMenuItemRequested += OnAddMenuItemRequested;
                vm.MenuItemSelected += OnMenuItemSelected;
                Components.Add(vm);
            }

            AddComponentCommand = new RelayCommand(generator => AddComponent(generator as IComponentGenerator));
        }
        public Guid SceneId => _sceneModel.Id;

        public ObservableCollection<ComponentViewModel> Components { get; set; } = [];
        public string FilterText
        {
            get => _filterText;
            set
            {
                SetProperty(ref _filterText, value);
                OnPropertyChanged(nameof(FilteredComponents));
            }
        }
        public IEnumerable<ComponentViewModel> FilteredComponents =>
            Components.Where(c => string.IsNullOrEmpty(FilterText)
                               || c.Key.Contains(FilterText)
                               || c.Type.Contains(FilterText));

        public IEnumerable<IComponentGenerator> AvailableGenerators => _componentGeneratorRegistry.GetAllComponents();
        public static string AddIcon => DinaIcon.Add.ToGlyph();

        private ComponentPropertiesViewModel? _selectedComponentViewModel;
        public ComponentPropertiesViewModel? SelectedComponentViewModel
        {
            get => _selectedComponentViewModel;
            set => SetProperty(ref _selectedComponentViewModel, value);
        }
        private void OnComponentSelected(object? sender, EventArgs e)
        {
            if (sender is not ComponentViewModel vm)
                return;
            // On désactive tous les MenuItems
            foreach(var c in Components)
            {
                foreach (var menuItem in c.MenuItems)
                    menuItem.IsSelected = false;
            }
            var previous = Components.FirstOrDefault(c => c.IsSelected);
            if (previous != null)
            {
                previous.IsSelected = false;
                if (previous.PropertiesViewModel != null)
                    previous.PropertiesViewModel.Applied -= OnComponentApplied;
            }
            vm.IsSelected = true;
            if (vm.PropertiesViewModel != null)
                vm.PropertiesViewModel.Applied += OnComponentApplied;
            SelectedComponentViewModel = vm.PropertiesViewModel;
        }
        private void OnComponentDeleted(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel component)
                return;

            var vm = Components.FirstOrDefault(c => c.Model == component);
            if (vm == null)
                return;

            Components.Remove(vm);
            _sceneModel.Components.Remove(component);

            _codeGenerator.RemoveComponent(_gameProjectModel, _sceneModel, component);
            _projectService.UpdateJsonProjectFile(_gameProjectModel);

            if (SelectedComponentViewModel?.Component == component)
                SelectedComponentViewModel = null;

            OnPropertyChanged(nameof(FilteredComponents));
        }
        private void OnComponentApplied(object? sender, ComponentModel oldSnapshot)
        {
            if (sender is not ComponentPropertiesViewModel vm)
                return;

            _codeGenerator.RemoveComponent(_gameProjectModel, _sceneModel, oldSnapshot, showWarning: false);
            _codeGenerator.AddComponent(_gameProjectModel, _sceneModel, vm.Component);
            _projectService.UpdateJsonProjectFile(_gameProjectModel);

            var componentVm = Components.FirstOrDefault(c => c.Model == vm.Component);
            componentVm?.NotifyKeyChanged();
        }

        public RelayCommand AddComponentCommand { get; }
        private void AddComponent(IComponentGenerator? generator)
        {
            if (generator == null)
                return;

            var newComponent = new ComponentModel { Type = generator.ComponentType };
            var existingKeys = _sceneModel.Components.Select(c => c.Key).ToList();
            var addComponentViewModel = new AddComponentViewModel(existingKeys, $"AddComponent_{generator.ComponentType}_Title");

            var addVm = _addComponentViewModelFactory.Create(generator.ComponentType, _gameProjectModel,
                                                             addComponentViewModel.ConfirmCommand.RaiseCanExecuteChanged);

            addComponentViewModel.SpecificProperties = addVm;
            if (addVm != null)
                addComponentViewModel.RegisterValidator(() => addVm.IsValid);

            bool confirmed = false;
            addComponentViewModel.ComponentConfirmed += (s, result) => confirmed = result;

            var window = new AddComponentWindow { DataContext = addComponentViewModel };
            window.ShowDialog();

            if (confirmed)
            {
                newComponent.Key = addComponentViewModel.Key;
                addVm?.ApplyToModel(newComponent);
                _sceneModel.Components.Add(newComponent);
                var propertiesVm = _propertiesViewModelFactory.Create(newComponent.Type, newComponent, _gameProjectModel);
                var vm = new ComponentViewModel(newComponent, propertiesVm);
                vm.ItemSelected += OnComponentSelected;
                vm.ItemDeleted += OnComponentDeleted;
                vm.BeforeMenuItemRemoved += OnMenuItemBeforeChanged;
                vm.AfterMenuItemRemoved += OnMenuItemChanged;
                vm.AddMenuItemRequested += OnAddMenuItemRequested;
                vm.MenuItemSelected += OnMenuItemSelected;
                Components.Add(vm);
                _codeGenerator.AddComponent(_gameProjectModel, _sceneModel, newComponent);
                _projectService.UpdateJsonProjectFile(_gameProjectModel);
                OnPropertyChanged(nameof(FilteredComponents));
            }
        }
        private void OnMenuItemBeforeChanged(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel component)
                return;
            _codeGenerator.RemoveComponent(_gameProjectModel, _sceneModel, component, showWarning: false);
        }
        private void OnMenuItemChanged(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel component)
                return;
            _codeGenerator.AddComponent(_gameProjectModel, _sceneModel, component);
            _projectService.UpdateJsonProjectFile(_gameProjectModel);

            var componentVm = Components.FirstOrDefault(c => c.Model == component);
            componentVm?.NotifyKeyChanged();
        }
        private void OnMenuItemAdded(object? sender, EventArgs e)
        {
            OnMenuItemBeforeChanged(sender, e);
            OnMenuItemChanged(sender, e);
        }
        private void OnAddMenuItemRequested(object? sender, EventArgs e)
        {
            if (sender is not ComponentModel component)
                return;

            var vm = Components.FirstOrDefault(c => c.Model == component);
            if (vm == null)
                return;

            var existingKeys = component.SubComponents.Select(c => c.Key).ToList();
            var addComponentViewModel = new AddComponentViewModel(existingKeys, "AddComponent_MenuItem_Title");

            var addVm = _addComponentViewModelFactory.Create("MenuItem", _gameProjectModel,
                                                             addComponentViewModel.ConfirmCommand.RaiseCanExecuteChanged);
            addComponentViewModel.SpecificProperties = addVm;
            if (addVm != null)
                addComponentViewModel.RegisterValidator(() => addVm.IsValid);

            bool confirmed = false;
            addComponentViewModel.ComponentConfirmed += (s, result) => confirmed = result;

            var window = new AddComponentWindow { DataContext = addComponentViewModel };
            window.ShowDialog();

            if (!confirmed)
                return;

            OnMenuItemBeforeChanged(sender, e);

            var menuItemModel = new ComponentModel
            {
                Key = addComponentViewModel.Key,
                Type = "MenuItem"
            };
            addVm?.ApplyToModel(menuItemModel);

            component.SubComponents.Add(menuItemModel);
            vm.AddMenuItem(new MenuItemViewModel(menuItemModel));

            OnMenuItemChanged(sender, e);
        }

        private void OnMenuItemSelected(object? sender, EventArgs e)
        {
            if (sender is not MenuItemViewModel menuItemVm)
                return;

            // Désélectionner le composant parent
            var parentVm = Components.FirstOrDefault(c => c.IsSelected);
            if (parentVm != null)
            {
                parentVm.IsSelected = false;
                if (parentVm.PropertiesViewModel != null)
                    parentVm.PropertiesViewModel.Applied -= OnComponentApplied;
            }

            // Afficher les propriétés du MenuItem
            var menuItemPropertiesVm = _propertiesViewModelFactory.Create("MenuItem", (ComponentModel)menuItemVm.Model, _gameProjectModel);
            if (menuItemPropertiesVm != null)
                menuItemPropertiesVm.Applied += OnMenuItemApplied;
            SelectedComponentViewModel = menuItemPropertiesVm;
        }
        private void OnMenuItemApplied(object? sender, ComponentModel oldSnapshot)
        {
            _projectService.UpdateJsonProjectFile(_gameProjectModel);

            var parentVm = Components.FirstOrDefault(c => c.MenuItems.Any(m => m.Model == SelectedComponentViewModel?.Component));
            var menuItemVm = parentVm?.MenuItems.FirstOrDefault(m => m.Model == SelectedComponentViewModel?.Component);
            menuItemVm?.NotifyChanged();
        }
    }
}
