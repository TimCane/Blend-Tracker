using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ServerlessFunctions.Models.Documents;
using ServerlessFunctions.Models.Messages;

namespace ServerlessFunctions.Functions
{
    public class ProcessBlendPlaylist(ILogger<ProcessBlendPlaylist> logger)
    {
        private static readonly string
            PlaylistId = Environment.GetEnvironmentVariable("Spotify_BlendPlaylistId") ?? string.Empty;

        private const string Market = "GB";
        private const string Fields = "items(added_by.id%2Ctrack(id%2Cname%2Cexplicit%2Cduration_ms%2Cartists(id%2Cname)%2Calbum(name%2Cid%2Cimages(url,width))))";

        [Function(nameof(ProcessBlendPlaylist))]
        public async Task<ProcessBlendPlaylistOutput> Run(
            [ServiceBusTrigger(Constants.ServiceBus.ProcessBlendPlaylistQueue, Connection = Constants.ServiceBus.Connection)] ProcessBlendPlaylistMessage message)
        {
            logger.LogInformation("Message AuthToken: {authToken}", message.BearerToken);

            if (string.IsNullOrWhiteSpace(message.BearerToken))
            {
                return new ProcessBlendPlaylistOutput([]);
            }

            var messages = new List<ProcessBlendSongMessage>();

            var items = await GetBlendPlaylistItems(message.BearerToken);
            foreach (var item in items)
            {
                messages.Add(new ProcessBlendSongMessage()
                {
                    Title = item.Track.Name,
                    Duration = item.Track.DurationMs,
                    Explicit = item.Track.Explicit,
                    Id = item.Track.Id,
                    AddedBy = new Models.Documents.AddedBy()
                    {
                        Id = item.AddedBy.Id,
                        AddedOn = DateTime.UtcNow
                    },
                    Album = new SongAlbum()
                    {
                        Id = item.Track.Album.Id,
                        Name = item.Track.Album.Name,
                        Image = item.Track.Album.Images.MaxBy(i => i.Width)?.Url
                    },
                    Artists = item.Track.Artists.Select(a => new SongArtist()
                    {
                        Id = a.Id,
                        Name = a.Name
                    }).ToList()
                });
            }


            return new ProcessBlendPlaylistOutput([.. messages], new ProcessRequestDocument(){Date = DateTime.Now.Date, Id = Guid.NewGuid().ToString(), UserId = message.UserId});
        }

        private static async Task<List<BlendPlaylistItem>> GetBlendPlaylistItems(string bearerToken)
        {
            var client = new HttpClient();

            var url = $"https://api.spotify.com/v1/playlists/{PlaylistId}/tracks?market={Market}&fields={Fields}";


            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
                Headers = { { "Authorization", $"Bearer {bearerToken}" } },
            };


            var response = await client.SendAsync(request);


            var result = await response.Content.ReadFromJsonAsync<BlendPlaylist>();
            return result.Items;
        }


        public class AddedBy
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
        }

        public class BlendTrackAlbum
        {
            [JsonPropertyName("images")]
            public List<AlbumImage> Images { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("id")]
            public string Id { get; set; }
        }

        public class BlendTrackArtist
        {
            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("id")]
            public string Id { get; set; }
        }

        public class AlbumImage
        {
            [JsonPropertyName("url")]
            public string? Url { get; set; }

            [JsonPropertyName("width")]
            public int Width { get; set; }
        }

        public class BlendPlaylistItem
        {
            [JsonPropertyName("track")]
            public BlendTrack Track { get; set; }

            [JsonPropertyName("added_by")]
            public AddedBy AddedBy { get; set; }
        }

        public class BlendPlaylist
        {
            [JsonPropertyName("items")]
            public List<BlendPlaylistItem> Items { get; set; }
        }

        public class BlendTrack
        {
            [JsonPropertyName("album")]
            public BlendTrackAlbum Album { get; set; }

            [JsonPropertyName("artists")]
            public List<BlendTrackArtist> Artists { get; set; }

            [JsonPropertyName("duration_ms")]
            public int DurationMs { get; set; }

            [JsonPropertyName("explicit")]
            public bool Explicit { get; set; }

            [JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonPropertyName("id")]
            public string Id { get; set; }
        }

        public class ProcessBlendPlaylistOutput(ProcessBlendSongMessage[] processBlendSongMessages, ProcessRequestDocument document = null)
        {
            [ServiceBusOutput(Constants.ServiceBus.ProcessBlendSongQueue, Connection = Constants.ServiceBus.Connection)]
            public ProcessBlendSongMessage[] ProcessBlendSongMessages { get; set; } = processBlendSongMessages;

            [CosmosDBOutput(Constants.CosmosNoSql.BlendDatabase, Constants.CosmosNoSql.ProcessRequestsContainer, Connection = Constants.CosmosNoSql.Connection, CreateIfNotExists = true)]
            public ProcessRequestDocument Document { get; set; } = document;
        }
    }
}
