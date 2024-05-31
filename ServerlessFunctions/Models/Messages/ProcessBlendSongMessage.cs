using ServerlessFunctions.Models.Documents;

namespace ServerlessFunctions.Models.Messages
{
    public class ProcessBlendSongMessage
    {
        // Spotify Song Id
        public string Id { get; set; }
        public int Duration { get; set; }
        public string Title { get; set; }
        public bool Explicit { get; set; }

        public List<SongArtist> Artists { get; set; }
        public SongAlbum Album { get; set; }

        public AddedBy AddedBy { get; set; }
    }
}
