using DinaCSharp.Core;
using DinaCSharp.Enums;
using DinaCSharp.Graphics;
using DinaCSharp.Interfaces;
using DinaCSharp.Services;
using DinaCSharp.Services.Audio;
using DinaCSharp.Services.Fonts;
using DinaCSharp.Services.Save;
using DinaCSharp.Services.Scenes;
using DinaCSharp.Services.Screen;
using DinaCSharp.Utils;

using __RootNamespace__.Core.Datas;
using __RootNamespace__.Core.Keys;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Linq;

namespace __RootNamespace__.Scenes
{
    public class OptionsMenuScene(SceneManager sceneManager) : Scene(sceneManager)
    {
        private ScreenManager _screenManager;
        private SoundManager _soundManager;

        private const int CATEGORY_FRAME_THICKNESS = 2;
        private const int CATEGORY_FRAME_PADDING = 8;
        private const int BUTTON_BORDER_THICKNESS = 2;

        private const float OPTIONS_SPACING_Y = 50f;
        private const float OPTIONS_SPACING_X = 30f;

        private Vector2 BUTTONS_DIMENSIONS = new Vector2(150, 40);
        private Vector2 SLIDER_DIMENSIONS = new Vector2(200, 20);
        private Vector2 LABEL_DIMENSIONS = new Vector2(220, 25);

        MouseState _oldMouseState;

        private SpriteFont _categoryFont;
        private SpriteFont _labelFont;
        private SpriteFont _optionFont;

        private Group _generalGroup;
        private Group _graphicsGroup;
        private Group _audioGroup;
        private Group _controlsGroup;
        private Group _buttonsGroup;

        private CheckBox _fullscreenCheckbox;
        private ListBox _resolutionListBox;
        private Button _resolutionButton;

        private Slider _masterSlider;
        private Slider _musicSlider;
        private Slider _soundsSlider;

        private ConfigData _configDatas;
        private ConfigData _defaultDatas;

        public override void Load()
        {
            UIScaler.Update(ScreenDimensions);

            _screenManager = ServiceLocator.Get<ScreenManager>(ServiceKeys.ScreenManager);
            _soundManager = ServiceLocator.Get<SoundManager>(ProjectServiceKeys.SoundManager);
            FontManager fontManager = ServiceLocator.Get<FontManager>(ServiceKeys.FontManager);

            LoadConfig();

            _categoryFont = fontManager.Load(FontKeys.Default);
            _labelFont = fontManager.Load(FontKeys.Default);
            _optionFont = fontManager.Load(FontKeys.Default);

            _generalGroup = [];
            _graphicsGroup = [];
            _audioGroup = [];
            _controlsGroup = [];


            Vector2 offsetTitle = new Vector2(2, 2);
            var titleFont = fontManager.Load(FontKeys.Default);
            var title = new ShadowText(titleFont, "OPTIONS_TITLE", PaletteColors.MainMenu_Title, new Vector2(0, ScreenDimensions.Y / 16), PaletteColors.MainMenu_Title_Shadow, offsetTitle, HorizontalAlignment.Center);
            title.Dimensions = new Vector2(ScreenDimensions.X, title.Dimensions.Y);
            _generalGroup.Add(title);

            _generalGroup.Add(CreateLanguageGroup());

            _graphicsGroup = CreateGraphicsGroup();
            _audioGroup = CreateSoundsGroup();
            _controlsGroup = CreateControlsGroup();
            _buttonsGroup = CreateButtonsGroup();

            // Positionnement des groupes
            _audioGroup.Position = new Vector2((ScreenDimensions.X - _audioGroup.Dimensions.X) * 7 / 9, ScreenDimensions.Y / 4);
            _graphicsGroup.Position = new Vector2((ScreenDimensions.X - _graphicsGroup.Dimensions.X) * 2 / 9,
                                                  _audioGroup.Position.Y + (_audioGroup.Dimensions.Y - _graphicsGroup.Dimensions.Y) / 2);
            _buttonsGroup.Position = new Vector2((ScreenDimensions.X - _buttonsGroup.Dimensions.X) / 2, ScreenDimensions.Y * 9 / 10);

            // Ajustement de la liste des résolutions (après avoir positionner le groupe graphique)
            _resolutionListBox.Position = _resolutionButton.Position + new Vector2(0, _resolutionButton.Dimensions.Y + 5);
        }

