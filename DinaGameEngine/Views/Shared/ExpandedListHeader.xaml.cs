using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DinaGameEngine.Views.Shared
{
    /// <summary>
    /// Interaction logic for ExpandedListHeader.xaml
    /// </summary>
    public partial class ExpandedListHeader : UserControl
    {
        public ExpandedListHeader()
        {
            InitializeComponent();
        }
        public static string ChevronDownIcon => DinaIcon.ChevronDown.ToGlyph();
        public static string ChevronUpIcon => DinaIcon.ChevronUp.ToGlyph();
        public static string AddIcon => DinaIcon.Add.ToGlyph();

        public string Header
        {
            get => (string)GetValue(HeaderProperty);
            set => SetValue(HeaderProperty, value);
        }
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(ExpandedListHeader), new PropertyMetadata(string.Empty));

        public bool IsExpanded
        {
            get => (bool)GetValue(IsExpandedProperty);
            set => SetValue(IsExpandedProperty, value);
        }
        public static readonly DependencyProperty IsExpandedProperty =
            DependencyProperty.Register(nameof(IsExpanded), typeof(bool), typeof(ExpandedListHeader), new PropertyMetadata(false));

        public ICommand ToggleExpandCommand
        {
            get => (ICommand)GetValue(ToggleExpandCommandProperty);
            set => SetValue(ToggleExpandCommandProperty, value);
        }
        public static readonly DependencyProperty ToggleExpandCommandProperty =
            DependencyProperty.Register(nameof(ToggleExpandCommand), typeof(ICommand), typeof(ExpandedListHeader), new PropertyMetadata(null));

        public ICommand AddCommand
        {
            get => (ICommand)GetValue(AddCommandProperty);
            set => SetValue(AddCommandProperty, value);
        }
        public static readonly DependencyProperty AddCommandProperty =
            DependencyProperty.Register(nameof(AddCommand), typeof(ICommand), typeof(ExpandedListHeader), new PropertyMetadata(null));

    }
}
