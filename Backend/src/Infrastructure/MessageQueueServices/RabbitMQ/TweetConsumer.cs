using Application.Follows.Shared.Interfaces;
using Application.Tweets.Shared.Interfaces;
using Application.Tweets.Shared.Models;
using Infrastructure.Configurations;
//using Infrastructure.Persistence.RedisCaching;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.MessageQueueServices.RabbitMQ;

public class TweetConsumer : ITweetConsumer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly IFollowRepository _followRepository;
    private readonly ILogger<TweetConsumer> _logger;
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IHomeTimelineRepository _homeTimelineRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TweetConsumer(
        IServiceProvider serviceProvider,
        IMapper mapper,
        ILogger<TweetConsumer> logger,
        IRabbitMQSettings rabbitMQSettings,
        IFollowRepository followRepository, 
        IHomeTimelineRepository homeTimelineRepository, IHttpContextAccessor httpContextAccessor)
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _logger = logger;
        _factory = new ConnectionFactory() { Uri = new Uri(rabbitMQSettings.URI) };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _followRepository = followRepository;
        _homeTimelineRepository = homeTimelineRepository;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task Connect(string userId)
    {
        var currentUserId = userId;

        var followerList = await _followRepository.GetFollowers(x => x.FollowedId == currentUserId);

        var followerIdList = followerList.Select(x => x.FollowerId).ToList();

        if (!followerIdList.Any()) return;

        foreach (var followerId in followerIdList)
        {
            _channel.QueueDeclare(followerId, true, false, false, null);
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var data = Encoding.UTF8.GetString(body);

                var tweet = JsonSerializer.Deserialize<Tweet>(data);

                _logger.LogInformation(currentUserId + "  post tweet --->");

                _logger.LogInformation(followerId + "  got tweet --->");

                await InsertHomeTimlineInMongo(followerId, tweet, currentUserId);

                //await _tweetCacheService.InsertHomeTimeline(followerId, tweetId);
          
            };

            _channel.BasicConsume(followerId, true, consumer);
        }
       
    }

    private async Task InsertHomeTimlineInMongo(string followerId, Tweet tweet, string currentUseId)
    {
        var homeTimeline = new HomeTimeline
        {
            UserId = followerId,
            TweetOwnerId = currentUseId,
            TweetId = tweet.Id,
            CreatedAt = tweet.CreatedAt
        };

        await _homeTimelineRepository.InsertOneAsync(homeTimeline);
    }
}