        private static Group CreateLanguageGroup()
        {
            Group languageGroup = [];


            return languageGroup;
        }

        public override void Reset()
        {
            _fullscreenCheckbox.IsChecked = _screenManager.IsFullScreen;
            _oldMouseState = Mouse.GetState();
        }
        public override void Update(GameTime gametime)
        {
            if (_resolutionListBox.Visible)
            {
                _resolutionListBox.Update(gametime);
                // Si on clique dans la liste, la liste est masquée (Visible = false)
                MouseState currentMouseState = Mouse.GetState();
                if (_resolutionListBox.Visible && currentMouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed)
                    // Si on a cliqué autre part que sur la liste, on la masque.
                    _resolutionListBox.Visible = false;

                _oldMouseState = currentMouseState;
                return;
            }
            _generalGroup?.Update(gametime);
            _graphicsGroup?.Update(gametime);
            _audioGroup?.Update(gametime);
            _controlsGroup?.Update(gametime);
            _buttonsGroup?.Update(gametime);
        }
        public override void Draw(SpriteBatch spritebatch)
        {
            _generalGroup?.Draw(spritebatch);
            _graphicsGroup?.Draw(spritebatch);
            _audioGroup?.Draw(spritebatch);
            _controlsGroup?.Draw(spritebatch);
            _buttonsGroup?.Draw(spritebatch);

            // On affiche la liste des résolutions par dessus tous les autres contrôles.
            _resolutionListBox?.Draw(spritebatch);
        }

        private void SaveConfig()
        {
            SaveManager.SaveObjectToFile(_configDatas, ProjectServiceKeys.Config.Value, true);
        }

        private void LoadConfig()
        {
            _defaultDatas = ServiceLocator.Get<DefaultConfigData>(ProjectServiceKeys.DefaultConfig);
            _configDatas = ServiceLocator.Get<ConfigData>(ProjectServiceKeys.Config) ?? _defaultDatas;
        }

