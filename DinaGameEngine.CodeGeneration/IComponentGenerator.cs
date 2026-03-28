using DinaGameEngine.Models.Project;

namespace DinaGameEngine.CodeGeneration
{
    public interface IComponentGenerator
    {
        public string ComponentType { get; }
        public void GenerateField(SectionParser sectionParser, ComponentModel component, int level);

        public void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level);
        public void GenerateLoadUser(SectionParser sectionParser, ComponentModel component, int level);

        public void GenerateReset(SectionParser sectionParser, ComponentModel component, int level);
        public void GenerateResetUser(SectionParser sectionParser, ComponentModel component, int level);

        public void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level);
        public void GenerateUpdateUser(SectionParser sectionParser, ComponentModel component, int level);

        public void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level);
        public void GenerateDrawUser(SectionParser sectionParser, ComponentModel component, int level);
    }
}
