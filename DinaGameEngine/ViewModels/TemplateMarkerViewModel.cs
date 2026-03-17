using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.ViewModels
{
    public class TemplateMarkerViewModel : ObservableObject
    {
        private readonly TemplateMarkerModel _model;

        public TemplateMarkerViewModel(TemplateMarkerModel model)
        {
            _model = model;
            ResetMarkerCommand = new RelayCommand(ExecuteResetMarker);
        }

        public string Key => _model.Key;
        public string Label => _model.Label;
        public string DefaultValue => _model.DefaultValue;

        public string Value
        {
            get => _model.Value;
            set
            {
                if (_model.Value == value)
                    return;
                
                _model.Value = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(IsNotDefaultValue));
                OnPropertyChanged(nameof(IsEmpty));
            }
        }

        public bool IsNotDefaultValue => _model.Value != _model.DefaultValue;
        public bool IsEmpty => string.IsNullOrEmpty(Value);

        public RelayCommand ResetMarkerCommand { get; }
        private void ExecuteResetMarker() => Value = DefaultValue;
    }
}
