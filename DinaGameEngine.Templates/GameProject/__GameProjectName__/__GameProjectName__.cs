using DinaCSharp.Inputs;
using DinaCSharp.Resources;
using DinaCSharp.Services;
using DinaCSharp.Services.Audio;
using DinaCSharp.Services.Fonts;
using DinaCSharp.Services.Localization;
using DinaCSharp.Services.Save;
using DinaCSharp.Services.Scenes;
using DinaCSharp.Services.Screen;

using __RootNamespace__.Core.Datas;
using __RootNamespace__.Core.Keys;
using __RootNamespace__.Scenes;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System.Linq;

namespace __RootNamespace__
{
    public class __GameProjectName__ : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SceneManager _sceneManager;
        private bool _isReadyToDraw;

        public __GameProjectName__()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        #region Cycle de vie MonoGame
        protected override void Initialize()
        {
            InitializeScreenManager();
            InitializeConfig();
            InitializeFontManager();
            InitializeSceneManager();
            InitializeInputManager();

            RegisterServices();

            ApplyConfiguration();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            RegisterScene();

            _sceneManager.SetCurrentScene(ProjectSceneKeys.MainMenu);

            _isReadyToDraw = true;
        }

        protected override void Update(GameTime gameTime)
        {
            _sceneManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (!_isReadyToDraw)
                return;

            GraphicsDevice.Clear(Color.Black);

            _sceneManager.Draw(_spriteBatch, true);

            base.Draw(gameTime);
        }
        #endregion


        #region DinaCSharp

        #region Initialisations
        private void InitializeScreenManager()
        {
            var screenManager = ScreenManager.Initialize(_graphics, Window);
            ScreenManager.SetAllowedResolutions([
                new Point(1280, 720),
                new Point(1600, 900),
                new Point(1920, 1080),
                new Point(2560, 1440),
                new Point(3840, 2160)
            ]);
            screenManager.Register(ServiceKeys.ScreenManager);
        }
        private void InitializeSceneManager()
        {
            _sceneManager = SceneManager.InitializeAndRegister(this);
        }
        private void InitializeFontManager()
        {
            var fontManager = FontManager.Initialize(Services, "FontContent",
                [
                    new ResolutionFontInfo(ResolutionKeys.R720p, new Point(1280, 720), "_720p"),
                    new ResolutionFontInfo(ResolutionKeys.R900p, new Point(1600, 900), "_900p"),
                    new ResolutionFontInfo(ResolutionKeys.R1080p, new Point(1920, 1080), "_1080p"),
                    new ResolutionFontInfo(ResolutionKeys.R1440p, new Point(2560, 1440), "_1440p"),
                    new ResolutionFontInfo(ResolutionKeys.R2160p, new Point(3840, 2160), "_2160p")
                ],
                ResolutionKeys.R1080p);
            fontManager.Register(ServiceKeys.FontManager);
        }
        private static void InitializeConfig()
        {
            /// Configuration par défaut
            var dpMaxResolution = ScreenManager.AvailableResolutions.ToList().Last();
            Point maxResolution = new Point(dpMaxResolution.Width, dpMaxResolution.Height);
            var defaultConfig = new DefaultConfigData(maxResolution, true);
            ServiceLocator.Register(ProjectServiceKeys.DefaultConfig, defaultConfig);

            /// Chargement du fichier de configuration et enregistrement dans le ServiceLocator
            var configData = SaveManager.LoadObjectFromEncryptFile<ConfigData>(ProjectServiceKeys.Config.Value) ?? defaultConfig;
            ServiceLocator.Register(ProjectServiceKeys.Config, configData);
        }
        private static void InitializeInputManager()
        {
            var playerController = InputManager.RegisterPlayer(PlayerIndex.One,
                (PlayerInputKeys.Up, new ControllerKey[] { new KeyboardControllerKey(Keys.Up), new GamepadControllerKey(Buttons.LeftThumbstickUp) }),
                (PlayerInputKeys.Down, new ControllerKey[] { new KeyboardControllerKey(Keys.Down), new GamepadControllerKey(Buttons.LeftThumbstickDown) }),
                (PlayerInputKeys.Left, new ControllerKey[] { new KeyboardControllerKey(Keys.Left), new GamepadControllerKey(Buttons.LeftThumbstickLeft) }),
                (PlayerInputKeys.Right, new ControllerKey[] { new KeyboardControllerKey(Keys.Right), new GamepadControllerKey(Buttons.LeftThumbstickRight) }),
                (PlayerInputKeys.Activate, new ControllerKey[] { new KeyboardControllerKey(Keys.Enter), new GamepadControllerKey(Buttons.A) }),
                (PlayerInputKeys.Cancel, new ControllerKey[] { new KeyboardControllerKey(Keys.Back), new GamepadControllerKey(Buttons.B) })
            );
            ServiceLocator.Register(ProjectServiceKeys.PlayerController, playerController);
        }
        #endregion Initialisations

        #region Enregistrement des services
        private void RegisterServices()
        {
            ServiceLocator.Register(ProjectServiceKeys.SoundManager, new SoundManager(this, "AudioContent"));

            ResourceManager assetsResourceManager = new ResourceManager(Services, "AssetsContent");
            ServiceLocator.Register(ProjectServiceKeys.AssetsResourceManager, assetsResourceManager);
        }


        #endregion Enregistrement des services

        private static void ApplyConfiguration()
        {
            ConfigData configData = ServiceLocator.Get<ConfigData>(ProjectServiceKeys.Config);
            ScreenManager screenManager = ServiceLocator.Get<ScreenManager>(ServiceKeys.ScreenManager);
            SoundManager soundManager = ServiceLocator.Get<SoundManager>(ServiceKeys.SoundManager);

            // Application de la configuration sauvegardée
            screenManager.SetResolution(configData.ResolutionWidth, configData.ResolutionHeight);
            screenManager.SetFullScreen(configData.Fullscreen);
            soundManager.MasterVolume = configData.MasterVolume / 100;
            soundManager.MusicVolume = configData.MusicVolume / 100;
            soundManager.GlobalSoundVolume = configData.SoundsVolume / 100;
        }
        #endregion DinaCSharp

        private void RegisterScene()
        {
            _sceneManager.AddScene(ProjectSceneKeys.MainMenu, () => new MainMenuScene(_sceneManager));
            _sceneManager.AddScene(ProjectSceneKeys.OptionsMenu, () => new OptionsMenuScene(_sceneManager));
        }
    }
}
