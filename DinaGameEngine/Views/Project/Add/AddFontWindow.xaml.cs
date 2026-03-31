using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels.Project.Add;

using System.Windows;

namespace DinaGameEngine.Views.Project.Add
{
    /// <summary>
    /// Interaction logic for AddFontWindow.xaml
    /// </summary>
    public partial class AddFontWindow : DinaWindow
    {
        public AddFontWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is AddFontViewModel vm)
            {
                vm.ItemConfirmed += (s, args) => Close();
            }
        }
        private void KeyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddFontViewModel vm)
                vm.IsKeyFocused = true;
        }
        private void KeyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddFontViewModel vm)
                vm.IsKeyFocused = false;
        }
    }
}
