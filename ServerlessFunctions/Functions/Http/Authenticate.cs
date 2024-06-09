using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ServerlessFunctions.Models.Messages;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Azure.Cosmos;
using ServerlessFunctions.Models.Documents;
using Azure.Core;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace ServerlessFunctions.Functions.Http
{
    public class Authenticate
    {
        private readonly ILogger<Authenticate> _logger;

        public Authenticate(ILogger<Authenticate> logger)
        {
            _logger = logger;
        }

        [Function(nameof(Authenticate))]
        public async Task<AuthenticateOutput> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [CosmosDBInput(Constants.CosmosNoSql.BlendDatabase, Connection = Constants.CosmosNoSql.Connection)] Database database)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var authorization = req.Headers[HeaderNames.Authorization];

            if (!AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return new AuthenticateOutput(new UnauthorizedResult());
            }

            if (string.IsNullOrEmpty(headerValue.Parameter))
            {
                return new AuthenticateOutput(new UnauthorizedResult());
            }

            var user = await GetUser(headerValue.Parameter);
            if (user == null)
            {
                return new AuthenticateOutput(new UnauthorizedResult());
            }

            var userContainer = database.GetContainer(Constants.CosmosNoSql.UsersContainer);
            var dbUser = userContainer.GetItemLinqQueryable<UserDocument>(true).AsEnumerable().FirstOrDefault(u => u.Id == user.Id);
            if (dbUser == null)
            {
                return new AuthenticateOutput(new UnauthorizedResult());
            }


            if (string.IsNullOrWhiteSpace(dbUser.DisplayName))
            {
                dbUser.DisplayName = user.DisplayName;
            }

            if (string.IsNullOrWhiteSpace(dbUser.Image))
            {
                dbUser.Image = user.Images.MaxBy(i => i.Width)?.Url;
            }

            var option = new CookieOptions
            {
                Expires = DateTime.Now.AddHours(5),
                HttpOnly = true,
            };

            req.HttpContext.Response.Cookies.Append(Constants.Cookie.Authentication, dbUser.Id, option);

            var processRequestsContainer = database.GetContainer(Constants.CosmosNoSql.ProcessRequestsContainer);
            var hasBeenProcessed = processRequestsContainer.GetItemLinqQueryable<ProcessRequestDocument>(true).AsEnumerable()
                .Any(u => u.Date == DateTime.Now.Date);

            ProcessBlendPlaylistMessage? message = null;
            if (!hasBeenProcessed)
            {
                message = new ProcessBlendPlaylistMessage() { BearerToken = headerValue.Parameter, UserId = dbUser.Id };
            }


            return new AuthenticateOutput(new UnauthorizedResult() , dbUser, message);
        }


        private static async Task<User?> GetUser(string bearerToken)
        {
            var client = new HttpClient();

            var url = $"https://api.spotify.com/v1/me";


            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get,
                Headers = { { "Authorization", $"Bearer {bearerToken}" } },
            };


            var response = await client.SendAsync(request);


            var result = await response.Content.ReadFromJsonAsync<User>();
            return result;
        }
        
        public class UserImage
        {
            [JsonPropertyName("url")]
            public string Url { get; set; }
            
            [JsonPropertyName("width")]
            public int Width { get; set; }
        }

        public class User
        {
            [JsonPropertyName("display_name")]
            public string DisplayName { get; set; }
            
            [JsonPropertyName("id")]
            public string Id { get; set; }

            [JsonPropertyName("images")]
            public List<UserImage> Images { get; set; }
        }

    }

    public class AuthenticateOutput(IActionResult httpResponse, UserDocument? document = null, ProcessBlendPlaylistMessage? message = null)
    {
        [CosmosDBOutput(Constants.CosmosNoSql.BlendDatabase, Constants.CosmosNoSql.UsersContainer, Connection = Constants.CosmosNoSql.Connection, CreateIfNotExists = true)]
        public UserDocument? Document { get; set; } = document;

        [ServiceBusOutput(Constants.ServiceBus.ProcessBlendPlaylistQueue, Connection = Constants.ServiceBus.Connection)]
        public ProcessBlendPlaylistMessage? Message { get; set; } = message;

        public IActionResult HttpResponse { get; set; } = httpResponse;
    }
}
