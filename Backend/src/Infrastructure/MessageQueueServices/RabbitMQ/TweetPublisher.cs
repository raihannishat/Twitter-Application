using Application.Follows.Shared.Interfaces;
using Application.Tweets.Shared.Interfaces;
using Application.Tweets.Shared.Models;
using Infrastructure.Configurations;
using RabbitMQ.Client;
using System.Text.Json;

namespace Infrastructure.MessageQueueServices.RabbitMQ;

public class TweetPublisher : ITweetPublisher
{
    private readonly ConnectionFactory _factory;
    private readonly IFollowRepository _followRepository;

    public TweetPublisher(IRabbitMQSettings rabbitMQSettings, IFollowRepository followRepository)
    {
        _factory = new ConnectionFactory() { Uri = new Uri(rabbitMQSettings.URI) };
        _followRepository = followRepository;
    }
    public async Task SendPostToQueue(string currentUserId, Tweet tweet)
    {
        var followerList = await _followRepository.GetFollowers(x => x.FollowedId == currentUserId);

        var followerIdList = followerList.Select(x => x.FollowerId).ToList();

        string exchange = currentUserId;

        using (var connection = _factory.CreateConnection())

        using (IModel channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange, ExchangeType.Fanout);

            foreach (var followerId in followerIdList)
            {
                channel.QueueDeclare(followerId, true, false, false, null);
            }

            foreach (var followerId in followerIdList)
            {
                channel.QueueBind(followerId, exchange, "");
            }

            var publisherMessages = JsonSerializer.Serialize(tweet);

            var body = Encoding.UTF8.GetBytes(publisherMessages);

            channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);

        }

        return;
    }
}
