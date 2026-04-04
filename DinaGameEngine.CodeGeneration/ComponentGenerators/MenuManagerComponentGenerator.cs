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

            foreach (var subComponent in component.SubComponents.Where(c => c.Type == "MenuTitle"))
            {
                sectionParser.InsertBeforeZone("FIELDS",
                                   [CodeBuilder.AddLine($"private IText _{component.Key}_{subComponent.Key}{subComponent.Type};", level)],
                                   checkExistingLines: true);
            }

            foreach (var subComponent in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                sectionParser.InsertBeforeZone("FIELDS",
                                   [CodeBuilder.AddLine($"private {subComponent.Type} _{component.Key}_{subComponent.Key}{subComponent.Type};", level)],
                                   checkExistingLines: true);
            }
        }
        protected override void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertBeforeZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{GetFieldName(component)} = new {ComponentType}();", level)]);

            foreach (var menuTitle in component.SubComponents.Where(c => c.Type == "MenuTitle"))
            {
                var fontFieldName = $"_{component.Key}_{menuTitle.Key}Font";
                var menuTitleFieldName = $"_{component.Key}_{menuTitle.Key}{menuTitle.Type}";

                var titleLine = $"var {menuTitleFieldName} = {GetFieldName(component)}.AddTitle({fontFieldName}, {GetStringProperty(menuTitle, "Content")}, PaletteColors.{GetStringProperty(menuTitle, "Color")}";

                if (menuTitle.Properties.TryGetValue("ShadowColor", out var shadowColor) && menuTitle.Properties.TryGetValue("ShadowOffset", out var shadowOffset))
                {
                    var shadowColorValue = GetStringProperty(menuTitle, "ShadowColor");
                    (int? offsetX, int? offsetY) = GetPointProperty(menuTitle, "ShadowOffset");
                    titleLine += $", PaletteColors.{shadowColorValue}, new Vector2({offsetX ?? 0}, {offsetY ?? 0})";
                }
                titleLine += ");";

                sectionParser.InsertBeforeZone("COMPONENT_LOAD",
                    [
                        CodeBuilder.AddLine($"var {fontFieldName} = _fontManager.Load(FontKeys.{GetStringProperty(menuTitle, "Font")});", level),
                        CodeBuilder.AddLine(titleLine, level)
                    ]);
                AddVector2PropertyToLoad(menuTitle, "Position", sectionParser, menuTitleFieldName, level);
                AddVector2PropertyToLoad(menuTitle, "Dimensions", sectionParser, menuTitleFieldName, level);
            }


            foreach (var menuItem in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var fontFieldName = $"_{component.Key}_{menuItem.Key}Font";
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}{menuItem.Type}";
                sectionParser.InsertBeforeZone("COMPONENT_LOAD",
                    [
                        CodeBuilder.AddLine($"var {fontFieldName} = _fontManager.Load(FontKeys.{GetStringProperty(menuItem, "Font")});", level),
                        CodeBuilder.AddLine($"_{menuItemFieldName} = {GetFieldName(component)}.AddItem({fontFieldName}, \"{GetStringProperty(menuItem, "Content")}\", " +
                                            $"PaletteColors.{GetStringProperty(menuItem, "Color")}, {menuItemFieldName}Selection, {menuItemFieldName}Deselection, {menuItemFieldName}Activation);", level)
                    ]);
                var stateValue = GetStringProperty(menuItem, "State");
                if (!string.IsNullOrEmpty(stateValue) && stateValue != "Enable")
                {
                    sectionParser.AddUsingIfMissing("DinaCSharp.Enums");
                    sectionParser.InsertBeforeZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"_{menuItemFieldName}.State = MenuItemState.{stateValue};", level)]);
                }

                AddVector2PropertyToLoad(menuItem, "Position", sectionParser, $"_{menuItemFieldName}", level);
                AddVector2PropertyToLoad(menuItem, "Dimensions", sectionParser, $"_{menuItemFieldName}", level);

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
            foreach (var menuItem in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}{menuItem.Type}";
                sectionParser.InsertBeforeZone("PARTIAL_METHODS",
                [
                    CodeBuilder.AddLine($"private partial {menuItem.Type} {menuItemFieldName}Selection({menuItem.Type} menuItem);", level),
                    CodeBuilder.AddLine($"private partial {menuItem.Type} {menuItemFieldName}Deselection({menuItem.Type} menuItem);", level),
                    CodeBuilder.AddLine($"private partial {menuItem.Type} {menuItemFieldName}Activation({menuItem.Type} menuItem);", level)
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
            foreach (var subComponent in component.SubComponents)
            {
                var menuItemFieldName = $"_{component.Key}_{subComponent.Key}{subComponent.Type}";
                sectionParser.InsertBeforeZone("AVAILABLE_FIELDS", [CodeBuilder.AddLine($"// [{subComponent.Type}] {menuItemFieldName}", level)], true);
            }
        }
        protected override void GenerateUserFilePartialFunctions(SectionParser sectionParser, ComponentModel component, int level)
        {
            foreach (var menuItem in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}{menuItem.Type}";

                if (!sectionParser.IsPartialFunctionExisting($"{menuItemFieldName}Selection"))
                {
                    sectionParser.InsertBeforeZone("PARTIAL_METHODS",
                    [
                        CodeBuilder.OpenBlock($"private partial {menuItem.Type} {menuItemFieldName}Selection({menuItem.Type} menuItem)", level),
                        CodeBuilder.AddLine($"return menuItem;", level + 1),
                        CodeBuilder.CloseBlock(level)
                    ]);
                }
                if (!sectionParser.IsPartialFunctionExisting($"{menuItemFieldName}Deselection"))
                {
                    sectionParser.InsertBeforeZone("PARTIAL_METHODS",
                    [
                        CodeBuilder.OpenBlock($"private partial {menuItem.Type} {menuItemFieldName}Deselection({menuItem.Type} menuItem)", level),
                        CodeBuilder.AddLine($"return menuItem;", level + 1),
                        CodeBuilder.CloseBlock(level),
                    ]);
                }
                if (!sectionParser.IsPartialFunctionExisting($"{menuItemFieldName}Activation"))
                {
                    sectionParser.InsertBeforeZone("PARTIAL_METHODS",
                    [
                        CodeBuilder.OpenBlock($"private partial {menuItem.Type} {menuItemFieldName}Activation({menuItem.Type} menuItem)", level),
                        CodeBuilder.AddLine($"return menuItem;", level + 1),
                        CodeBuilder.CloseBlock(level),
                    ]);
                }
            }
        }
        protected override IEnumerable<string> RemoveField(SectionParser sectionParser, ComponentModel component, int level)
        {
            var fields = base.RemoveField(sectionParser, component, level).ToList();

            if (!sectionParser.ContainsOutsideZone("FIELDS", "_fontManager"))
                sectionParser.RemoveField("_fontManager");
            fields.Add("_fontManager");

            foreach (var subComponent in component.SubComponents.Where(c => c.Type == "MenuTitle"))
            {
                var menuItemFieldName = $"_{component.Key}_{subComponent.Key}{subComponent.Type}";
                sectionParser.RemoveField(menuItemFieldName);
                sectionParser.RemoveFromZone("FIELDS", line => line == CodeBuilder.AddLine($"private IText {menuItemFieldName};", level));
                fields.Add(menuItemFieldName);
            }
            foreach (var subComponent in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var menuItemFieldName = $"_{component.Key}_{subComponent.Key}{subComponent.Type}";
                sectionParser.RemoveField(menuItemFieldName);
                sectionParser.RemoveFromZone("FIELDS", line => line == CodeBuilder.AddLine($"private {subComponent.Type} {menuItemFieldName};", level));
                fields.Add(menuItemFieldName);
            }
            return fields;
        }
        protected override void RemoveLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.RemoveFromZone("COMPONENT_LOAD", $"{GetFieldName(component)}.SetActionKeys");
            foreach (var subComponent in component.SubComponents)
            {
                sectionParser.RemoveFromZone("COMPONENT_LOAD", $"_{component.Key}_{subComponent.Key}Font");
                sectionParser.RemoveFromZone("COMPONENT_LOAD", $"_{component.Key}_{subComponent.Key}{subComponent.Type}");
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
            foreach (var menuItem in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}{menuItem.Type}";
                sectionParser.RemoveFromZone("PARTIAL_METHODS", $"{menuItem.Type} {menuItemFieldName}Selection");
                sectionParser.RemoveFromZone("PARTIAL_METHODS", $"{menuItem.Type} {menuItemFieldName}Deselection");
                sectionParser.RemoveFromZone("PARTIAL_METHODS", $"{menuItem.Type} {menuItemFieldName}Activation");
            }
        }
        protected override void RemoveUserFileCommentField(SectionParser sectionParser, ComponentModel component)
        {
            foreach (var menuItem in component.SubComponents)
                sectionParser.RemoveFromZone("AVAILABLE_FIELDS", $" _{component.Key}_{menuItem.Key}{menuItem.Type}");
            if (!sectionParser.ContainsOutsideZone("AVAILABLE_FIELDS", "_fontManager"))
                sectionParser.RemoveFromZone("AVAILABLE_FIELDS", "_fontManager");
            base.RemoveUserFileCommentField(sectionParser, component);
        }
        protected override void RemoveUserFilePartialFunctions(SectionParser sectionParser, ComponentModel component)
        {
            foreach (var menuItem in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}{menuItem.Type}";
                sectionParser.RemovePartialFunction($"MenuItem {menuItemFieldName}Selection");
                sectionParser.RemovePartialFunction($"MenuItem {menuItemFieldName}Deselection");
                sectionParser.RemovePartialFunction($"MenuItem {menuItemFieldName}Activation");
            }
        }
    }
}
