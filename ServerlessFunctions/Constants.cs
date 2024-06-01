namespace ServerlessFunctions
{
    public class Constants
    {
        public class CosmosNoSql
        {
            public const string Connection = "blendtrackercosno_NOSQLDB";
            public const string BlendDatabase = "Blend";
            public const string SongsContainer = "Songs";
            public const string UsersContainer = "Users";
            public const string ProcessRequestsContainer = "ProcessRequests";
        }

        public class ServiceBus
        {
            public const string Connection = "blendtrackersbns_SERVICEBUS";
            public const string ProcessBlendSongQueue = "process-blend-song";
            public const string ProcessBlendPlaylistQueue = "process-blend-playlist";
        }

        public class Cookie
        {
            public const string Authentication = "Authentication";
        }
    }
}
