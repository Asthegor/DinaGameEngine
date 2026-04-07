using DinaGameEngine.Themes;
using DinaGameEngine.ViewModels.Project.Add;

using System.Windows;
using System.Windows.Input;

namespace DinaGameEngine.Views.Project.Add
{
    /// <summary>
    /// Interaction logic for AddColorWindow.xaml
    /// </summary>
    public partial class AddColorWindow : DinaWindow
    {
        public AddColorWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            Loaded += OnLoaded;
        }
        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is AddColorViewModel vm)
            {
                vm.ItemConfirmed += (s, args) => Close();
            }
        }
        private void KeyTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddColorViewModel vm)
                vm.IsKeyFocused = true;
        }
        private void KeyTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddColorViewModel vm)
                vm.IsKeyFocused = false;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(KeyTextBox);
            KeyTextBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }
    }
}
