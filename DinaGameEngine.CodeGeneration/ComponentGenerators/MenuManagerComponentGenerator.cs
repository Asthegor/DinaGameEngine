using DinaGameEngine.Models.Project;

namespace DinaGameEngine.CodeGeneration.ComponentGenerators
{
    public class MenuManagerComponentGenerator : ComponentGenerator, IComponentGenerator
    {
        public override string ComponentType => "MenuManager";

        protected override void GenerateUsing(SectionParser sectionParser, string rootNamespace)
        {
            sectionParser.AddUsingIfMissing($"{rootNamespace}.Core.Keys");
            sectionParser.AddUsingIfMissing("DinaCSharp.Enums");
            sectionParser.AddUsingIfMissing("DinaCSharp.Services");
            sectionParser.AddUsingIfMissing("DinaCSharp.Services.Fonts");
            sectionParser.AddUsingIfMissing("DinaCSharp.Services.Menus");
        }
        protected override void GenerateField(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("FIELDS",
                               [CodeBuilder.AddLine($"private FontManager _fontManager = ServiceLocator.Get<FontManager>(DinaServiceKeys.FontManager);", level)],
                               checkExistingLines: true);
            base.GenerateField(sectionParser, component, level);

            foreach (var menuItem in component.MenuItems)
            {
                sectionParser.InsertBeforeZone("FIELDS",
                                   [CodeBuilder.AddLine($"private MenuItem _{component.Key}_{menuItem.Key}MenuItem;", level)],
                                   checkExistingLines: true);
            }
        }
        protected override void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{GetFieldName(component)} = new {ComponentType}();", level)]);
            foreach (var menuItem in component.MenuItems)
            {
                var fontFieldName = $"_{component.Key}_{menuItem.Key}Font";
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}MenuItem";
                sectionParser.InsertBeforeZone("COMPONENT_LOAD",
                    [
                        CodeBuilder.AddLine($"var {fontFieldName} = _fontManager.Load(FontKeys.{menuItem.Font});", level),
                        CodeBuilder.AddLine($"_{menuItemFieldName} = {GetFieldName(component)}.AddItem({fontFieldName}, \"{menuItem.Content}\", " +
                                            $"PaletteColors.{menuItem.Color}, {menuItemFieldName}Selection, {menuItemFieldName}Deselection, {menuItemFieldName}Activation);", level)
                    ]);
                if (!string.IsNullOrEmpty(menuItem.State) && menuItem.State != "Enable")
                    sectionParser.InsertBeforeZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{menuItemFieldName}.State = MenuItemState.{menuItem.State};", level)]);
            }
            sectionParser.InsertBeforeZone("COMPONENT_LOAD",
                                           [ CodeBuilder.AddLine($"{GetFieldName(component)}.SetActionKeys((MenuAction.Up, PlayerInputKeys.Up), " +
                                                                     "(MenuAction.Down, PlayerInputKeys.Down), " +
                                                                     "(MenuAction.Left, PlayerInputKeys.Left), " +
                                                                     "(MenuAction.Right, PlayerInputKeys.Right), " +
                                                                     "(MenuAction.Activate, PlayerInputKeys.Activate), " +
                                                                     "(MenuAction.Cancel, PlayerInputKeys.Cancel));",
                                                                 level)
                                           ]);
        }
        protected override void GenerateReset(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_RESET", [CodeBuilder.AddLine($"{GetFieldName(component)}?.Reset();", level)]);
        }
        protected override void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_UPDATE", [CodeBuilder.AddLine($"{GetFieldName(component)}?.Update(gameTime);", level)]);
        }
        protected override void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_DRAW", [CodeBuilder.AddLine($"{GetFieldName(component)}?.Draw(spriteBatch);", level)]);
        }
        protected override void GeneratePartialFunctions(SectionParser sectionParser, ComponentModel component, int level)
        {
            foreach (var menuItem in component.MenuItems)
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}MenuItem";
                sectionParser.InsertBeforeZone("PARTIAL_METHODS",
                [
                    CodeBuilder.AddLine($"private partial MenuItem {menuItemFieldName}Selection(MenuItem menuItem);", level),
                    CodeBuilder.AddLine($"private partial MenuItem {menuItemFieldName}Deselection(MenuItem menuItem);", level),
                    CodeBuilder.AddLine($"private partial MenuItem {menuItemFieldName}Activation(MenuItem menuItem);", level)
                ]);
            }
        }

        protected override void GenerateUserFileUsings(SectionParser sectionParser, ComponentModel component)
        {
            sectionParser.AddUsingIfMissing("DinaCSharp.Services.Menus");
        }
        protected override void GenerateUserFileCommentField(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("AVAILABLE_FIELDS", [CodeBuilder.AddLine($"// [FontManager] _fontManager", level)], true);
            base.GenerateUserFileCommentField(sectionParser, component, level);
            foreach (var menuItem in component.MenuItems)
            {
                var menuItemFieldName = $"_{component.Key}_{menuItem.Key}MenuItem";
                sectionParser.InsertBeforeZone("AVAILABLE_FIELDS", [CodeBuilder.AddLine($"// [MenuItem] {menuItemFieldName}", level)], true);
            }
        }
        protected override void GenerateUserFilePartialFunctions(SectionParser sectionParser, ComponentModel component, int level)
        {
            foreach (var menuItem in component.MenuItems)
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}MenuItem";
                sectionParser.InsertBeforeZone("PARTIAL_METHODS",
                [
                    CodeBuilder.OpenBlock($"private partial MenuItem {menuItemFieldName}Selection(MenuItem menuItem)", level),
                    CodeBuilder.AddLine($"return menuItem;", level + 1),
                    CodeBuilder.CloseBlock(level),
                    CodeBuilder.OpenBlock($"private partial MenuItem {menuItemFieldName}Deselection(MenuItem menuItem)", level),
                    CodeBuilder.AddLine($"return menuItem;", level + 1),
                    CodeBuilder.CloseBlock(level),
                    CodeBuilder.OpenBlock($"private partial MenuItem {menuItemFieldName}Activation(MenuItem menuItem)", level),
                    CodeBuilder.AddLine($"return menuItem;", level + 1),
                    CodeBuilder.CloseBlock(level),
                ]);
            }
        }
        protected override IEnumerable<string> RemoveField(SectionParser sectionParser, ComponentModel component, int level)
        {
            var fields = base.RemoveField(sectionParser, component, level).ToList();

            if (!sectionParser.ContainsOutsideZone("FIELDS", "_fontManager"))
                sectionParser.RemoveField("_fontManager");
            fields.Add("_fontManager");

            foreach (var menuItem in component.MenuItems)
            {
                var menuItemFieldName = $"_{component.Key}_{menuItem.Key}MenuItem";
                sectionParser.RemoveField(menuItemFieldName);
                sectionParser.RemoveFromZone("FIELDS", line => line == CodeBuilder.AddLine($"private MenuItem {menuItemFieldName};", level));
                fields.Add(menuItemFieldName);
            }
            return fields;
        }
        protected override void RemoveLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_LOAD", $"{GetFieldName(component)}.SetActionKeys");
            foreach (var menuItem in component.MenuItems)
            {
                sectionParser.RemoveFromZone("COMPONENT_LOAD", $"_{component.Key}_{menuItem.Key}Font");
                sectionParser.RemoveFromZone("COMPONENT_LOAD", $"_{component.Key}_{menuItem.Key}MenuItem");
            }
            sectionParser.RemoveFromZone("COMPONENT_LOAD", $"{GetFieldName(component)}");
        }
        protected override void RemoveReset(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_RESET", $"{GetFieldName(component)}");
        }
        protected override void RemoveUpdate(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_UPDATE", $"{GetFieldName(component)}");
        }
        protected override void RemoveDraw(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_DRAW", $"{GetFieldName(component)}");
        }
        protected override void RemovePartialFunctions(SectionParser sectionParser, ComponentModel component, int level)
        {
            foreach (var menuItem in component.MenuItems)
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}MenuItem";
                sectionParser.RemoveFromZone("PARTIAL_METHODS", $"MenuItem {menuItemFieldName}Selection");
                sectionParser.RemoveFromZone("PARTIAL_METHODS", $"MenuItem {menuItemFieldName}Deselection");
                sectionParser.RemoveFromZone("PARTIAL_METHODS", $"MenuItem {menuItemFieldName}Activation");
            }
        }
        protected override void RemoveUserFileCommentField(SectionParser sectionParser, ComponentModel component)
        {
            foreach (var menuItem in component.MenuItems)
                sectionParser.RemoveFromZone("AVAILABLE_FIELDS", $" _{component.Key}_{menuItem.Key}MenuItem");
            if (!sectionParser.ContainsOutsideZone("AVAILABLE_FIELDS", "_fontManager"))
                sectionParser.RemoveFromZone("AVAILABLE_FIELDS", "_fontManager");
            base.RemoveUserFileCommentField(sectionParser, component);
        }
        protected override void RemoveUserFilePartialFunctions(SectionParser sectionParser, ComponentModel component)
        {
            foreach (var menuItem in component.MenuItems)
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}MenuItem";
                sectionParser.RemovePartialFunction($"MenuItem {menuItemFieldName}Selection");
                sectionParser.RemovePartialFunction($"MenuItem {menuItemFieldName}Deselection");
                sectionParser.RemovePartialFunction($"MenuItem {menuItemFieldName}Activation");
            }
        }
    }
}
