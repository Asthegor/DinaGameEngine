using DinaGameEngine.Models.Project;

namespace DinaGameEngine.CodeGeneration.ComponentGenerators
{
    public class TextComponentGenerator : ComponentGenerator, IComponentGenerator
    {
        public override string ComponentType => "Text";


        protected override void GenerateUsing(SectionParser sectionParser)
        {
            sectionParser.AddUsingIfMissing("DinaCSharp.Graphics");
            sectionParser.AddUsingIfMissing("DinaCSharp.Services");
            sectionParser.AddUsingIfMissing("DinaCSharp.Services.Fonts");
        }
        protected override void GenerateField(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("FIELDS",
                                           [$"{CodeBuilder.Indentation(level)}private FontManager _fontManager = ServiceLocator.Get<FontManager>(DinaServiceKeys.FontManager);"],
                                           checkExistingLines: true);
            base.GenerateField(sectionParser, component, level);
        }
        protected override void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            var font = component.Properties["Font"];
            var content = component.Properties["Content"];
            var colorKey = component.Properties["Color"];
            sectionParser.InsertBeforeZone("COMPONENT_LOAD",
                [
                    $"{CodeBuilder.Indentation(level)}var {component.Key}Font = _fontManager.Load(FontKeys.{font});",
                    $"{CodeBuilder.Indentation(level)}{GetFieldName(component)} = new {ComponentType}({component.Key}Font, \"{content}\", PaletteColors.{colorKey});"
                ]);

            var excludedKeys = new[] { "Font", "Content", "Color" };
            foreach (var property in component.Properties)
            {
                if (!excludedKeys.Contains(property.Key))
                    sectionParser.InsertBeforeZone("COMPONENT_LOAD", [$"{CodeBuilder.Indentation(level)}{GetFieldName(component)}.{property.Key} = {property.Value};"]);
            }
        }
        protected override void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_UPDATE", [$"{CodeBuilder.Indentation(level)}{GetFieldName(component)}.Update(gameTime);"]);
        }
        protected override void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_DRAW", [$"{CodeBuilder.Indentation(level)}{GetFieldName(component)}.Draw(spriteBatch);"]);
        }

        protected override IEnumerable<string> RemoveField(SectionParser sectionParser, ComponentModel component, int level)
        {
            var fields = base.RemoveField(sectionParser, component, level).ToList();
            sectionParser.RemoveField("_fontManager");
            fields.Add("_fontManager");
            return fields;
        }
        protected override void RemoveLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            var font = component.Properties["Font"];
            var content = component.Properties["Content"];
            var colorKey = component.Properties["Color"];
            sectionParser.RemoveFromZone("COMPONENT_LOAD", line => line == $"{CodeBuilder.Indentation(level)}var {component.Key}Font = _fontManager.Load(FontKeys.{font});");
            sectionParser.RemoveFromZone("COMPONENT_LOAD", line => line == $"{CodeBuilder.Indentation(level)}{GetFieldName(component)} = new {ComponentType}({component.Key}Font, \"{content}\", PaletteColors.{colorKey});");

            var excludedKeys = new[] { "Font", "Content", "Color" };
            foreach (var property in component.Properties)
            {
                if (!excludedKeys.Contains(property.Key))
                    sectionParser.RemoveFromZone("COMPONENT_LOAD", $"{CodeBuilder.Indentation(level)}{GetFieldName(component)}.{property.Key} = {property.Value};");
            }

        }
        protected override void RemoveUpdate(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_UPDATE", line => line == $"{CodeBuilder.Indentation(level)}{GetFieldName(component)}.Update(gameTime);");
        }
        protected override void RemoveDraw(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_DRAW", line => line == $"{CodeBuilder.Indentation(level)}{GetFieldName(component)}.Draw(spriteBatch);");
        }
    }
}
