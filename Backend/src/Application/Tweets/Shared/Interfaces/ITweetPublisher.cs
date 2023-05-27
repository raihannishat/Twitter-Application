using Application.Tweets.Shared.Models;

namespace Application.Tweets.Shared.Interfaces;

public interface ITweetPublisher
{
    Task SendPostToQueue(string currentUserId, Tweet tweet);
}
