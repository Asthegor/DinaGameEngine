using DinaGameEngine.Commands;
using DinaGameEngine.Common.Enums;

using System.Windows;
using System.Windows.Input;

namespace DinaGameEngine.Themes
{
    public class DinaWindow : Window
    {
        public DinaWindow()
        {
            CloseCommand = new RelayCommand(_ => Close());
            MinimizeCommand = new RelayCommand(_ => WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(_ => WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized);
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
            get { return (ICommand)GetValue(MaximizeCommandProperty); }
            set { SetValue(MaximizeCommandProperty, value); }
        }
        public static readonly DependencyProperty MaximizeCommandProperty =
            DependencyProperty.Register(nameof(MaximizeCommand), typeof(ICommand), typeof(DinaWindow), new PropertyMetadata(null));

    }
}
