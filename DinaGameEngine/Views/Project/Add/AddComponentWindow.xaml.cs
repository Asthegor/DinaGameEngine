using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels.Project.Add;

using System.Windows;
using System.Windows.Input;

namespace DinaGameEngine.Views.Project.Add
{
    public partial class AddComponentWindow : DinaWindow
    {
        public AddComponentWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            Loaded += OnLoaded;
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
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(KeyTextBox);
            KeyTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }
    }
}