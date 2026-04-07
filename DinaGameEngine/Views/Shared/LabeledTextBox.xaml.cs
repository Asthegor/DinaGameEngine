using System.Windows;
using System.Windows.Controls;

namespace DinaGameEngine.Views.Shared
{
    /// <summary>
    /// Interaction logic for LabeledTextBox.xaml
    /// </summary>
    public partial class LabeledTextBox : UserControl
    {
        public LabeledTextBox()
        {
            InitializeComponent();
        }
        public bool IsKeyFocused
        {
            get => (bool)GetValue(IsKeyFocusedProperty);
            set => SetValue(IsKeyFocusedProperty, value);
        }
        public static readonly DependencyProperty IsKeyFocusedProperty =
            DependencyProperty.Register(nameof(IsKeyFocused), typeof(bool), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabeledTextBox), new PropertyMetadata(string.Empty));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(LabeledTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(LabeledTextBox), new PropertyMetadata(false));

        private void TextBox_GotFocus(object sender, RoutedEventArgs e) => IsKeyFocused = true;

        private void TextBox_LostFocus(object sender, RoutedEventArgs e) => IsKeyFocused = false;


    }
}
