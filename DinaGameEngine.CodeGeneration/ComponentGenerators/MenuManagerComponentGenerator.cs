using DinaGameEngine.Models.Helpers;
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
            sectionParser.InsertIntoZone("FIELDS",
                               [CodeBuilder.AddLine($"private FontManager _fontManager = ServiceLocator.Get<FontManager>(DinaServiceKeys.FontManager);", level)],
                               checkExistingLines: true);
            base.GenerateField(sectionParser, component, level);

            foreach (var subComponent in component.SubComponents.Where(c => c.Type == "MenuTitle"))
            {
                sectionParser.InsertIntoZone("FIELDS",
                                   [CodeBuilder.AddLine($"private IText _{component.Key}_{subComponent.Key}{subComponent.Type};", level)],
                                   checkExistingLines: true);
            }

            foreach (var subComponent in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                sectionParser.InsertIntoZone("FIELDS",
                                   [CodeBuilder.AddLine($"private {subComponent.Type} _{component.Key}_{subComponent.Key}{subComponent.Type};", level)],
                                   checkExistingLines: true);
            }
        }
        protected override void GenerateLoad(SectionParser sectionParser, ComponentModel component, int level)
        {
            var args = new List<string>();
            var (sX, sY) = ComponentPropertyHelper.GetPointProperty(component, "ItemSpacing");
            if (sX.HasValue || sY.HasValue)
                args.Add($"itemspacing: new Vector2({sX ?? 0}f, {sY ?? 0}f)");

            var currentIndex = ComponentPropertyHelper.GetIntProperty(component, "CurrentItemIndex", -1);
            if (currentIndex != -1)
                args.Add($"currentitemindex: {currentIndex}");

            var direction = ComponentPropertyHelper.GetStringProperty(component, "Direction");
            if (!string.IsNullOrEmpty(direction))
                args.Add($"direction: MenuItemDirection.{direction}");

            var constructor = $"{GetFieldName(component)} = new {ComponentType}({string.Join(", ", args)});";
            sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine(constructor, level)]);

            foreach (var menuTitle in component.SubComponents.Where(c => c.Type == "MenuTitle"))
            {
                sectionParser.AddUsingIfMissing("DinaCSharp.Interfaces");

                var fontFieldName = $"_{component.Key}_{menuTitle.Key}Font";
                var menuTitleFieldName = $"_{component.Key}_{menuTitle.Key}{menuTitle.Type}";

                var titleLine = $"var {menuTitleFieldName} = {GetFieldName(component)}.AddTitle(font: {fontFieldName}, text: \"{ComponentPropertyHelper.GetStringProperty(menuTitle, "Content")}\", color: PaletteColors.{ComponentPropertyHelper.GetStringProperty(menuTitle, "Color")}";

                if (menuTitle.Properties.TryGetValue("ShadowColor", out var shadowColor) && menuTitle.Properties.TryGetValue("ShadowOffset", out var shadowOffset))
                {
                    var shadowColorValue = ComponentPropertyHelper.GetStringProperty(menuTitle, "ShadowColor");
                    (int? offsetX, int? offsetY) = ComponentPropertyHelper.GetPointProperty(menuTitle, "ShadowOffset");
                    titleLine += $", shadowcolor: PaletteColors.{shadowColorValue}, shadowoffset: new Vector2({offsetX ?? 0}, {offsetY ?? 0})";
                }
                titleLine += ");";

                sectionParser.InsertIntoZone("COMPONENT_LOAD",
                    [
                        CodeBuilder.AddLine($"var {fontFieldName} = _fontManager.Load(FontKeys.{ComponentPropertyHelper.GetStringProperty(menuTitle, "Font")});", level),
                        CodeBuilder.AddLine(titleLine, level)
                    ]);
                AddVector2PropertyToLoad(menuTitle, "Position", sectionParser, menuTitleFieldName, level);
                AddVector2PropertyToLoad(menuTitle, "Dimensions", sectionParser, menuTitleFieldName, level);

                /*
                var hAlign = ComponentPropertyHelper.GetStringProperty(menuTitle, "HAlign");
                var vAlign = ComponentPropertyHelper.GetStringProperty(menuTitle, "VAlign");
                if (!string.IsNullOrEmpty(hAlign) || !string.IsNullOrEmpty(vAlign))
                {
                    sectionParser.AddUsingIfMissing("DinaCSharp.Enums");
                    var h = string.IsNullOrEmpty(hAlign) ? "Left" : hAlign;
                    var v = string.IsNullOrEmpty(vAlign) ? "Top" : vAlign;
                    sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{menuTitleFieldName}.SetAlignments(HorizontalAlignment.{h}, VerticalAlignment.{v});", level)]);
                }
                */
                var zOrder = ComponentPropertyHelper.GetIntProperty(menuTitle, "ZOrder");
                if (zOrder != null && zOrder != 0)
                    sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{menuTitleFieldName}.ZOrder = {zOrder};", level)]);

                var titleVisible = ComponentPropertyHelper.GetBoolProperty(menuTitle, "Visible", true);
                if (!titleVisible)
                    sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{menuTitleFieldName}.Visible = {titleVisible};", level)]);

            }


            foreach (var menuItem in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var fontFieldName = $"_{component.Key}_{menuItem.Key}Font";
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}{menuItem.Type}";
                sectionParser.InsertIntoZone("COMPONENT_LOAD",
                    [
                        CodeBuilder.AddLine($"var {fontFieldName} = _fontManager.Load(FontKeys.{ComponentPropertyHelper.GetStringProperty(menuItem, "Font")});", level),
                        CodeBuilder.AddLine($"_{menuItemFieldName} = {GetFieldName(component)}.AddItem({fontFieldName}, \"{ComponentPropertyHelper.GetStringProperty(menuItem, "Content")}\", " +
                                            $"PaletteColors.{ComponentPropertyHelper.GetStringProperty(menuItem, "Color")}, {menuItemFieldName}Selection, {menuItemFieldName}Deselection, {menuItemFieldName}Activation);", level)
                    ]);
                var stateValue = ComponentPropertyHelper.GetStringProperty(menuItem, "State");
                if (!string.IsNullOrEmpty(stateValue) && stateValue != "Enable")
                {
                    sectionParser.AddUsingIfMissing("DinaCSharp.Enums");
                    sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"_{menuItemFieldName}.State = MenuItemState.{stateValue};", level)]);
                }

                AddVector2PropertyToLoad(menuItem, "Position", sectionParser, $"_{menuItemFieldName}", level);
                AddVector2PropertyToLoad(menuItem, "Dimensions", sectionParser, $"_{menuItemFieldName}", level);

                /*
                var hAlign = ComponentPropertyHelper.GetStringProperty(menuItem, "HAlign");
                var vAlign = ComponentPropertyHelper.GetStringProperty(menuItem, "VAlign");
                if (!string.IsNullOrEmpty(hAlign) || !string.IsNullOrEmpty(vAlign))
                {
                    sectionParser.AddUsingIfMissing("DinaCSharp.Enums");
                    var h = string.IsNullOrEmpty(hAlign) ? "Left" : hAlign;
                    var v = string.IsNullOrEmpty(vAlign) ? "Top" : vAlign;
                    sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"_{menuItemFieldName}.SetAlignments(HorizontalAlignment.{h}, VerticalAlignment.{v});", level)]);
                }
                */
                var zOrder = ComponentPropertyHelper.GetIntProperty(menuItem, "ZOrder");
                if (zOrder != null && zOrder != 0)
                    sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"_{menuItemFieldName}.ZOrder = {zOrder};", level)]);

                var itemVisible = ComponentPropertyHelper.GetBoolProperty(menuItem, "Visible", true);
                if (!itemVisible)
                    sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"_{menuItemFieldName}.Visible = {itemVisible};", level)]);
            }

            sectionParser.InsertIntoZone("COMPONENT_LOAD",
                                           [ CodeBuilder.AddLine($"{GetFieldName(component)}.SetActionKeys((MenuAction.Up, PlayerInputKeys.Up), " +
                                                                     "(MenuAction.Down, PlayerInputKeys.Down), " +
                                                                     "(MenuAction.Left, PlayerInputKeys.Left), " +
                                                                     "(MenuAction.Right, PlayerInputKeys.Right), " +
                                                                     "(MenuAction.Activate, PlayerInputKeys.Activate), " +
                                                                     "(MenuAction.Cancel, PlayerInputKeys.Cancel));",
                                                                 level)
                                           ]);

            var visible = ComponentPropertyHelper.GetBoolProperty(component, "Visible", true);
            if (!visible)
                sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{GetFieldName(component)}.Visible = {visible};", level)]);

            var iconAlignment = ComponentPropertyHelper.GetStringProperty(component, "IconAlignment");
            if (!string.IsNullOrEmpty(iconAlignment) && iconAlignment != "None")
            {
                var argsSetIconItems = new List<string>();
                
                var iconLeftKey = ComponentPropertyHelper.GetStringProperty(component, "IconLeftTexture");
                if (!string.IsNullOrEmpty (iconLeftKey))
                    argsSetIconItems.Add($"iconLeftKey: {iconLeftKey}");

                var iconRightKey = ComponentPropertyHelper.GetStringProperty(component, "IconRightTexture");
                if (!string.IsNullOrEmpty (iconRightKey))
                    argsSetIconItems.Add($"iconRightKey: {iconRightKey}");
                
                var (iconSpacingX, iconSpacingY) = ComponentPropertyHelper.GetPointProperty(component, "IconSpacing");
                if (iconSpacingX.HasValue || iconSpacingY.HasValue)
                    argsSetIconItems.Add($"iconSpacing: new Vector2({iconSpacingX ?? 0}f, {iconSpacingY ?? 0}f)");
                
                var iconResize = ComponentPropertyHelper.GetBoolProperty(component, "IconResize", false);
                if (iconResize)
                    argsSetIconItems.Add($"resize: {iconResize}");

                var setIconItems = $"{GetFieldName(component)}.SetIconItems({string.Join(", ", args)});";
                sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine(constructor, level)]);
            }
            var iconVisible = ComponentPropertyHelper.GetBoolProperty(component, "IconVisible", false);
            if (iconVisible)
                sectionParser.InsertIntoZone("COMPONENT_LOAD", [CodeBuilder.AddLine($"{GetFieldName(component)}.IconsVisible = {iconVisible};", level)]);

        }
        protected override void GenerateReset(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertIntoZone("COMPONENT_RESET", [CodeBuilder.AddLine($"{GetFieldName(component)}?.Reset();", level)]);
        }
        protected override void GenerateUpdate(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertIntoZone("COMPONENT_UPDATE", [CodeBuilder.AddLine($"{GetFieldName(component)}?.Update(gameTime);", level)]);
        }
        protected override void GenerateDraw(SectionParser sectionParser, ComponentModel component, int level)
        {
            sectionParser.InsertIntoZone("COMPONENT_DRAW", [CodeBuilder.AddLine($"{GetFieldName(component)}?.Draw(spriteBatch);", level)]);
        }
        protected override void GeneratePartialFunctions(SectionParser sectionParser, ComponentModel component, int level)
        {
            foreach (var menuItem in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}{menuItem.Type}";
                sectionParser.InsertIntoZone("PARTIAL_METHODS",
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
            sectionParser.InsertIntoZone("AVAILABLE_FIELDS", [CodeBuilder.AddLine($"// [FontManager] _fontManager", level)], true);
            base.GenerateUserFileCommentField(sectionParser, component, level);
            foreach (var subComponent in component.SubComponents)
            {
                var menuItemFieldName = $"_{component.Key}_{subComponent.Key}{subComponent.Type}";
                sectionParser.InsertIntoZone("AVAILABLE_FIELDS", [CodeBuilder.AddLine($"// [{subComponent.Type}] {menuItemFieldName}", level)], true);
            }
        }
        protected override void GenerateUserFilePartialFunctions(SectionParser sectionParser, ComponentModel component, int level)
        {
            foreach (var menuItem in component.SubComponents.Where(c => c.Type == "MenuItem"))
            {
                var menuItemFieldName = $"{component.Key}_{menuItem.Key}{menuItem.Type}";

                if (!sectionParser.IsPartialFunctionExisting($"{menuItemFieldName}Selection"))
                {
                    sectionParser.InsertIntoZone("PARTIAL_METHODS",
                    [
                        CodeBuilder.OpenBlock($"private partial {menuItem.Type} {menuItemFieldName}Selection({menuItem.Type} menuItem)", level),
                        CodeBuilder.AddLine($"return menuItem;", level + 1),
                        CodeBuilder.CloseBlock(level)
                    ]);
                }
                if (!sectionParser.IsPartialFunctionExisting($"{menuItemFieldName}Deselection"))
                {
                    sectionParser.InsertIntoZone("PARTIAL_METHODS",
                    [
                        CodeBuilder.OpenBlock($"private partial {menuItem.Type} {menuItemFieldName}Deselection({menuItem.Type} menuItem)", level),
                        CodeBuilder.AddLine($"return menuItem;", level + 1),
                        CodeBuilder.CloseBlock(level),
                    ]);
                }
                if (!sectionParser.IsPartialFunctionExisting($"{menuItemFieldName}Activation"))
                {
                    sectionParser.InsertIntoZone("PARTIAL_METHODS",
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
