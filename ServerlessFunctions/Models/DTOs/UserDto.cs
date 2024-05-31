using AutoMapper;
using ServerlessFunctions.Models.Documents;

namespace ServerlessFunctions.Models.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }

        public string? DisplayName { get; set; }

        public string? Image { get; set; }
    }

    public class UserDtoProfile : Profile
    {
        public UserDtoProfile()
        {
            this.CreateMap<UserDocument, UserDto>();
        }
    }
}
