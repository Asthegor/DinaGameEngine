using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

using System.Drawing;
using System.Text.Json;

namespace DinaGameEngine.CodeGeneration.ComponentGenerators
{
    public abstract class ComponentGenerator
    {
        public abstract string ComponentType { get; }
        public string GetFieldName(ComponentModel component) => $"_{component.Key}{ComponentType}";

        public void AddToDesigner(SectionParser sectionParser, ComponentModel component, string rootNamespace)
        {
            GenerateUsing(sectionParser, rootNamespace);
            GenerateField(sectionParser, component, level: 2);
            GenerateLoad(sectionParser, component, level: 3);
            GenerateReset(sectionParser, component, level: 3);
            GenerateUpdate(sectionParser, component, level: 3);
            GenerateDraw(sectionParser, component, level: 3);
            GeneratePartialFunctions(sectionParser, component, level: 2);
        }
        #region Générations pour le Designer
        protected virtual void GenerateUsing(SectionParser sectionParser, string rootNamespace) { }
        protected virtual void GenerateField(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("FIELDS", [CodeBuilder.AddLine($"private {ComponentType} {GetFieldName(component)};", level)]);
        }
        protected virtual void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void GenerateReset(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void GeneratePartialFunctions(SectionParser sectionParser, ComponentModel component, int level) { }
        #endregion

        public virtual void AddToUserFile(SectionParser sectionParser, ComponentModel component)
        {
            GenerateUserFileUsings(sectionParser, component);
            GenerateUserFileCommentField(sectionParser, component, level: 1);
            GenerateUserFilePartialFunctions(sectionParser, component, level: 2);
        }
        #region Générations pour UserFile
        protected virtual void GenerateUserFileUsings(SectionParser sectionParser, ComponentModel component) { }
        protected virtual void GenerateUserFileCommentField(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("AVAILABLE_FIELDS", [CodeBuilder.AddLine($"// [{ComponentType}] {GetFieldName(component)}", level)], true);
        }
        protected virtual void GenerateUserFilePartialFunctions(SectionParser sectionParser, ComponentModel component, int level) { }
        #endregion


        public IEnumerable<string> RemoveFromDesigner(SectionParser sectionParser, ComponentModel component)
        {
            var removedFields = new List<string>();
            RemovePartialFunctions(sectionParser, component, level: 2);
            RemoveDraw(sectionParser, component, level: 3);
            RemoveUpdate(sectionParser, component, level: 3);
            RemoveReset(sectionParser, component, level: 3);
            RemoveLoad(sectionParser, component, level: 3);
            var fields = RemoveField(sectionParser, component, level: 2);
            removedFields.AddRange(fields);
            return [.. removedFields];
        }
        #region Suppressions du Designer
        protected virtual IEnumerable<string> RemoveField(SectionParser sectionParser, ComponentModel component, int level)
        {
            var fieldName = GetFieldName(component);
            sectionParser.RemoveFromZone("FIELDS", line => line == CodeBuilder.AddLine($"private {ComponentType} {fieldName};", level));
            return [fieldName];
        }
        protected virtual void RemoveLoad(SectionParser sectionParser, ComponentModel component, int level) {}
        protected virtual void RemoveReset(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void RemoveUpdate(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void RemoveDraw(SectionParser sectionParser, ComponentModel component, int level) { }
        protected virtual void RemovePartialFunctions(SectionParser sectionParser, ComponentModel component, int level) { }
        #endregion

        public virtual void RemoveFromUserFile(SectionParser sectionParser, ComponentModel component)
        {
            RemoveUserFileCommentField(sectionParser, component);
            RemoveUserFilePartialFunctions(sectionParser, component);
        }
        #region Suppression du UserFile
        protected virtual void RemoveUserFileCommentField(SectionParser sectionParser, ComponentModel component)
        {
            sectionParser.RemoveFromZone("AVAILABLE_FIELDS", GetFieldName(component));
        }
        protected virtual void RemoveUserFilePartialFunctions(SectionParser sectionParser, ComponentModel component) { }
        #endregion

        #region Utils
        protected static void AddVector2PropertyToLoad(ComponentModel component, string propertyName, SectionParser sectionParser, string componentFieldName, int level)
        {
            (int? x, int? y) = ComponentPropertyHelper.GetPointProperty(component, propertyName);
            if (x == null || y == null)
                return;
            sectionParser.InsertBeforeZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{componentFieldName}.{propertyName} = new Vector2({x}f, {y}f);", level)]);
        }

        #endregion
    }
}
