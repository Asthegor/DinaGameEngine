using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels.Project.Add;

using System.Windows;

namespace DinaGameEngine.Views.Project.Add
{
    /// <summary>
    /// Logique d'interaction pour AddSceneWindow.xaml
    /// </summary>
    public partial class AddSceneWindow : DinaWindow
    {
        public AddSceneWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is AddSceneViewModel vm)
            {
                vm.SceneConfirmed += (s, args) => Close();
            }
        }
    }
}
