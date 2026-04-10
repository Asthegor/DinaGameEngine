using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.CodeGeneration
{
    public class ComponentGeneratorRegistry(ILogService logService) : IComponentGeneratorRegistry
    {
        private readonly Dictionary<string, IComponentGenerator> _componentGenerators = [];

        public void Register(IComponentGenerator componentGenerator)
        {
            _componentGenerators[componentGenerator.ComponentType] = componentGenerator;
            logService.Info($"ComponentGenerator '{componentGenerator.ComponentType}' enregistré.");
        }
        public IComponentGenerator? GetGenerator(string componentType)
        {
            _componentGenerators.TryGetValue(componentType, out var generator);
            return generator;
        }

        public IEnumerable<IComponentGenerator> GetAllComponents()
        {
            return _componentGenerators.Values;
        }
    }
}
