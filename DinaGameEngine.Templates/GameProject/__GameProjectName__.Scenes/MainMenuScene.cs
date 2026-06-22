using DinaCSharp.Enums;
using DinaCSharp.Services;
using DinaCSharp.Services.Audio;
using DinaCSharp.Services.Fonts;
using DinaCSharp.Services.Menus;
using DinaCSharp.Services.Scenes;

using __RootNamespace__.Core.Keys;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System;

namespace __RootNamespace__.Scenes
{
    public class MainMenuScene(SceneManager sceneManager) : Scene(sceneManager)
    {
        private const float MENU_SPACING_X = 0;
        private const float MENU_SPACING_Y = 20;

        private Vector2 MENU_SPACING = new Vector2(MENU_SPACING_X, MENU_SPACING_Y);
        private Vector2 MENU_OFFSET_TITLE_SHADOW = new Vector2(3, 3);
        private Vector2 MENU_TITLE_POSITION = new Vector2(0, sceneManager.ScreenDimensions.Y / 10);

        private FontManager _fontManager;
        private SoundManager _soundManager;

        MenuManager _menuManager;
        public override void Load()
        {
            LoadingManagers();

            CreateMenu();
        }
        public override void Reset()
        {
            _menuManager?.Reset();
        }
        public override void Update(GameTime gametime)
        {
            _menuManager?.Update(gametime);
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            _menuManager?.Draw(spritebatch);
        }

        private void LoadingManagers()
        {
            _fontManager = ServiceLocator.Get<FontManager>(DinaServiceKeys.FontManager);
            _soundManager = ServiceLocator.Get<SoundManager>(ServiceKeys.SoundManager);
            _menuManager = new MenuManager(MENU_SPACING);
        }

        private void CreateMenu()
        {
            // Polices du titre et des items de menu
            // (définies avec la police par défaut)
            SpriteFont titleFont = _fontManager.Load(FontKeys.Default);
            SpriteFont menuItemFont = _fontManager.Load(FontKeys.Default);

            #region Ajout du titre
            var mainTitle = _menuManager.AddTitle(titleFont, "GAME_TITLE", 
                                                  MENU_TITLE_POSITION, PaletteColors.MainMenu_Title, 
                                                  PaletteColors.MainMenu_Title_Shadow, MENU_OFFSET_TITLE_SHADOW);

            _menuManager.CenterTitles(ScreenDimensions);
            #endregion


            AddMenuItem(menuItemFont, "MAINMENU_PLAY", LaunchGame);
            AddMenuItem(menuItemFont, "MAINMENU_OPTIONS", LaunchOptions);
            AddMenuItem(menuItemFont, "MAINMENU_QUIT", QuitGame);
            _menuManager.CenterMenuItems(ScreenDimensions);
            _menuManager.ItemsPosition = new Vector2((ScreenDimensions.X - _menuManager.ItemsDimensions.X) / 2, ScreenDimensions.Y / 2);

            // Ajout des touches pour le menu
            _menuManager.SetActionKeys(
                (MenuAction.Up, PlayerInputKeys.Up),
                (MenuAction.Down, PlayerInputKeys.Down),
                (MenuAction.Left, PlayerInputKeys.Left),
                (MenuAction.Right, PlayerInputKeys.Right),
                (MenuAction.Activate, PlayerInputKeys.Activate),
                (MenuAction.Cancel, PlayerInputKeys.Cancel)
                );

        }


        #region Fonctions utilitaires
        private void AddMenuItem(SpriteFont menuItemFont, string key, Func<MenuItem, MenuItem> onClick, bool disabled = false)
        {
            var item = _menuManager.AddItem(menuItemFont, key, PaletteColors.MainMenu_MenuItem, Selection, Deselection, onClick, HorizontalAlignment.Center);
            if (disabled)
            {
                item.State = MenuItemState.Disable;
                item.DisableColor = PaletteColors.MainMenu_MenuItem_Disabled;
            }
        }
        #endregion


        #region Fonctions génériques du menu
        private MenuItem Selection(MenuItem item)
        {
            item.Color = PaletteColors.MainMenu_MenuItem_Hovered;
            return item;
        }
        private MenuItem Deselection(MenuItem item)
        {
            item.Color = PaletteColors.MainMenu_MenuItem;
            return item;
        }
        private MenuItem LaunchGame(MenuItem item)
        {
            _soundManager.StopSong();
            SetCurrentScene(SceneKeys.GameScene);
            return item;
        }
        private MenuItem LaunchOptions(MenuItem item)
        {
            _soundManager.StopSong();
            SetCurrentScene(SceneKeys.OptionsMenu);
            return item;
        }
        private MenuItem QuitGame(MenuItem item)
        {
            Exit();
            return item;
        }
        #endregion
    }
}
