using System.Windows;
using System.Windows.Controls;

namespace DinaGameEngine.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SceneCardView.xaml
    /// </summary>
    public partial class SceneCardView : UserControl
    {
        public SceneCardView()
        {
            InitializeComponent();
        }
        public string SceneName
        {
            get => (string)GetValue(SceneNameProperty);
            set => SetValue(SceneNameProperty, value);
        }
        public static readonly DependencyProperty SceneNameProperty =
            DependencyProperty.Register(nameof(SceneName), typeof(string), typeof(SceneCardView), new PropertyMetadata(string.Empty));

        public string SceneKey
        {
            get => (string)GetValue(SceneKeyProperty);
            set => SetValue(SceneKeyProperty, value);
        }
        public static readonly DependencyProperty SceneKeyProperty =
            DependencyProperty.Register(nameof(SceneKey), typeof(string), typeof(SceneCardView), new PropertyMetadata(string.Empty));
        public int ComponentCount
        {
            get => (int)GetValue(ComponentCountProperty);
            set => SetValue(ComponentCountProperty, value);
        }
        public static readonly DependencyProperty ComponentCountProperty =
            DependencyProperty.Register(nameof(ComponentCount), typeof(int), typeof(SceneCardView), new PropertyMetadata(0));
    }
}
