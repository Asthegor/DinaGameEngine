using DinaGameEngine.Common;

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
