using DinaGameEngine.Common.Enums;
using DinaGameEngine.Extensions;
using DinaGameEngine.Models.Project;

namespace DinaGameEngine.ViewModels.Project.Items
{
    public class SceneCardViewModel(SceneModel sceneModel) : ItemViewModel(sceneModel)
    {
        public override string Icon => DinaIcon.LookupEntities.ToGlyph();
        public override string Name => ((SceneModel)Model).Name;
        public override string Key => ((SceneModel)Model).Key;
        public int ComponentsCount => ((SceneModel)Model).Components.Count;
    }
}
