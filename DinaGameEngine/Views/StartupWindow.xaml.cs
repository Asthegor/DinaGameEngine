using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels;

namespace DinaGameEngine.Views
{
    /// <summary>
    /// Logique d'interaction pour StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : DinaWindow
    {
        public StartupWindow(StartupViewModel startupViewModel)
        {
            InitializeComponent();

            DataContext = startupViewModel;
        }
    }
}
