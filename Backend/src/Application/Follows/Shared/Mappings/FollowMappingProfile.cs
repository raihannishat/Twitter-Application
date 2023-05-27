using Application.Follows.Shared.Models;

namespace Application.Follows.Shared.Mappings;

public class FollowMappingProfile : Profile
{
    public FollowMappingProfile()
    {
        CreateMap<Follow, FollowDto>().ReverseMap();
        CreateMap<User, UserProfileDto>().ReverseMap();
    }
}
