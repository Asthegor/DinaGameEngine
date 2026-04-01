using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels.Project.Add;

using System.Windows;

namespace DinaGameEngine.Views.Project.Add
{
    public partial class AddComponentWindow : DinaWindow
    {
        public AddComponentWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is AddComponentViewModel vm)
                vm.ComponentConfirmed += (s, result) => Close();
        }

        private void KeyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddComponentViewModel vm)
                vm.IsKeyFocused = true;
        }

        private void KeyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddComponentViewModel vm)
                vm.IsKeyFocused = false;
        }
    }
}