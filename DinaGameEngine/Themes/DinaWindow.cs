using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;

using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DinaGameEngine.Themes
{
    public class DinaWindow : Window
    {
        private double _restoreLeft, _restoreTop, _restoreWidth, _restoreHeight;
        public DinaWindow()
        {
            CloseCommand = new RelayCommand(_ => Close());
            MinimizeCommand = new RelayCommand(_ => WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(_ =>
            {
                if (!IsMaximized)
                {
                    _restoreLeft = Left;
                    _restoreTop = Top;
                    _restoreWidth = Width;
                    _restoreHeight = Height;
                    Left = SystemParameters.WorkArea.Left;
                    Top = SystemParameters.WorkArea.Top;
                    Width = SystemParameters.WorkArea.Width;
                    Height = SystemParameters.WorkArea.Height;
                    IsMaximized = true;
                }
                else
                {
                    Left = _restoreLeft;
                    Top = _restoreTop;
                    Width = _restoreWidth;
                    Height = _restoreHeight;
                    IsMaximized = false;
                }
            });
        }
        public TitleBarButtons TitleBarButtons
        {
            get { return (TitleBarButtons)GetValue(TitleBarButtonsProperty); }
            set { SetValue(TitleBarButtonsProperty, value); }
        }
        public static readonly DependencyProperty TitleBarButtonsProperty =
            DependencyProperty.Register(nameof(TitleBarButtons), typeof(TitleBarButtons), typeof(DinaWindow), new PropertyMetadata(TitleBarButtons.CloseOnly));



        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(DinaWindow), new PropertyMetadata(null));

        public ICommand MinimizeCommand
        {
            get { return (ICommand)GetValue(MinimizeCommandProperty); }
            set { SetValue(MinimizeCommandProperty, value); }
        }
        public static readonly DependencyProperty MinimizeCommandProperty =
            DependencyProperty.Register(nameof(MinimizeCommand), typeof(ICommand), typeof(DinaWindow), new PropertyMetadata(null));

        public ICommand MaximizeCommand
        {
            get => (ICommand)GetValue(MaximizeCommandProperty);
            set => SetValue(MaximizeCommandProperty, value);
        }
        public static readonly DependencyProperty MaximizeCommandProperty =
            DependencyProperty.Register(nameof(MaximizeCommand), typeof(ICommand), typeof(DinaWindow), new PropertyMetadata(null));

        public bool IsMaximized
        {
            get => (bool)GetValue(IsMaximizedProperty);
            set => SetValue(IsMaximizedProperty, value);
        }
        public static readonly DependencyProperty IsMaximizedProperty =
            DependencyProperty.Register(nameof(IsMaximized), typeof(bool), typeof(DinaWindow), new PropertyMetadata(false));
    }
}
