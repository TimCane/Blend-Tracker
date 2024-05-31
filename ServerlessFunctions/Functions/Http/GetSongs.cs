using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using ServerlessFunctions.Models.Documents;
using AutoMapper;
using ServerlessFunctions.Models.DTOs;

namespace ServerlessFunctions.Functions.Http
{
    public class GetSongs
    {
        private readonly ILogger<GetSongs> _logger;
        private readonly IMapper _mapper;

        public GetSongs(ILogger<GetSongs> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [Function(nameof(GetSongs))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [CosmosDBInput(Constants.CosmosNoSql.BlendDatabase, Constants.CosmosNoSql.SongsContainer, Connection = Constants.CosmosNoSql.Connection)] Container container, [FromQuery] int take = 10, [FromQuery] int skip = 0)
        {
            var bearerToken = req.Cookies[Constants.Cookie.Authentication];
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return new UnauthorizedResult();
            }


            var songs = container.GetItemLinqQueryable<SongDocument>(true).Skip(skip).Take(take).AsEnumerable()
                .ToList();
            
            return new OkObjectResult(_mapper.Map<List<SongDto>>(songs));

        }
    }
}
