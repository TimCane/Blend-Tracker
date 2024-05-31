using System.Text.Json.Serialization;

namespace ServerlessFunctions.Models.Documents
{
    public class SongDocument
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        
        public string Title { get; set; }
        public List<SongArtist> Artists { get; set; }
        public SongAlbum Album { get; set; }
        
        public List<AddedBy> AddedBy { get; set; }

        public bool Explicit { get; set; }

        public int Duration { get; set; }

        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
    }

    public class AddedBy
    {
        public string Id { get; set; }
        public DateTime AddedOn { get; set; }
    }

    public class SongArtist
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class SongAlbum
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
    }
}
