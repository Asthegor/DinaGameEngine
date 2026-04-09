using DinaGameEngine.CodeGeneration;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models;
using DinaGameEngine.Models.Project;

using System.Drawing;

namespace DinaGameEngine.Services
{
    public partial class ProjectService
    {
        private static void AddMainMenuDefaults(GameProjectModel gameProjectModel)
        {
            #region Polices
            gameProjectModel.Fonts.Add(new FontModel { Key = "MainMenu_Title", Size = 128, Spacing = 0, Style = SpriteFontStyle.Regular, TtfRelativePath = "../TTF_Files/Roboto-Regular.ttf" });
            gameProjectModel.Fonts.Add(new FontModel { Key = "MainMenu_MenuItems", Size = 30, Spacing = 0, Style = SpriteFontStyle.Regular, TtfRelativePath = "../TTF_Files/Roboto-Regular.ttf" });
            #endregion

            #region Couleurs
            gameProjectModel.Colors.Add(new ColorModel { Key = "MainMenu_Title", R = 255, G = 165, B = 000, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "MainMenu_Title_Shadow", R = 169, G = 169, B = 169, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "MainMenu_MenuItem_Disabled", R = 169, G = 169, B = 169, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "MainMenu_MenuItem", R = 255, G = 255, B = 255, A = 255 });
            gameProjectModel.Colors.Add(new ColorModel { Key = "MainMenu_MenuItem_Hovered", R = 255, G = 255, B = 000, A = 255 });
            #endregion

            var mainMenuScene = new SceneModel { Name = "Main Menu", Class = "MainMenuScene", Key = "MainMenu", IsStartup = true };

            #region MenuManager
            var menuManager = new ComponentModel { Type = "MenuManager", Key = "Main" };

            #region Titles
            var menuTitle = new ComponentModel { Type = "MenuTitle", Key = "MainTitle" };
            menuTitle.Properties["Font"] = "MainMenu_Title";
            menuTitle.Properties["Content"] = "GAME_TITLE";
            menuTitle.Properties["Color"] = "MainMenu_Title";
            menuTitle.Properties["ShadowColor"] = "MainMenu_Title_Shadow";
            menuTitle.Properties["ShadowOffset"] = new Point(3, 3);
            menuTitle.Properties["Position"] = new Point(604, 90);
            menuTitle.Properties["Dimensions"] = new Point(711, 200);


            menuManager.SubComponents.Add(menuTitle);
            #endregion

            #region MenuItems
            var menuItemContinue = new ComponentModel { Type = "MenuItem", Key = "Continue" };
            menuItemContinue.Properties["Font"] = "MainMenu_MenuItems";
            menuItemContinue.Properties["Content"] = "MAINMENU_CONTINUE";
            menuItemContinue.Properties["Color"] = "MainMenu_MenuItem";
            menuItemContinue.Properties["HAlign"] = DinaHorizontalAlignment.Center.ToString();
            menuItemContinue.Properties["VAlign"] = DinaVerticalAlignment.Center.ToString();
            menuItemContinue.Properties["State"] = DinaState.Disable.ToString();
            menuItemContinue.Properties["Position"] = new Point(786, 511);
            menuItemContinue.Properties["Dimensions"] = new Point(348, 106);
            menuManager.SubComponents.Add(menuItemContinue);

            var menuItemPlay = new ComponentModel { Type = "MenuItem", Key = "Play" };
            menuItemPlay.Properties["Font"] = "MainMenu_MenuItems";
            menuItemPlay.Properties["Content"] = "MAINMENU_PLAY";
            menuItemPlay.Properties["Color"] = "MainMenu_MenuItem";
            menuItemPlay.Properties["HAlign"] = DinaHorizontalAlignment.Center.ToString();
            menuItemPlay.Properties["VAlign"] = DinaVerticalAlignment.Center.ToString();
            menuItemPlay.Properties["Position"] = new Point(786, 622);
            menuItemPlay.Properties["Dimensions"] = new Point(348, 106);
            menuManager.SubComponents.Add(menuItemPlay);

            var menuItemOptions = new ComponentModel { Type = "MenuItem", Key = "Options" };
            menuItemOptions.Properties["Font"] = "MainMenu_MenuItems";
            menuItemOptions.Properties["Content"] = "MAINMENU_OPTIONS";
            menuItemOptions.Properties["Color"] = "MainMenu_MenuItem";
            menuItemOptions.Properties["HAlign"] = DinaHorizontalAlignment.Center.ToString();
            menuItemOptions.Properties["VAlign"] = DinaVerticalAlignment.Center.ToString();
            menuItemOptions.Properties["Position"] = new Point(786, 733);
            menuItemOptions.Properties["Dimensions"] = new Point(348, 106);
            menuManager.SubComponents.Add(menuItemOptions);

            var menuItemQuit = new ComponentModel { Type = "MenuItem", Key = "Quit" };
            menuItemQuit.Properties["Font"] = "MainMenu_MenuItems";
            menuItemQuit.Properties["Content"] = "MAINMENU_QUIT";
            menuItemQuit.Properties["Color"] = "MainMenu_MenuItem";
            menuItemQuit.Properties["HAlign"] = DinaHorizontalAlignment.Center.ToString();
            menuItemQuit.Properties["VAlign"] = DinaVerticalAlignment.Center.ToString();
            menuItemQuit.Properties["Position"] = new Point(786, 844);
            menuItemQuit.Properties["Dimensions"] = new Point(348, 106);
            menuManager.SubComponents.Add(menuItemQuit);
            #endregion

            mainMenuScene.Components.Add(menuManager);

            gameProjectModel.Scenes.Add(mainMenuScene);
            #endregion

        }

