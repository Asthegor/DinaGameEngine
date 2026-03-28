using DinaGameEngine.Models.Project;

using System.Text;

namespace DinaGameEngine.CodeGeneration
{
    public interface IComponentGeneratorRegistry
    {
        public void Register(IComponentGenerator componentGenerator);
        public void Generate(StringBuilder sb, ComponentModel component, int level);
        IEnumerable<IComponentGenerator> GetAllComponents();
    }
}
