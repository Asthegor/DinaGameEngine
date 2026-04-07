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
    /// Interaction logic for ValidatedLabeledTextBox.xaml
    /// </summary>
    public partial class ValidatedLabeledTextBox : UserControl
    {
        public ValidatedLabeledTextBox()
        {
            InitializeComponent();
        }
        public bool IsKeyFocused
        {
            get => (bool)GetValue(IsKeyFocusedProperty);
            set => SetValue(IsKeyFocusedProperty, value);
        }
        public static readonly DependencyProperty IsKeyFocusedProperty =
            DependencyProperty.Register(nameof(IsKeyFocused), typeof(bool), typeof(ValidatedLabeledTextBox),
                new PropertyMetadata(false));
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(ValidatedLabeledTextBox), new PropertyMetadata(string.Empty));
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(ValidatedLabeledTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));


        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register(nameof(IsReadOnly), typeof(bool), typeof(ValidatedLabeledTextBox), new PropertyMetadata(false));

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }
        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(nameof(ErrorMessage), typeof(string), typeof(ValidatedLabeledTextBox), new PropertyMetadata(string.Empty));


    }
}
