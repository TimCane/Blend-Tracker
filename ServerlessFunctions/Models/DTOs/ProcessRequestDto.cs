using AutoMapper;
using ServerlessFunctions.Models.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ServerlessFunctions.Models.DTOs
{
    public class ProcessRequestDto
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string UserId { get; set; }
    }

    public class ProcessRequestDtoProfile : Profile
    {
        public ProcessRequestDtoProfile()
        {
            this.CreateMap<ProcessRequestDocument, ProcessRequestDto>();
        }
    }
}
