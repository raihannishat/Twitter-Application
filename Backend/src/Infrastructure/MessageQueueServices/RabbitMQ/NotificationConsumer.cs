using Application.Notifications.Shared.Interfaces;
using Infrastructure.SignalR.Notification;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace Infrastructure.MessageQueueServices.RabbitMQ;

public class NotificationConsumer : INotificationConsumer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IMapper _mapper;
    private readonly IFollowRepository _followRepository;
    private readonly ILogger<NotificationConsumer> _logger;
    private readonly ConnectionFactory _factory;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHomeTimelineRepository _homeTimelineRepository;

    public NotificationConsumer(
        IServiceProvider serviceProvider,
        IMapper mapper,
        ILogger<NotificationConsumer> logger, 
        IRabbitMQSettings rabbitMQSettings,
        ICurrentUserService currentUserService,
        IFollowRepository followRepository, 
        IHomeTimelineRepository homeTimelineRepository
        )
    {
        _serviceProvider = serviceProvider;
        _mapper = mapper;
        _logger = logger;
        _factory = new ConnectionFactory() { Uri = new Uri(rabbitMQSettings.URI) };
        _connection = _factory.CreateConnection();
        _channel = _connection.CreateModel();
        _currentUserService = currentUserService;
        _followRepository = followRepository;
        _homeTimelineRepository = homeTimelineRepository;
    }

    public Task ReceiveNotification()
    {
        var queueName = $"{_currentUserService.UserId}_notification";       
        
        _channel.QueueDeclare(queueName, true, false, false, null);
        
        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            var chatHub = (IHubContext<NotificationHub>)_serviceProvider.GetService(typeof(IHubContext<NotificationHub>))!;
            
            var body = ea.Body.ToArray();
            
            var data = Encoding.UTF8.GetString(body);

            var groupName = $"{_currentUserService.UserId}";

            await chatHub.Clients.Group(groupName).SendAsync("ReceiveNotification", data);

        };

        _channel.BasicConsume(queueName, true, consumer);
        
        return Task.CompletedTask;
    }

}
