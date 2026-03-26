using DinaGameEngine.Common;

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DinaGameEngine.ViewModels
{
    public class ButtonBarViewModel : ObservableObject
    {
        public ObservableCollection<ButtonDescriptor> Buttons { get; set; } = [];
        public Orientation Orientation { get; set; } = Orientation.Horizontal;
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Stretch;
        public VerticalAlignment VerticalAlignment { get; set; } = VerticalAlignment.Top;
        public double ButtonHeight { get; set; } = 40;
        public Thickness ButtonPadding { get; set; } = new Thickness(0);
        public int Rows => Orientation == Orientation.Horizontal ? 1 : 0;
        public int Columns => Orientation == Orientation.Vertical ? 1 : 0;
    }
}
