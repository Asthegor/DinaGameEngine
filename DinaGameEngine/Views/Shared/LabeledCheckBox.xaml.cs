using DinaGameEngine.Commands;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DinaGameEngine.Views.Shared
{
    /// <summary>
    /// Interaction logic for LabeledCheckBox.xaml
    /// </summary>
    public partial class LabeledCheckBox : UserControl
    {
        public LabeledCheckBox()
        {
            InitializeComponent();
        }
        public ICommand ToggleCommand => new RelayCommand(_ => IsChecked = !IsChecked);
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabeledCheckBox), new PropertyMetadata(string.Empty));

        public bool? IsChecked
        {
            get => (bool?)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }
        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register(nameof(IsChecked), typeof(bool?), typeof(LabeledCheckBox), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
