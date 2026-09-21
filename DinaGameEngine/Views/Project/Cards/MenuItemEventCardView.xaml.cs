using System.Windows;
using System.Windows.Controls;

namespace DinaGameEngine.Views.Project.Cards
{
    public partial class MenuItemEventCardView : UserControl
    {
        public MenuItemEventCardView()
        {
            InitializeComponent();
        }

        private void AddActionButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (button.ContextMenu != null)
            {
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.IsOpen = true;
            }
        }
    }
}