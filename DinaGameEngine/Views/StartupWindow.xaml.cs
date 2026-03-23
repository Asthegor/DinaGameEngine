using DinaGameEngine.Models;
using DinaGameEngine.ViewModels;

using System.Windows;
using System.Windows.Input;

namespace DinaGameEngine.Views
{
    /// <summary>
    /// Logique d'interaction pour StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow(StartupViewModel startupViewModel)
        {
            InitializeComponent();

            DataContext = startupViewModel;
        }

        // Permet de déplacer la fenêtre en cliquant sur la barre de titre
        private void TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
