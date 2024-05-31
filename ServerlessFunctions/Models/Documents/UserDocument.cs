using System.Text.Json.Serialization;

namespace ServerlessFunctions.Models.Documents
{
    public class UserDocument
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public string? DisplayName { get; set; }
        
        public string? Image { get; set; }
    }
}
