using DinaGameEngine.Abstractions;
using DinaGameEngine.Commands;
using DinaGameEngine.Common;
using DinaGameEngine.Common.Enums;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Add
{
    public class AddFontViewModel : AddItemViewModel<string>
    {
        private float _size;
        private SpriteFontStyle _style;
        private float _spacing;
        private readonly string _rootPath;
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private string _ttfFolder;

        public AddFontViewModel(IEnumerable<string> existingKeys, string rootPath,
                                IFileService fileService, IDialogService dialogService,
                                FontModel? fontModel = null)
            : base(existingKeys, "AddFont_Title", fontModel != null)
        {
            _rootPath = rootPath;
            _fileService = fileService;
            _dialogService = dialogService;
            _ttfFolder = _fileService.Combine(_rootPath, "Fonts", "FontContent", "TTF_Files");

            AvailableStyles = [.. Enum.GetValues<SpriteFontStyle>()];

            BrowseTtfFileCommand = new RelayCommand(_ => BrowseTtfFile());

            RefreshTtfFiles();

            if (fontModel != null)
            {
                Key = fontModel.Key;
                Size = fontModel.Size;
                Style = fontModel.Style;
                Spacing = fontModel.Spacing;
                SelectedNamedItem = NamedItems.FirstOrDefault(n => _fileService.GetFileName(n.Item) == _fileService.GetFileName(fontModel.TtfRelativePath));
            }
        }

        public float Size
        {
            get => _size;
            set
            {
                SetProperty(ref _size, value);
                AddCommand.RaiseCanExecuteChanged();
            }
        }

        public SpriteFontStyle Style
        {
            get => _style;
            set => SetProperty(ref _style, value);
        }

        public float Spacing
        {
            get => _spacing;
            set => SetProperty(ref _spacing, value);
        }

        public IEnumerable<SpriteFontStyle> AvailableStyles { get; }

        public RelayCommand BrowseTtfFileCommand { get; }

        protected override bool CanConfirm()
            => base.CanConfirm() && SelectedNamedItem != null && Size > 0;

        protected override void OnSelectedNamedItemChanged(NamedItem<string>? item)
        {
            AddCommand.RaiseCanExecuteChanged();
        }

        private void RefreshTtfFiles()
        {
            if (!_fileService.DirectoryExists(_ttfFolder))
                return;

            var files = _fileService.GetFiles(_ttfFolder, "*.ttf", recursive: true);
            NamedItems = [.. files.Select(f => new NamedItem<string>
            {
                Name = _fileService.GetFileName(f),
                Item = f
            })];
            OnPropertyChanged(nameof(NamedItems));
        }

        private void BrowseTtfFile()
        {
            var selectedFile = _dialogService.OpenFileDialog(
                LocalizationManager.GetTranslation("AddFont_BrowseTtf_Title"),
                LocalizationManager.GetTranslation("AddFont_BrowseTtf_File"));

            if (selectedFile == null)
                return;

            string destinationFile;

            if (selectedFile.StartsWith(_ttfFolder, StringComparison.OrdinalIgnoreCase))
            {
                destinationFile = selectedFile;
            }
            else
            {
                if (!_fileService.DirectoryExists(_ttfFolder))
                    _fileService.CreateDirectory(_ttfFolder);

                destinationFile = _fileService.Combine(_ttfFolder, _fileService.GetFileName(selectedFile));
                _fileService.CopyFile(selectedFile, destinationFile);
            }

            RefreshTtfFiles();
            SelectedNamedItem = NamedItems.FirstOrDefault(n => n.Item == destinationFile);
        }
    }
}