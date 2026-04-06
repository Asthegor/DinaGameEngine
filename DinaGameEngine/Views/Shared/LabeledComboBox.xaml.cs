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
    /// Interaction logic for LabeledComboBox.xaml
    /// </summary>
    public partial class LabeledComboBox : UserControl
    {
        public LabeledComboBox()
        {
            InitializeComponent();
        }
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(LabeledComboBox), new PropertyMetadata(string.Empty));

        public System.Collections.IEnumerable AvailableKeys
        {
            get => (System.Collections.IEnumerable)GetValue(AvailableKeysProperty);
            set => SetValue(AvailableKeysProperty, value);
        }
        public static readonly DependencyProperty AvailableKeysProperty =
            DependencyProperty.Register(nameof(AvailableKeys), typeof(System.Collections.IEnumerable), typeof(LabeledComboBox), new PropertyMetadata(null));

        public object? SelectedKey
        {
            get => GetValue(SelectedKeyProperty);
            set => SetValue(SelectedKeyProperty, value);
        }
        public static readonly DependencyProperty SelectedKeyProperty =
            DependencyProperty.Register(nameof(SelectedKey), typeof(object), typeof(LabeledComboBox), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }
        public static readonly DependencyProperty DisplayMemberPathProperty =
            DependencyProperty.Register(nameof(DisplayMemberPath), typeof(string), typeof(LabeledComboBox), new PropertyMetadata(string.Empty));
    }
}
