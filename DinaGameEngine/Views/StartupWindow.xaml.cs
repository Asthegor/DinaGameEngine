using DinaGameEngine.Models;
using DinaGameEngine.ViewModels;

using System.Windows;

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

            startupViewModel.ProjectOpened += OnProjectOpened;
        }

        private void OnProjectOpened(object? sender, GameProjectModel e)
        {
            Close();
        }
    }
}
