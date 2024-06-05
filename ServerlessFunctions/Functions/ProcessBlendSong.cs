using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ServerlessFunctions.Models.Documents;
using ServerlessFunctions.Models.Messages;

namespace ServerlessFunctions.Functions
{
    public class ProcessBlendSong(ILogger<ProcessBlendSong> logger)
    {
        [Function(nameof(ProcessBlendSong))]
        public async Task<ProcessBlendSongOutput> Run(
            [ServiceBusTrigger(Constants.ServiceBus.ProcessBlendSongQueue, Connection = Constants.ServiceBus.Connection)] ProcessBlendSongMessage message,
            [CosmosDBInput(Constants.CosmosNoSql.BlendDatabase, Constants.CosmosNoSql.SongsContainer, Connection = Constants.CosmosNoSql.Connection, SqlQuery = "Select * from c where c.id = {Id}")] IEnumerable<SongDocument> songDocuments
            )
        {
            var songDocument = songDocuments.FirstOrDefault();
            if (songDocument == null)
            {
                // First time seeing this song
                songDocument = new SongDocument
                {
                    Id = message.Id,
                    SeenOn = new List<DateTime>(),
                    Duration = message.Duration,
                    Title = message.Title,
                    Explicit = message.Explicit,
                    Artists = message.Artists,
                    Album = message.Album,
                    AddedBy = new List<AddedBy>()
                };
            }

            songDocument.SeenOn.Add(DateTime.UtcNow);

            if (songDocument.AddedBy.All(by => by.Id != message.AddedBy.Id))
            {
                songDocument.AddedBy.Add(message.AddedBy);
            }

            return new ProcessBlendSongOutput(songDocument);
        }


        public class ProcessBlendSongOutput(SongDocument document)
        {
            [CosmosDBOutput(Constants.CosmosNoSql.BlendDatabase, Constants.CosmosNoSql.SongsContainer, Connection = Constants.CosmosNoSql.Connection, CreateIfNotExists = true)]
            public SongDocument? Document { get; set; } = document;
        }
    }
}