        private void UpdateMainMenuUserFile(GameProjectModel gameProjectModel)
        {
            var mainMenuScene = gameProjectModel.Scenes.FirstOrDefault(s => s.Key == "MainMenuScene");
            if (mainMenuScene == null)
            {
                _logService.Warning($"Scène 'MainMenuScene' non présente.");
                return;
            }
            var menuManager = mainMenuScene.Components.FirstOrDefault(c => c.Key == "MainMenu");
            if (menuManager == null)
            {
                _logService.Warning($"Composant 'MainMenu' non présent dans la scène '{mainMenuScene.Key}'.");
                return;
            }

            var menuItems = menuManager.SubComponents.Where(c => c.Type == "MenuItem").ToList();

            foreach (var menuItem in menuItems)
            {
                var menuItemFieldName = $"MainMenu_{menuItem.Key}MenuItem";

                _codeGenerator.WriteInPartialFunction(gameProjectModel, mainMenuScene,
                    $"{menuItemFieldName}Selection", [CodeBuilder.AddLine($"menuItem.Color = PaletteColors.MainMenu_MenuItem_Hovered;", 3)]);

                _codeGenerator.WriteInPartialFunction(gameProjectModel, mainMenuScene,
                    $"{menuItemFieldName}Deselection", [CodeBuilder.AddLine($"menuItem.Color = PaletteColors.MainMenu_MenuItem;", 3)]);

                switch (menuItem.Key)
                {
                    case "Play":
                        _codeGenerator.WriteInPartialFunction(gameProjectModel, mainMenuScene,
                            $"{menuItemFieldName}Activation", [CodeBuilder.AddLine($"SetCurrentScene(GameScene);", 3)]);
                        break;
                    case "Options":
                        _codeGenerator.WriteInPartialFunction(gameProjectModel, mainMenuScene,
                            $"{menuItemFieldName}Activation", [CodeBuilder.AddLine($"SetCurrentScene(OptionsMenuScene);", 3)]);
                        break;
                    case "Quit":
                        _codeGenerator.WriteInPartialFunction(gameProjectModel, mainMenuScene,
                            $"{menuItemFieldName}Activation", [CodeBuilder.AddLine($"Exit();", 3)]);
                        break;
                }
            }

        }
    }
}