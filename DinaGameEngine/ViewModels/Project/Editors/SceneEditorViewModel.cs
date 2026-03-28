using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;

using System.Collections.ObjectModel;

namespace DinaGameEngine.ViewModels
{
    public class SceneEditorViewModel : ObservableObject
    {
        private readonly ILogService _logService;
        private readonly IComponentGeneratorRegistry _componentGeneratorRegistry;
        private SceneModel _sceneModel;
        private string _filterText = string.Empty;
        public SceneEditorViewModel(ILogService logService, IComponentGeneratorRegistry componentGeneratorRegistry, SceneModel sceneModel)
        {
            _logService = logService;
            _componentGeneratorRegistry = componentGeneratorRegistry;
            _sceneModel = sceneModel;

            foreach (var component in _sceneModel.Components)
                Components.Add(component);
        }
        public Guid SceneId => _sceneModel.Id;

        public ObservableCollection<ComponentModel> Components { get; set; } = [];
        public string FilterText
        {
            get => _filterText;
            set
            {
                SetProperty(ref _filterText, value);
                OnPropertyChanged(nameof(FilteredComponents));
            }
        }
        public IEnumerable<ComponentModel> FilteredComponents => Components.Where(c => string.IsNullOrEmpty(FilterText) 
                                                                                    || c.Key.Contains(FilterText) 
                                                                                    || c.Type.Contains(FilterText));

        public IEnumerable<IComponentGenerator> AvailableGenerators => _componentGeneratorRegistry.GetAllComponents();
        public string AddIcon => DinaIcon.Add.ToGlyph();
    }
}
