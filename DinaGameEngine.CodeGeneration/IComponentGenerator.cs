using DinaGameEngine.Models.Project;

namespace DinaGameEngine.CodeGeneration
{
    public interface IComponentGenerator
    {
        public string ComponentType { get; }
        public string GetFieldName(ComponentModel component);
        public void Add(SectionParser sectionParser, ComponentModel component);
        public IEnumerable<string> Remove(SectionParser sectionParser, ComponentModel component);
        public void GenerateUserFileCommentField(SectionParser sectionParser, ComponentModel component);
        public void RemoveUserFileCommentField(SectionParser sectionParser, ComponentModel component);
    }
}
