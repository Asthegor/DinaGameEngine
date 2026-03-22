using DinaGameEngine.Models;

using System.Text;

namespace DinaGameEngine.CodeGeneration
{
    public interface IComponentGenerator
    {
        string ComponentType { get; }
        void GenerateField(StringBuilder sb, ComponentModel component, int level);

        void GenerateLoad(StringBuilder sb, ComponentModel component, int level);
        void GenerateLoadUser(StringBuilder sb, ComponentModel component, int level);

        void GenerateReset(StringBuilder sb, ComponentModel component, int level);
        void GenerateResetUser(StringBuilder sb, ComponentModel component, int level);

        void GenerateUpdate(StringBuilder sb, ComponentModel component, int level);
        void GenerateUpdateUser(StringBuilder sb, ComponentModel component, int level);

        void GenerateDraw(StringBuilder sb, ComponentModel component, int level);
        void GenerateDrawUser(StringBuilder sb, ComponentModel component, int level);
    }
}
