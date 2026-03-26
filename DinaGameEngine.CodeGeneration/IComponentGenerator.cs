using DinaGameEngine.Models.Project;

using System.Text;

namespace DinaGameEngine.CodeGeneration
{
    public interface IComponentGenerator
    {
        string ComponentType { get; }
        void GenerateField(SectionParser sectionParser, ComponentModel component, int level);

        void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level);
        void GenerateLoadUser(SectionParser sectionParser, ComponentModel component, int level);

        void GenerateReset(SectionParser sectionParser, ComponentModel component, int level);
        void GenerateResetUser(SectionParser sectionParser, ComponentModel component, int level);

        void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level);
        void GenerateUpdateUser(SectionParser sectionParser, ComponentModel component, int level);

        void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level);
        void GenerateDrawUser(SectionParser sectionParser, ComponentModel component, int level);
    }
}
