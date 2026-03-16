using DinaGameEngine.Common;

using System.Windows.Markup;

namespace DinaGameEngine.Extensions
{
    public class TranslateExtension : MarkupExtension
    {
        public string Key { get; set; } = string.Empty;

        public TranslateExtension() { }
        public TranslateExtension(string key) { Key = key; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return LocalizationManager.GetTranslation(Key);
        }
    }
}