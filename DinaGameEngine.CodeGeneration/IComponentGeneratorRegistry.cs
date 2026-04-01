namespace DinaGameEngine.CodeGeneration
{
    public interface IComponentGeneratorRegistry
    {
        public void Register(IComponentGenerator componentGenerator);
        public IComponentGenerator? GetGenerator(string componentType);
        IEnumerable<IComponentGenerator> GetAllComponents();
    }
}
