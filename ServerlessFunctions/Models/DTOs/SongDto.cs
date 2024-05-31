using AutoMapper;
using ServerlessFunctions.Models.Documents;

namespace ServerlessFunctions.Models.DTOs
{
    public class SongDto
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public List<SongArtistDto> Artists { get; set; }
        public SongAlbumDto Album { get; set; }

        public List<AddedByDto> AddedBy { get; set; }

        public bool Explicit { get; set; }

        public int Duration { get; set; }

        public DateTime FirstSeen { get; set; }
        public DateTime LastSeen { get; set; }
    }

    public class AddedByDto
    {
        public string Id { get; set; }
        public DateTime AddedOn { get; set; }
    }

    public class SongArtistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class SongAlbumDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
    }


    public class SongDtoProfile: Profile
    {
        public SongDtoProfile()
        {
            this.CreateMap<SongDocument, SongDto>();
            this.CreateMap<SongArtist, SongArtistDto>();
            this.CreateMap<AddedBy, AddedByDto>();
            this.CreateMap<SongAlbum, SongAlbumDto>();
        }
    }
}
