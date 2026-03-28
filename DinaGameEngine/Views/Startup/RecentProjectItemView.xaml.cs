using System.Windows;
using System.Windows.Controls;

namespace DinaGameEngine.Views.Startup
{
    /// <summary>
    /// Logique d'interaction pour RecentProjectItemView.xaml
    /// </summary>
    public partial class RecentProjectItemView : UserControl
    {
        public RecentProjectItemView()
        {
            InitializeComponent();
        }

        public string IconPath
        {
            get => (string)GetValue(IconPathProperty);
            set => SetValue(IconPathProperty, value);
        }
        public static readonly DependencyProperty IconPathProperty =
            DependencyProperty.Register(nameof(IconPath), typeof(string), typeof(RecentProjectItemView), new PropertyMetadata(string.Empty));

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(RecentProjectItemView), new PropertyMetadata(string.Empty));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register(nameof(Path), typeof(string), typeof(RecentProjectItemView), new PropertyMetadata(string.Empty));
        public string LastOpenedAt
        {
            get => (string)GetValue(LastOpenedAtProperty);
            set => SetValue(LastOpenedAtProperty, value);
        }
        public static readonly DependencyProperty LastOpenedAtProperty =
            DependencyProperty.Register(nameof(LastOpenedAt), typeof(string), typeof(RecentProjectItemView), new PropertyMetadata(string.Empty));
        public bool IsPinned
        {
            get => (bool)GetValue(IsPinnedProperty);
            set => SetValue(IsPinnedProperty, value);
        }
        public static readonly DependencyProperty IsPinnedProperty =
            DependencyProperty.Register(nameof(IsPinned), typeof(bool), typeof(RecentProjectItemView), new PropertyMetadata(false));

    }
}
