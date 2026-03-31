using DinaGameEngine.Models.Project;

using System.Windows.Media;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddColorViewModel : AddItemViewModel<Color>
    {
        private byte _r = 255;
        private byte _g = 255;
        private byte _b = 255;
        private byte _a = 255;

        public AddColorViewModel(IEnumerable<string> existingKeys, ColorModel? colorModel = null)
            : base(existingKeys, "AddColor_Title", colorModel != null)
        {
            NamedItems = [.. typeof(Colors).GetProperties()
                .Select(p => new NamedItem<Color>
                {
                    Name = p.Name,
                    Item = (Color)p.GetValue(null)!
                })];

            if (colorModel != null)
            {
                Key = colorModel.Key;
                R = colorModel.R;
                G = colorModel.G;
                B = colorModel.B;
                A = colorModel.A;

                SelectedNamedItem = NamedItems.FirstOrDefault(c =>
                    c.Item.R == R && c.Item.G == G &&
                    c.Item.B == B && c.Item.A == A);
            }
        }

        public byte R
        {
            get => _r;
            set
            {
                SetProperty(ref _r, value);
                OnPropertyChanged(nameof(PreviewColor));
                if (!_isUpdatingFromNamedItem)
                    SelectedNamedItem = null;
            }
        }
        public byte G
        {
            get => _g;
            set
            {
                SetProperty(ref _g, value);
                OnPropertyChanged(nameof(PreviewColor));
                if (!_isUpdatingFromNamedItem)
                    SelectedNamedItem = null;
            }
        }
        public byte B
        {
            get => _b;
            set
            {
                SetProperty(ref _b, value);
                OnPropertyChanged(nameof(PreviewColor));
                if (!_isUpdatingFromNamedItem)
                    SelectedNamedItem = null;
            }
        }
        public byte A
        {
            get => _a;
            set
            {
                SetProperty(ref _a, value);
                OnPropertyChanged(nameof(PreviewColor));
                if (!_isUpdatingFromNamedItem)
                    SelectedNamedItem = null;
            }
        }

        public Color PreviewColor
        {
            get => Color.FromArgb(A, R, G, B);
            set
            {
                SelectedNamedItem = null;
                R = value.R;
                G = value.G;
                B = value.B;
                A = value.A;
                OnPropertyChanged();
            }
        }

        protected override void OnSelectedNamedItemChanged(NamedItem<Color>? item)
        {
            if (item == null)
                return;
            R = item.Item.R;
            G = item.Item.G;
            B = item.Item.B;
            A = item.Item.A;
        }
    }
}