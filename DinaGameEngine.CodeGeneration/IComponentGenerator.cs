using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.CodeGeneration
{
    public interface IComponentGenerator
    {
        public string ComponentType { get; }
        public string GetFieldName(ComponentModel component);
        public void AddToDesigner(SectionParser sectionParser, ComponentModel component, string rootNamespace);
        public IEnumerable<string> RemoveFromDesigner(SectionParser sectionParser, ComponentModel component);
        public void AddToUserFile(SectionParser sectionParser, ComponentModel component);
        public void RemoveFromUserFile(SectionParser sectionParser, ComponentModel component);
    }
}
