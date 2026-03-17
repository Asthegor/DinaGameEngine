using Microsoft.Xna.Framework;

namespace __RootNamespace__.Core.Datas
{
    [Serializable]
    public class ConfigData
    {
        public bool Fullscreen { get; set; }
        public int ResolutionWidth { get; set; }
        public int ResolutionHeight { get; set; }
        public float MasterVolume { get; set; }
        public float MusicVolume { get; set; }
        public float SoundsVolume { get; set; }
        public string Language { get; set; } = "";
    }
    public class DefaultConfigData : ConfigData
    {
        public DefaultConfigData(Point resolution, bool fullscreen)
        {
            ResolutionWidth = resolution.X;
            ResolutionHeight = resolution.Y;
            Fullscreen = fullscreen;
            MasterVolume = 100f;
            MusicVolume = 100f;
            SoundsVolume = 100f;
            Language = "fr";
        }
    }
}
