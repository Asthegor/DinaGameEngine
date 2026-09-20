using System.Text.Json.Serialization;

namespace DinaGameEngine.Models.Updater
{
    public class ReleaseAssetInfo
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("browser_download_url")]
        public string DownloadUrl { get; set; } = string.Empty;
    }
}
