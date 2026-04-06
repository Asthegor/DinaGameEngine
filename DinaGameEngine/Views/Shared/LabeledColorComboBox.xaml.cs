using DinaGameEngine.Models.Project;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DinaGameEngine.Views.Shared
{
    /// <summary>
    /// Interaction logic for LabeledColorComboBox.xaml
    /// </summary>
    public partial class LabeledColorComboBox : UserControl
    {
        public LabeledColorComboBox()
        {
            InitializeComponent();
        }
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabeledColorComboBox), new PropertyMetadata(string.Empty));

        public IEnumerable<ColorModel> AvailableColors
        {
            get => (IEnumerable<ColorModel>)GetValue(AvailableColorsProperty);
            set => SetValue(AvailableColorsProperty, value);
        }
        public static readonly DependencyProperty AvailableColorsProperty =
            DependencyProperty.Register(nameof(AvailableColors), typeof(IEnumerable<ColorModel>), typeof(LabeledColorComboBox), new PropertyMetadata(null));

        public ColorModel? SelectedColor
        {
            get => (ColorModel?)GetValue(SelectedColorProperty);
            set => SetValue(SelectedColorProperty, value);
        }
        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(ColorModel), typeof(LabeledColorComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
