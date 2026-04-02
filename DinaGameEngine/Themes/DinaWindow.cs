using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;

using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace DinaGameEngine.Themes
{
    public class DinaWindow : Window
    {
        private double _restoreLeft, _restoreTop, _restoreWidth, _restoreHeight;

        public DinaWindow()
        {
            CloseCommand = new RelayCommand(_ => Close());
            MinimizeCommand = new RelayCommand(_ => WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(_ => MaximizeWindow());

            Activated += (_, _) =>
            {
                TitleBarBackground = DinaColor.TitleBarBackground.ToBrush();
                TitleBarForeground = DinaColor.TitleBarForeground.ToBrush();
                TitleBarBorderBrush = DinaColor.WindowBorder.ToBrush();
            };
            Deactivated += (_, _) =>
            {
                TitleBarBackground = DinaColor.WindowInactiveBackground.ToBrush();
                TitleBarForeground = DinaColor.WindowInactiveForeground.ToBrush();
                TitleBarBorderBrush = DinaColor.WindowInactiveBorder.ToBrush();
            };
        }

        public TitleBarButtons TitleBarButtons
        {
            get => (TitleBarButtons)GetValue(TitleBarButtonsProperty);
            set => SetValue(TitleBarButtonsProperty, value);
        }
        public static readonly DependencyProperty TitleBarButtonsProperty =
            DependencyProperty.Register(nameof(TitleBarButtons), typeof(TitleBarButtons), typeof(DinaWindow),
                new PropertyMetadata(TitleBarButtons.CloseOnly));

        public ICommand CloseCommand
        {
            get => (ICommand)GetValue(CloseCommandProperty);
            set => SetValue(CloseCommandProperty, value);
        }
        public static readonly DependencyProperty CloseCommandProperty =
            DependencyProperty.Register(nameof(CloseCommand), typeof(ICommand), typeof(DinaWindow),
                new PropertyMetadata(null));

        public ICommand MinimizeCommand
        {
            get => (ICommand)GetValue(MinimizeCommandProperty);
            set => SetValue(MinimizeCommandProperty, value);
        }
        public static readonly DependencyProperty MinimizeCommandProperty =
            DependencyProperty.Register(nameof(MinimizeCommand), typeof(ICommand), typeof(DinaWindow),
                new PropertyMetadata(null));

        public ICommand MaximizeCommand
        {
            get => (ICommand)GetValue(MaximizeCommandProperty);
            set => SetValue(MaximizeCommandProperty, value);
        }
        public static readonly DependencyProperty MaximizeCommandProperty =
            DependencyProperty.Register(nameof(MaximizeCommand), typeof(ICommand), typeof(DinaWindow),
                new PropertyMetadata(null));

        public bool IsMaximized
        {
            get => (bool)GetValue(IsMaximizedProperty);
            set => SetValue(IsMaximizedProperty, value);
        }
        public static readonly DependencyProperty IsMaximizedProperty =
            DependencyProperty.Register(nameof(IsMaximized), typeof(bool), typeof(DinaWindow),
                new PropertyMetadata(false));

        public Brush TitleBarBackground
        {
            get => (Brush)GetValue(TitleBarBackgroundProperty);
            set => SetValue(TitleBarBackgroundProperty, value);
        }
        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register(nameof(TitleBarBackground), typeof(Brush), typeof(DinaWindow),
                new PropertyMetadata(DinaColor.TitleBarBackground.ToBrush()));

        public Brush TitleBarForeground
        {
            get => (Brush)GetValue(TitleBarForegroundProperty);
            set => SetValue(TitleBarForegroundProperty, value);
        }
        public static readonly DependencyProperty TitleBarForegroundProperty =
            DependencyProperty.Register(nameof(TitleBarForeground), typeof(Brush), typeof(DinaWindow),
                new PropertyMetadata(DinaColor.TitleBarForeground.ToBrush()));

        public Brush TitleBarBorderBrush
        {
            get => (Brush)GetValue(TitleBarBorderBrushProperty);
            set => SetValue(TitleBarBorderBrushProperty, value);
        }
        public static readonly DependencyProperty TitleBarBorderBrushProperty =
            DependencyProperty.Register(nameof(TitleBarBorderBrush), typeof(Brush), typeof(DinaWindow),
                new PropertyMetadata(DinaColor.WindowBorder.ToBrush()));

        private void MaximizeWindow()
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
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                MaximizeWindow();
            }
            base.OnStateChanged(e);
        }
    }
}