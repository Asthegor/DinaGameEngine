using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;
using DinaGameEngine.ViewModels.Shared;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public abstract class ItemViewModel : ObservableObject
    {
        protected readonly ItemModel _model;
        private bool _isSelected;
        public ItemViewModel(ItemModel model)
        {
            _model = model;

            OpenCommand = new RelayCommand(_ => Open());
            DeleteCommand = new RelayCommand(_ => Delete());
            SelectCommand = new RelayCommand(_ => Select());

            NavigationButtons = new ButtonBarViewModel();
            CreateButtons();
        }
        public ItemModel Model => _model;
        public Guid ItemId => _model.Id;
        public abstract string Icon { get; }
        public abstract string Name { get; }
        public abstract string Key { get; }

        public event EventHandler? ItemOpened;
        public event EventHandler? ItemDeleted;
        public event EventHandler? ItemSelected;

        public RelayCommand OpenCommand { get; }
        private void Open()
        {
            ItemOpened?.Invoke(_model, EventArgs.Empty);
        }
        public RelayCommand DeleteCommand { get; }
        private void Delete()
        {
            ItemDeleted?.Invoke(this, EventArgs.Empty);
        }
        public RelayCommand SelectCommand { get; }
        private void Select()
        {
            ItemSelected?.Invoke(this, EventArgs.Empty);
        }

        public ButtonBarViewModel NavigationButtons { get; }
        private void CreateButtons()
        {
            NavigationButtons.Buttons.Clear();
            NavigationButtons.Buttons.Add(new ButtonDescriptor { Icon = DinaIcon.Open.ToGlyph(), Command = OpenCommand, Role = ButtonRole.Neutral });
            NavigationButtons.Buttons.Add(new ButtonDescriptor { Icon = DinaIcon.Delete.ToGlyph(), Command = DeleteCommand, Role = ButtonRole.Secondary });
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }
    }
}
