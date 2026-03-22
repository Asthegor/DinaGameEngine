using DinaGameEngine.ViewModels;

using System.Windows;

namespace DinaGameEngine.Views
{
    /// <summary>
    /// Logique d'interaction pour LanguageSelectionWindow.xaml
    /// </summary>
    public partial class LanguageSelectionWindow : Window
    {
        public LanguageSelectionWindow(LanguageSelectionViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
