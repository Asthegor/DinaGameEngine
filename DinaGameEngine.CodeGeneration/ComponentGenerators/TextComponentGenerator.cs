using DinaGameEngine.Models.Project;

namespace DinaGameEngine.CodeGeneration.ComponentGenerators
{
    public class TextComponentGenerator : IComponentGenerator
    {
        public string ComponentType => "Text";
        public void GenerateField(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("FIELDS", [$"{CodeBuilder.Indentation(level)}private Text _{component.Key}Text;"]);
        }
        public void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            var font = component.Properties["Font"];
            var content = component.Properties["Content"];
            var colorKey = component.Properties["Color"];
            sectionParser.InsertBeforeZone("COMPONENT_LOAD",
                [
                    $"{CodeBuilder.Indentation(level)}var {component.Key}Font = _fontManager.Load(FontKeys.{font});",
                    $"{CodeBuilder.Indentation(level)}_{component.Key}Text = new Text({component.Key}Font, {content}, PaletteColors.{colorKey});"
                ]);

            var excludedKeys = new[] { "Font", "Content", "Color" };
            foreach (var property in component.Properties)
            {
                if (!excludedKeys.Contains(property.Key))
                    sectionParser.InsertBeforeZone("COMPONENT_LOAD", [$"{CodeBuilder.Indentation(level)}_{component.Key}Text.{property.Key} = {property.Value};"]);
            }
        }
        public void GenerateLoadUser(SectionParser sectionParser, ComponentModel component, int level)
        {
        }
        public void GenerateReset(SectionParser sectionParser, ComponentModel component, int level)
        {
        }
        public void GenerateResetUser(SectionParser sectionParser, ComponentModel component, int level)
        {
        }
        public void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_UPDATE", [$"{CodeBuilder.Indentation(level)}_{component.Key}Text.Update(gameTime);"]);
        }
        public void GenerateUpdateUser(SectionParser sectionParser, ComponentModel component, int level)
        {
        }
        public void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_DRAW", [$"{CodeBuilder.Indentation(level)}_{component.Key}Text.Draw(spriteBatch);"]);
        }
        public void GenerateDrawUser(SectionParser sectionParser, ComponentModel component, int level)
        {
        }
    }
}
