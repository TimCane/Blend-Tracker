using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ServerlessFunctions.Models.Documents;
using ServerlessFunctions.Models.DTOs;

namespace ServerlessFunctions.Functions.Http
{
    public class GetProcessRequests
    {
        private readonly ILogger<GetProcessRequests> _logger;
        private readonly IMapper _mapper;

        public GetProcessRequests(ILogger<GetProcessRequests> logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }

        [Function(nameof(GetProcessRequests))]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequest req,
            [CosmosDBInput(Constants.CosmosNoSql.BlendDatabase, Constants.CosmosNoSql.ProcessRequestsContainer, Connection = Constants.CosmosNoSql.Connection)] Container container)
        {
            var bearerToken = req.Cookies[Constants.Cookie.Authentication];
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                return new UnauthorizedResult();
            }

            var processRequests = container.GetItemLinqQueryable<ProcessRequestDocument>(true).AsEnumerable()
                .ToList();

            return new OkObjectResult(_mapper.Map<List<ProcessRequestDto>>(processRequests));

        }
    }
}
