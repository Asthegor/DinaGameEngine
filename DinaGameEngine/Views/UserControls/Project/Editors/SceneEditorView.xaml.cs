using System.Windows;
using System.Windows.Controls;

namespace DinaGameEngine.Views.UserControls.Project.Editors
{
    /// <summary>
    /// Interaction logic for SceneEditorView.xaml
    /// </summary>
    public partial class SceneEditorView : UserControl
    {
        public SceneEditorView()
        {
            InitializeComponent();
        }
        private void AddComponentButton_Click(object sender, RoutedEventArgs e)
        {
            AddComponentButton.ContextMenu.DataContext = AddComponentButton.DataContext;
            AddComponentButton.ContextMenu.IsOpen = true;
        }
    }
}
