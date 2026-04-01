using DinaGameEngine.Models.Project;

namespace DinaGameEngine.CodeGeneration.ComponentGenerators
{
    public abstract class ComponentGenerator
    {
        public abstract string ComponentType { get; }
        public string GetFieldName(ComponentModel component) => $"_{component.Key}{ComponentType}";

        public void Add(SectionParser sectionParser, ComponentModel component)
        {
            GenerateUsing(sectionParser);
            GenerateField(sectionParser, component, level: 2);
            GenerateLoad(sectionParser, component, level: 3);
            GenerateReset(sectionParser, component, level: 3);
            GenerateUpdate(sectionParser, component, level: 3);
            GenerateDraw(sectionParser, component, level: 3);
        }
        public IEnumerable<string> Remove(SectionParser sectionParser, ComponentModel component)
        {
            var removedFields = new List<string>();
            RemoveDraw(sectionParser, component, level: 3);
            RemoveUpdate(sectionParser, component, level: 3);
            RemoveReset(sectionParser, component, level: 3);
            RemoveLoad(sectionParser, component, level: 3);
            var fields = RemoveField(sectionParser, component, 2);
            removedFields.AddRange(fields);
            return removedFields;
        }

        #region Générations
        protected virtual void GenerateUsing(SectionParser sectionParser) { }
        protected virtual void GenerateField(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("FIELDS", [$"{CodeBuilder.Indentation(level)}private {ComponentType} {GetFieldName(component)};"]);
        }
        protected virtual void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void GenerateReset(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level) { }
        public virtual void GenerateUserFileCommentField(SectionParser sectionParser, ComponentModel component)
        {
            sectionParser.InsertBeforeZone("AVAILABLE_FIELDS", [$"{CodeBuilder.Indentation(1)}// [{ComponentType}] {GetFieldName(component)}"], true);
        }
        #endregion

        #region Suppressions
        protected virtual IEnumerable<string> RemoveField(SectionParser sectionParser, ComponentModel component, int level)
        {
            var fieldName = GetFieldName(component);
            sectionParser.RemoveFromZone("FIELDS", line => line == $"{CodeBuilder.Indentation(level)}private {ComponentType} {fieldName};");
            return [fieldName];
        }
        protected virtual void RemoveLoad(SectionParser sectionParser, ComponentModel component, int level) {}
        protected virtual void RemoveReset(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void RemoveUpdate(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void RemoveDraw(SectionParser sectionParser, ComponentModel component, int level) { }
        public virtual void RemoveUserFileCommentField(SectionParser sectionParser, ComponentModel component)
        {
            sectionParser.RemoveFromZone("AVAILABLE_FIELDS", line => line == $"{CodeBuilder.Indentation(1)}// [{ComponentType}] {GetFieldName(component)}");
        }
        #endregion


    }
}
