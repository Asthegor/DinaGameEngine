using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;

namespace DinaGameEngine.ViewModels
{
    public class DialogViewModel
    {
        private DialogButtons _buttons = DialogButtons.OK;
        public DialogViewModel()
        {
            CloseCommand = new RelayCommand(_ =>
            {
                Result = DialogResult.None;
                CloseAction?.Invoke();
            });

            FooterButtons = new ButtonBarViewModel();
        }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DialogIcon Icon { get; set; } = DialogIcon.None;
        public DialogButtons Buttons
        {
            get => _buttons;
            set
            {
                _buttons = value;
                CreateFooterButtons();
            }
        }
        public DialogResult Result { get; set; } = DialogResult.None;
        public ButtonBarViewModel FooterButtons { get; }

        private void CreateFooterButtons()
        {
            FooterButtons.Buttons.Clear();
            switch (Buttons)
            {
                case DialogButtons.OK:
                    FooterButtons.Buttons.Add(new ButtonDescriptor
                    {
                        Label = LocalizationManager.GetTranslation("Dialog_OK"),
                        Command = new RelayCommand(_ =>
                        {
                            Result = DialogResult.OK;
                            CloseAction?.Invoke();
                        }),
                        Role = ButtonRole.Primary,
                        ContentHorizontalAlignment = ControlHorizontalAlignment.Center,
                        ContentVerticalAlignment = ControlVerticalAlignment.Center,
                    });
                    break;
                case DialogButtons.OKCancel:
                    FooterButtons.Buttons.Add(new ButtonDescriptor
                    {
                        Label = LocalizationManager.GetTranslation("Dialog_Cancel"),
                        Command = new RelayCommand(_ =>
                        {
                            Result = DialogResult.Cancel;
                            CloseAction?.Invoke();
                        }),
                        Role = ButtonRole.Secondary,
                        ContentHorizontalAlignment = ControlHorizontalAlignment.Center,
                        ContentVerticalAlignment = ControlVerticalAlignment.Center,
                    });
                    FooterButtons.Buttons.Add(new ButtonDescriptor
                    {
                        Label = LocalizationManager.GetTranslation("Dialog_OK"),
                        Command = new RelayCommand(_ =>
                        {
                            Result = DialogResult.OK;
                            CloseAction?.Invoke();
                        }),
                        Role = ButtonRole.Primary,
                        ContentHorizontalAlignment = ControlHorizontalAlignment.Center,
                        ContentVerticalAlignment = ControlVerticalAlignment.Center,
                    });
                    break;
                case DialogButtons.YesNo:
                    FooterButtons.Buttons.Add(new ButtonDescriptor
                    {
                        Label = LocalizationManager.GetTranslation("Dialog_No"),
                        Command = new RelayCommand(_ =>
                        {
                            Result = DialogResult.No;
                            CloseAction?.Invoke();
                        }),
                        Role = ButtonRole.Secondary,
                        ContentHorizontalAlignment = ControlHorizontalAlignment.Center,
                        ContentVerticalAlignment = ControlVerticalAlignment.Center,
                    });
                    FooterButtons.Buttons.Add(new ButtonDescriptor
                    {
                        Label = LocalizationManager.GetTranslation("Dialog_Yes"),
                        Command = new RelayCommand(_ =>
                        {
                            Result = DialogResult.Yes;
                            CloseAction?.Invoke();
                        }),
                        Role = ButtonRole.Primary,
                        ContentHorizontalAlignment = ControlHorizontalAlignment.Center,
                        ContentVerticalAlignment = ControlVerticalAlignment.Center,
                    });
                    break;
            }
        }

        public RelayCommand CloseCommand { get; }
        public Action? CloseAction { get; set; }
    }
}
