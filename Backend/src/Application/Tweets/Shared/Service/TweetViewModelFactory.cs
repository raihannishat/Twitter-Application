using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tweets.Shared.Service;
public class TweetViewModelFactory : ITweetViewModelFactory
{
    private readonly ILikeRepository _likeRepository;
    private readonly IRetweetRepository _retweetRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserRepository _userRepository;

    public TweetViewModelFactory(
        ILikeRepository likeRepository,
        IRetweetRepository retweetRepository,
        ICurrentUserService currentUserService,
        IUserRepository userRepository)
    {
        _likeRepository = likeRepository;
        _retweetRepository = retweetRepository;
        _currentUserService = currentUserService;
        _userRepository = userRepository;
    }

    public async Task<TweetViewModel> CreateTweetViewModelAsync(Tweet tweet, User user)
    {
        var isLikedByCurrentUser = await _likeRepository.FindOneByMatchAsync(x => x.TweetId == tweet.Id && x.UserId == _currentUserService.UserId);
        var isRetweetedByCurrentUser = await _retweetRepository.FindOneByMatchAsync(x => x.TweetId == tweet.Id && x.UserId == _currentUserService.UserId);
        var isRetweeted = await _retweetRepository.FindOneByMatchAsync(x => x.TweetId == tweet.Id && x.UserId == user.Id);
        var tweetCreator = await _userRepository.FindByIdAsync(tweet.UserId!);
        var tweetVm = new TweetViewModel()
        {
            Id = tweet.Id,
            Content = tweet.Content,
            CreatedAt = tweet.CreatedAt,
            Likes = tweet.Likes,
            Retweets = tweet.Retweets,
            Comments = tweet.Comments,
            UserId = user.Id,
            UserName = user.Name,
            Image = user.Image,
            TweetCreatorId = tweetCreator.Id,
            TweetCreatorName = tweetCreator.Name,
            TweetCreatorImage = tweetCreator.Image
        };

        if (isLikedByCurrentUser != null)
        {
            tweetVm.IsLikedByCurrentUser = true;
        }

        if (isRetweetedByCurrentUser != null)
        {
            tweetVm.IsRetweetedByCurrentUser = true;
        }

        if (isRetweeted != null)
        {
            tweetVm.IsRetweeted = true;
        }

        if (tweet.UserId == _currentUserService.UserId)
        {
            tweetVm.CanDelete = true;
        }

        return tweetVm;
    }
}

