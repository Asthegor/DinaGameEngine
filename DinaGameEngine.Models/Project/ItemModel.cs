using System.Text.Json.Serialization;

namespace DinaGameEngine.Models.Project
{
    public class ItemModel
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
