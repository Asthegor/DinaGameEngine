using DinaGameEngine.ViewModels.Startup;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DinaGameEngine.Views.Startup
{
    /// <summary>
    /// Logique d'interaction pour ListRecentProjectsView.xaml
    /// </summary>
    public partial class ListRecentProjectsView : UserControl
    {
        public ListRecentProjectsView()
        {
            InitializeComponent();
        }
        private void OnProjectsListMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is DependencyObject source && GetVisualAncestorOrSelf<RecentProjectItemView>(source) != null)
                return;

            if (DataContext is StartupViewModel vm)
                vm.SelectedProject = null;
        }
        private static T? GetVisualAncestorOrSelf<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T t)
                    return t;
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }
    }
}
