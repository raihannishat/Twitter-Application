//using Application.Tweets.Shared.Interfaces;
//using Application.Tweets.Shared.Models;
//using System.Text.RegularExpressions;

//namespace Application.Tweets.Commands.UpdateTweet;

//public class UpdateTweetCommandHandler : IRequestHandler<UpdateTweetCommand, Result<Unit>>
//{
//    private readonly ITweetRepository _tweetRepository;
//    private readonly IHashtagRepository _hashtagRepository;
//    private readonly ITweetCacheService _tweetCacheService;
//    private readonly IMapper _mapper;
//    private readonly ICurrentUserService _currentUserService;

//    public UpdateTweetCommandHandler(ITweetRepository tweetRepository, IMapper mapper, ICurrentUserService currentUserService, ITweetCacheService tweetCacheService, IHashtagRepository hashtagRepository)
//    {
//        _tweetRepository = tweetRepository;
//        _mapper = mapper;
//        _currentUserService = currentUserService;
//        _tweetCacheService = tweetCacheService;
//        _hashtagRepository = hashtagRepository;
//    }

//    public async Task<Result<Unit>> Handle(UpdateTweetCommand request, CancellationToken cancellationToken)
//    {
//        try
//        {
//            var tweet = await _tweetRepository.FindByIdAsync(request.Id!);
//            if(tweet.UserId != _currentUserService.UserId || tweet == null)
//            {
//                return null;
//            }
            
//            tweet.Content = request.Content;
//            tweet.Edited = true;

//            await _tweetRepository.ReplaceOneAsync(tweet);

//            var hashtags = await ExtractHashTag(request.Content!);

//            if (hashtags.Count > 0)
//            {
//                await UpdateHashtagInMongo(hashtags, tweet.Id);
//            }

//            var tweetDto = _mapper.Map<TweetDto>(tweet);

//            if (tweet.Comments != null)
//            {
//                tweetDto.Comment = tweet.Comments.Count;
//            }

//            await _tweetCacheService.UpdateTweetAsync(tweetDto);

//            await _tweetCacheService.InsertHashtags(hashtags);

//            return Result<Unit>.Success(Unit.Value);
//        }
//        catch
//        {
//            return Result<Unit>.Failure("Failed to update tweet");
//        }
//    }


//    private async Task<List<string>> ExtractHashTag(string content)
//    {
//        var regex = new Regex(@"#\w+");

//        var matches = regex.Matches(content);

//        var hashtags = new HashSet<string>();

//        if (matches.Count == 0)
//        {
//            return new List<string>();
//        }

//        foreach (var match in matches)
//        {
//            hashtags.Add(match.ToString()!.ToLower());
//        }

//        return hashtags.ToList();
//    }

//    private async Task UpdateHashtagInMongo(List<string> hashtags, string tweetId)
//    {
//        foreach (var tag in hashtags)
//        {
//            var tagExist = await _hashtagRepository.FindOneAsync(x => x.Name == tag);
//            if (tagExist == null)
//            {
//                var entity = new Hashtag
//                {
//                    Name = tag,
//                    TweetIds = new List<string>() { tweetId }
//                };
//                await _hashtagRepository.InsertOneAsync(entity);
//            }
//        }
//    }
//}