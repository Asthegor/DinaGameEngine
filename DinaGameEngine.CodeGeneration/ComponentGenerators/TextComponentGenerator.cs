using DinaGameEngine.Models.Helpers;
using DinaGameEngine.Models.Project;

using System.Drawing;
using System.Text.Json;

namespace DinaGameEngine.CodeGeneration.ComponentGenerators
{
    public class TextComponentGenerator : ComponentGenerator, IComponentGenerator
    {
        public override string ComponentType => "Text";


        protected override void GenerateUsing(SectionParser sectionParser, string rootnamespace)
        {
            sectionParser.AddUsingIfMissing("DinaCSharp.Graphics");
            sectionParser.AddUsingIfMissing("DinaCSharp.Services");
            sectionParser.AddUsingIfMissing("DinaCSharp.Services.Fonts");
        }
        protected override void GenerateField(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("FIELDS",
                                           [CodeBuilder.AddLine($"private FontManager _fontManager = ServiceLocator.Get<FontManager>(DinaServiceKeys.FontManager);", level)],
                                           checkExistingLines: true);
            base.GenerateField(sectionParser, component, level);
        }
        protected override void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            var font = ComponentPropertyHelper.GetStringProperty(component, "Font");
            var content = ComponentPropertyHelper.GetStringProperty(component, "Content");
            var colorKey = ComponentPropertyHelper.GetStringProperty(component, "Color");
            sectionParser.InsertBeforeZone("COMPONENT_LOAD",
                [
                    CodeBuilder.AddLine($"var {component.Key}Font = _fontManager.Load(FontKeys.{font});", level),
                    CodeBuilder.AddLine($"{GetFieldName(component)} = new {ComponentType}({component.Key}Font, \"{content}\", PaletteColors.{colorKey});", level)
                ]);

            AddVector2PropertyToLoad(component, "Position", sectionParser, GetFieldName(component), level);
            AddVector2PropertyToLoad(component, "Dimensions", sectionParser, GetFieldName(component), level);

            var visibleValue = ComponentPropertyHelper.GetStringProperty(component, "Visible");
            if (!string.IsNullOrEmpty(visibleValue))
                sectionParser.InsertBeforeZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{GetFieldName(component)}.Visible = {visibleValue.ToLower()};", level)]);

            var hAlign = ComponentPropertyHelper.GetStringProperty(component, "HorizontalAlignment");
            var vAlign = ComponentPropertyHelper.GetStringProperty(component, "VerticalAlignment");
            if (!string.IsNullOrEmpty(hAlign) || !string.IsNullOrEmpty(vAlign))
            {
                sectionParser.AddUsingIfMissing("DinaCSharp.Enums");
                var h = string.IsNullOrEmpty(hAlign) ? "Left" : hAlign;
                var v = string.IsNullOrEmpty(vAlign) ? "Top" : vAlign;
                sectionParser.InsertBeforeZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{GetFieldName(component)}.SetAlignments(HorizontalAlignment.{h}, VerticalAlignment.{v});", level)]);
            }

            var excludedKeys = new[] { "Font", "Content", "Color", "Position", "Dimensions", "Visible",
                                       "HorizontalAlignment", "VerticalAlignment" };
            foreach (var property in component.Properties)
            {
                if (!excludedKeys.Contains(property.Key))
                    sectionParser.InsertBeforeZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{GetFieldName(component)}.{property.Key} = {property.Value};", level)]);
            }
        }
        protected override void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_UPDATE", [CodeBuilder.AddLine($"{GetFieldName(component)}.Update(gameTime);", level)]);
        }
        protected override void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_DRAW", [CodeBuilder.AddLine($"{GetFieldName(component)}.Draw(spriteBatch);", level)]);
        }

        protected override void GenerateUserFileCommentField(SectionParser sectionParser, ComponentModel component, int level)
        {
            base.GenerateUserFileCommentField(sectionParser, component, level);
            sectionParser.InsertBeforeZone("AVAILABLE_FIELDS", [CodeBuilder.AddLine($"// [FontManager] _fontManager", level)], true);
        }

        protected override IEnumerable<string> RemoveField(SectionParser sectionParser, ComponentModel component, int level)
        {
            var fields = base.RemoveField(sectionParser, component, level).ToList();
            if (!sectionParser.ContainsOutsideZone("FIELDS", "_fontManager"))
                sectionParser.RemoveField("_fontManager");
            fields.Add("_fontManager");
            return fields;
        }
        protected override void RemoveLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            var font = ComponentPropertyHelper.GetStringProperty(component, "Font");
            var content = ComponentPropertyHelper.GetStringProperty(component, "Content");
            var colorKey = ComponentPropertyHelper.GetStringProperty(component, "Color");
            sectionParser.RemoveFromZone("COMPONENT_LOAD", line => line == CodeBuilder.AddLine($"var {component.Key}Font = _fontManager.Load(FontKeys.{font});", level));
            sectionParser.RemoveFromZone("COMPONENT_LOAD", line => line == CodeBuilder.AddLine($"{GetFieldName(component)} = new {ComponentType}({component.Key}Font, \"{content}\", PaletteColors.{colorKey});", level));

            var (px, py) = ComponentPropertyHelper.GetPointProperty(component, "Position");
            if (px != null && py != null)
                sectionParser.RemoveFromZone("COMPONENT_LOAD", CodeBuilder.AddLine($"{GetFieldName(component)}.Position = new Vector2({px}f, {py}f);", level));

            var (dx, dy) = ComponentPropertyHelper.GetPointProperty(component, "Dimensions");
            if (dx != null && dy != null)
                sectionParser.RemoveFromZone("COMPONENT_LOAD", CodeBuilder.AddLine($"{GetFieldName(component)}.Dimensions = new Vector2({dx}f, {dy}f);", level));

            var excludedKeys = new[] { "Font", "Content", "Color", "Position", "Dimensions" };
            foreach (var property in component.Properties)
            {
                if (!excludedKeys.Contains(property.Key))
                    sectionParser.RemoveFromZone("COMPONENT_LOAD", CodeBuilder.AddLine($"{GetFieldName(component)}.{property.Key} = {property.Value};", level));
            }

        }
        protected override void RemoveUpdate(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_UPDATE", line => line == CodeBuilder.AddLine($"{GetFieldName(component)}.Update(gameTime);", level));
        }
        protected override void RemoveDraw(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_DRAW", line => line == CodeBuilder.AddLine($"{GetFieldName(component)}.Draw(spriteBatch);", level));
        }
        protected override void RemoveUserFileCommentField(SectionParser sectionParser, ComponentModel component)
        {
            if (!sectionParser.ContainsOutsideZone("AVAILABLE_FIELDS", "_fontManager"))
                sectionParser.RemoveFromZone("AVAILABLE_FIELDS", "_fontManager");
            base.RemoveUserFileCommentField(sectionParser, component);
        }
    }
}
