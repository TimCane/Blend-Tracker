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
    public class GetUsers
    {
        private readonly ILogger<GetUsers> _logger;
        private readonly IMapper _mapper;

        public GetUsers(ILogger<GetUsers> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [Function(nameof(GetUsers))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [CosmosDBInput(Constants.CosmosNoSql.BlendDatabase, Constants.CosmosNoSql.UsersContainer, Connection = Constants.CosmosNoSql.Connection)] Container container)
        {
            var bearerToken = req.Cookies[Constants.Cookie.Authentication];
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return new UnauthorizedResult();
            }

            var users = container.GetItemLinqQueryable<UserDocument>(true).AsEnumerable()
                .ToList();

            return new OkObjectResult(_mapper.Map<List<UserDto>>(users));

        }
    }
}
