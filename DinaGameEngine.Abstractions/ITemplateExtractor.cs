using DinaGameEngine.Common;
using DinaGameEngine.Models;

namespace DinaGameEngine.Abstractions
{
    public interface ITemplateExtractor
    {
        public bool Extract(TemplateType type, string rootPath, List<TemplateMarkerModel> markers);
        public List<TemplateMarkerModel>? GetMarkers<T>(TemplateType type, T model);
    }
}
