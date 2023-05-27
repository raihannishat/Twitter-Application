using Application.Tweets.Commands.CreateTweet;
using Application.Tweets.Commands.UpdateTweet;
using Application.Tweets.Shared.Models;

namespace Application.Tweets.Shared.Mappings;
public class TweetMappingProfile : Profile
{
    public TweetMappingProfile()
    {
        CreateMap<Tweet, CreateTweetCommand>().ReverseMap();
        CreateMap<Tweet, UpdateTweetCommand>().ReverseMap();
        CreateMap<Tweet, TweetViewModel>().ReverseMap();
        CreateMap<Tweet, TweetDto>().ReverseMap();
        CreateMap<TweetDto, TweetViewModel>().ReverseMap();
        CreateMap<Search, HashtagVM>().ReverseMap();
    }
}
