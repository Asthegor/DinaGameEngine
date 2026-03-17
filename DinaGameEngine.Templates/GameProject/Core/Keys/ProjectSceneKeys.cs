using DinaCSharp.Services;
using DinaCSharp.Services.Scenes;

namespace __RootNamespace__.Core.Keys
{
    public class ProjectSceneKeys
    {
        public static readonly Key<SceneTag> MainMenu = Key<SceneTag>.FromString("MainMenu");
        public static readonly Key<SceneTag> OptionsMenu = Key<SceneTag>.FromString("OptionsMenu");
        public static readonly Key<SceneTag> GameScene = Key<SceneTag>.FromString("GameScene");

    }
}
