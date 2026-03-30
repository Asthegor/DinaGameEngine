using System.Windows;
using System.Windows.Controls;

namespace DinaGameEngine.Views.Project.Components
{
    /// <summary>
    /// Logique d'interaction pour ColorSliderView.xaml
    /// </summary>
    public partial class ColorSliderView : UserControl
    {
        public ColorSliderView()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(ColorSliderView), new PropertyMetadata(string.Empty));
        public byte ColorValue
        {
            get => (byte)GetValue(ColorValueProperty);
            set => SetValue(ColorValueProperty, value);
        }
        public static readonly DependencyProperty ColorValueProperty =
            DependencyProperty.Register(nameof(ColorValue), typeof(byte), typeof(ColorSliderView), new PropertyMetadata((byte)0));

    }
}
