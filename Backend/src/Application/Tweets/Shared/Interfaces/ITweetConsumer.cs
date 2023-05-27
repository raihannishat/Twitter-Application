namespace Application.Tweets.Shared.Interfaces;

public interface ITweetConsumer
{
    Task Connect(string currentUserId);
}
