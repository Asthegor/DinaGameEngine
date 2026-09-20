using System.Text.Json.Serialization;

namespace DinaGameEngine.Models.Updater
{
    public class DinaCSharpReleaseInfo
    {
        [JsonPropertyName("tag_name")]
        public string TagName { get; set; } = string.Empty;

        [JsonPropertyName("body")]
        public string ChangelogMarkdown { get; set; } = string.Empty;

        [JsonPropertyName("assets")]
        public List<ReleaseAssetInfo> Assets { get; set; } = [];
    }
}
