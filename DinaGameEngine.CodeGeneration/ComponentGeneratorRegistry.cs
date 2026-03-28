using DinaGameEngine.Common;
using DinaGameEngine.Models.Project;

using System.Text;

namespace DinaGameEngine.CodeGeneration
{
    public class ComponentGeneratorRegistry : IComponentGeneratorRegistry
    {
        private readonly ILogService _logService;
        private Dictionary<string, IComponentGenerator> _componentGenerators = [];

        public ComponentGeneratorRegistry(ILogService logService)
        {
            _logService = logService;
        }

        public void Register(IComponentGenerator componentGenerator)
        {
            _componentGenerators[componentGenerator.ComponentType] = componentGenerator;
        }
        public void Generate(StringBuilder sb, ComponentModel component, int level)
        {
            if (!_componentGenerators.TryGetValue(component.Type, out var generator))
            {
                _logService.Warning($"Générator non trouvé pour '{component.Type}'");
                return;
            }
            //generator.Generate(sb, component, level);
        }

        public IEnumerable<IComponentGenerator> GetAllComponents()
        {
            return _componentGenerators.Values;
        }
    }
}
