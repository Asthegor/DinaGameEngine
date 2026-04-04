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

namespace DinaGameEngine.Views.Project.Components
{
    /// <summary>
    /// Interaction logic for Vector2InputView.xaml
    /// </summary>
    public partial class Vector2InputView : UserControl
    {
        public Vector2InputView()
        {
            InitializeComponent();
        }

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(Vector2InputView), new PropertyMetadata(string.Empty));
        public int? ValueX
        {
            get { return (int?)GetValue(ValueXProperty); }
            set { SetValue(ValueXProperty, value); }
        }
        public static readonly DependencyProperty ValueXProperty =
            DependencyProperty.Register(nameof(ValueX), typeof(int?), typeof(Vector2InputView), new PropertyMetadata(null));
        public int? ValueY
        {
            get { return (int?)GetValue(ValueYProperty); }
            set { SetValue(ValueYProperty, value); }
        }
        public static readonly DependencyProperty ValueYProperty =
            DependencyProperty.Register(nameof(ValueY), typeof(int?), typeof(Vector2InputView), new PropertyMetadata(null));

        public ICommand ResetCommand
        {
            get => (ICommand)GetValue(ResetCommandProperty);
            set => SetValue(ResetCommandProperty, value);
        }
        public static readonly DependencyProperty ResetCommandProperty =
            DependencyProperty.Register(nameof(ResetCommand), typeof(ICommand), typeof(Vector2InputView), new PropertyMetadata(null));

        public string ResetIcon
        {
            get => (string)GetValue(ResetIconProperty);
            set => SetValue(ResetIconProperty, value);
        }
        public static readonly DependencyProperty ResetIconProperty =
            DependencyProperty.Register(nameof(ResetIcon), typeof(string), typeof(Vector2InputView), new PropertyMetadata(string.Empty));
    }
}