        private Group CreateGraphicsGroup()
        {
            Group graphicsGroup = [];
            graphicsGroup.AddTitle(_categoryFont, "OPTIONS_CATEGORY_GRAPHICS", PaletteColors.Options_Category, PaletteColors.Options_Title_Shadow, UIScaler.Scale(CATEGORY_FRAME_PADDING), UIScaler.Scale(CATEGORY_FRAME_THICKNESS));

            float maxLabelWidth = 0;
            var fontHeight = _labelFont.LineSpacing;
            var fullscreenLabel = new Text(_labelFont, "OPTIONS_LBL_FULLSCREEN", PaletteColors.Options_Label, verticalalignment: VerticalAlignment.Center)
            { Dimensions = UIScaler.Scale(LABEL_DIMENSIONS) };
            _fullscreenCheckbox = new CheckBox(Color.Orange, Color.DarkGoldenrod, Vector2.Zero, new Vector2(fontHeight * 3 / 4, fontHeight * 3 / 4))
            {
                IsChecked = _screenManager.IsFullScreen,
                //CheckedTexture = Content.Load<Texture2D>("Image")
            };
            _fullscreenCheckbox.OnClicked += (sender, eventArgs) =>
            {
                _screenManager.SetFullScreen(eventArgs.CheckBox.IsChecked);
                _configDatas.Fullscreen = _screenManager.IsFullScreen;
                SaveConfig();
            };
            maxLabelWidth = Math.Max(maxLabelWidth, fullscreenLabel.Dimensions.X);

            var resolutionLabel = new Text(_labelFont, "OPTIONS_LBL_RESOLUTION", PaletteColors.Options_Label)
            { Dimensions = UIScaler.Scale(LABEL_DIMENSIONS) };
            var currentResolution = _screenManager.CurrentResolution;
            _resolutionButton = new Button(Vector2.Zero, UIScaler.Scale(BUTTONS_DIMENSIONS),
                                           _optionFont, $"{currentResolution.X} x {currentResolution.Y}",
                                           PaletteColors.Options_Button_Text, margin: UIScaler.Scale(new Vector2(2, 2)))
            {
                BorderThickness = UIScaler.Scale(2),
                BackgroundColor = Color.DarkSlateGray,
                BorderColor = Color.Gray
            };
            _resolutionButton.OnHovered += (sender, eventArgs) => eventArgs.Button.TextColor = Color.LimeGreen;
            maxLabelWidth = Math.Max(maxLabelWidth, resolutionLabel.Dimensions.X);

            var resolutions = ScreenManager.AvailableResolutions
                .Select(dm => new Text(_optionFont, $"{dm.Width} x {dm.Height}", PaletteColors.Options_Label))
                .Cast<IElement>()
                .ToList();
            _resolutionListBox = new ListBox(resolutions, Vector2.Zero, nbvisibleelements: -1) { Visible = false };
            _resolutionListBox.OnLeftClick += (sender, eventArgs) =>
            {
                int index = eventArgs.Index;
                if (index >= 0)
                {
                    var resolutions = ScreenManager.AvailableResolutions;
                    var dm = resolutions.ElementAt(index);
                    _screenManager.SetResolution(dm.Width, dm.Height);
                    _resolutionButton.Content = $"{dm.Width} x {dm.Height}";
                    _configDatas.ResolutionWidth = _screenManager.CurrentResolution.X;
                    _configDatas.ResolutionHeight = _screenManager.CurrentResolution.Y;
                    SaveConfig();
                }
                _resolutionListBox.Visible = false;
            };
            _resolutionButton.OnClicked += (sender, eventArgs) => { _resolutionListBox.Visible = true; };

            // Mise à jour des positions des contrôles au sein du Group
            resolutionLabel.Position = fullscreenLabel.Position + new Vector2(0, UIScaler.Scale(OPTIONS_SPACING_Y));
            _resolutionButton.Position = resolutionLabel.Position + new Vector2(maxLabelWidth + UIScaler.Scale(OPTIONS_SPACING_X), (resolutionLabel.Dimensions.Y - _resolutionButton.Dimensions.Y) / 2);
            _fullscreenCheckbox.Position = new Vector2(_resolutionButton.Position.X + (_resolutionButton.Dimensions.X - _fullscreenCheckbox.Dimensions.X) / 2, fullscreenLabel.Position.Y + (fullscreenLabel.Dimensions.Y - _fullscreenCheckbox.Dimensions.Y) / 2);

            // Ajout des contrôles au Group
            graphicsGroup.Add(fullscreenLabel);
            graphicsGroup.Add(_fullscreenCheckbox);
            graphicsGroup.Add(resolutionLabel);
            graphicsGroup.Add(_resolutionButton);

            graphicsGroup.Dimensions = new Vector2(resolutionLabel.Dimensions.X + _resolutionButton.Dimensions.X + UIScaler.Scale(OPTIONS_SPACING_X), graphicsGroup.Dimensions.Y);

            return graphicsGroup;
        }
        private Group CreateSoundsGroup()
        {
            Group audioGroup = [];
            audioGroup.AddTitle(_categoryFont, "OPTIONS_CATEGORY_AUDIO",
                                PaletteColors.Options_Title, PaletteColors.Options_Title_Shadow,
                                UIScaler.Scale(CATEGORY_FRAME_PADDING), UIScaler.Scale(CATEGORY_FRAME_THICKNESS));

            float maxLabelWidth = 0;

            int masterVolume = (int)(_soundManager.MasterVolume * 100);
            var masterLabel = new Text(_labelFont, "OPTIONS_LBL_MASTERVOLUME", PaletteColors.Options_Label) { Dimensions = UIScaler.Scale(LABEL_DIMENSIONS) };
            var masterValue = new Text(_optionFont, "100", PaletteColors.Options_Label)
            { Content = masterVolume.ToString() };
            _masterSlider = new Slider(Vector2.Zero, UIScaler.Scale(SLIDER_DIMENSIONS), 0, 100, masterVolume);
            _masterSlider.OnValueChanged += (sender, eventArgs) =>
            {
                _soundManager.MasterVolume = eventArgs.Value / _masterSlider.MaxValue;
                masterValue.Content = ((int)eventArgs.Value).ToString();
                _configDatas.MasterVolume = (int)eventArgs.Value;
            };
            maxLabelWidth = Math.Max(maxLabelWidth, masterLabel.Dimensions.X);

            int musicVolume = (int)(_soundManager.MusicVolume * 100);
            var musicLabel = new Text(_labelFont, "OPTIONS_LBL_MUSICVOLUME", PaletteColors.Options_Label) { Dimensions = UIScaler.Scale(LABEL_DIMENSIONS) };
            var musicValue = new Text(_optionFont, "100", PaletteColors.Options_Label)
            { Content = musicVolume.ToString() };
            _musicSlider = new Slider(Vector2.Zero, UIScaler.Scale(SLIDER_DIMENSIONS), 0, 100, musicVolume);
            _musicSlider.OnValueChanged += (sender, eventArgs) =>
            {
                _soundManager.MusicVolume = eventArgs.Value / _musicSlider.MaxValue;
                musicValue.Content = ((int)eventArgs.Value).ToString();
                _configDatas.MusicVolume = (int)eventArgs.Value;
            };
            maxLabelWidth = Math.Max(maxLabelWidth, musicLabel.Dimensions.X);

            int soundsVolume = (int)(_soundManager.GlobalSoundVolume * 100);
            var soundsLabel = new Text(_labelFont, "OPTIONS_LBL_SOUNDSVOLUME", PaletteColors.Options_Label) { Dimensions = UIScaler.Scale(LABEL_DIMENSIONS) };
            var soundsValue = new Text(_optionFont, "100", PaletteColors.Options_Label)
            { Content = soundsVolume.ToString() };
            _soundsSlider = new Slider(Vector2.Zero, UIScaler.Scale(SLIDER_DIMENSIONS), 0, 100, soundsVolume);
            _soundsSlider.OnValueChanged += (sender, eventArgs) =>
            {
                _soundManager.GlobalSoundVolume = eventArgs.Value / _soundsSlider.MaxValue;
                soundsValue.Content = ((int)eventArgs.Value).ToString();
                _configDatas.SoundsVolume = (int)eventArgs.Value;
            };
            maxLabelWidth = Math.Max(maxLabelWidth, soundsLabel.Dimensions.X);

            // mise à jour des positions au sein du Group
            var spacingX = UIScaler.Scale(OPTIONS_SPACING_X);
            masterValue.Position = masterLabel.Position + new Vector2(maxLabelWidth + spacingX, (masterLabel.Dimensions.Y - masterValue.Dimensions.Y) / 2);
            _masterSlider.Position = masterValue.Position + new Vector2(masterValue.Dimensions.X + spacingX, (masterValue.Dimensions.Y - _masterSlider.Dimensions.Y) / 2);
            musicLabel.Position = masterLabel.Position + new Vector2(0, UIScaler.Scale(OPTIONS_SPACING_Y));
            musicValue.Position = musicLabel.Position + new Vector2(maxLabelWidth + spacingX, (musicLabel.Dimensions.Y - musicValue.Dimensions.Y) / 2);
            _musicSlider.Position = musicValue.Position + new Vector2(musicValue.Dimensions.X + spacingX, (musicValue.Dimensions.Y - _musicSlider.Dimensions.Y) / 2);
            soundsLabel.Position = musicLabel.Position + new Vector2(0, UIScaler.Scale(OPTIONS_SPACING_Y));
            soundsValue.Position = soundsLabel.Position + new Vector2(maxLabelWidth + spacingX, (soundsLabel.Dimensions.Y - soundsValue.Dimensions.Y) / 2);
            _soundsSlider.Position = soundsValue.Position + new Vector2(soundsValue.Dimensions.X + spacingX, (soundsValue.Dimensions.Y - _soundsSlider.Dimensions.Y) / 2);

            // Ajout des contrôles au Group
            audioGroup.Add(masterLabel);
            audioGroup.Add(_masterSlider);
            audioGroup.Add(masterValue);
            audioGroup.Add(musicLabel);
            audioGroup.Add(_musicSlider);
            audioGroup.Add(musicValue);
            audioGroup.Add(soundsLabel);
            audioGroup.Add(_soundsSlider);
            audioGroup.Add(soundsValue);

            return audioGroup;
        }
        private static Group CreateControlsGroup()
        {
            return [];
        }
        private Group CreateButtonsGroup()
        {
            Group buttonGroup = [];

            var buttonDimensionScaled = UIScaler.Scale(BUTTONS_DIMENSIONS);
            var backButton = new Button(default, buttonDimensionScaled,
                                        _labelFont, "UI_BACK", PaletteColors.Options_Button_Text, SaveAndGotoMainMenu, onHover: OnHoverBackButton)
            {
                TextColor = PaletteColors.Options_Button_Text,
                BackgroundColor = PaletteColors.Options_Button_Back_Background,
                BorderColor = PaletteColors.Options_Button_Back_Border,
                BorderThickness = UIScaler.Scale(BUTTON_BORDER_THICKNESS)
            };


            var resetButton = new Button(Vector2.Zero, buttonDimensionScaled,
                                         _labelFont, "UI_RESET", PaletteColors.Options_Button_Text, ResetModifications, onHover: OnHoverResetButton)
            {
                TextColor = PaletteColors.Options_Button_Text,
                BackgroundColor = PaletteColors.Options_Button_Reset_Background,
                BorderColor = PaletteColors.Options_Button_Reset_Border,
                BorderThickness = UIScaler.Scale(BUTTON_BORDER_THICKNESS),
            };

            backButton.Position = new Vector2(resetButton.Position.X + resetButton.Dimensions.X + UIScaler.Scale(OPTIONS_SPACING_X), resetButton.Position.Y);

            // Ajout des boutons au groupe
            buttonGroup.Add(resetButton);
            buttonGroup.Add(backButton);

            return buttonGroup;
        }
        private void SaveAndGotoMainMenu(Button button)
        {
            SaveConfig();
            SetCurrentScene(ProjectSceneKeys.MainMenu);
        }

        private void ResetModifications(Button button)
        {
            _screenManager.SetResolution(_defaultDatas.ResolutionWidth, _defaultDatas.ResolutionHeight);
            _screenManager.SetFullScreen(_defaultDatas.Fullscreen);

            _masterSlider.Value = _defaultDatas.MasterVolume;
            _soundManager.MasterVolume = _defaultDatas.MasterVolume;

            _musicSlider.Value = _defaultDatas.MusicVolume;
            _soundManager.MusicVolume = _defaultDatas.MusicVolume;

            _soundsSlider.Value = _defaultDatas.SoundsVolume;
            _soundManager.GlobalSoundVolume = _defaultDatas.SoundsVolume;

            _fullscreenCheckbox.IsChecked = _defaultDatas.Fullscreen;

            _configDatas = _defaultDatas;
            SaveConfig();
        }

        private void OnHoverBackButton(Button button)
        {
            button.BorderColor = PaletteColors.Options_Button_Back_Hovered;
        }
        private void OnHoverResetButton(Button button)
        {
            button.BorderColor = PaletteColors.Options_Button_Reset_Hovered;
        }
    }

}
