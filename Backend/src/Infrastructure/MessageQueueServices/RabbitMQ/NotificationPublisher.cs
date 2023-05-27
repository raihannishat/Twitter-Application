using Application.Notifications.Shared.Interfaces;
using RabbitMQ.Client;
using System.Text.Json;

namespace Infrastructure.MessageQueueServices.RabbitMQ;

public class NotificationPublisher : INotificationPublisher
{
    private readonly ConnectionFactory _factory;
    private readonly ICurrentUserService _currentUserService;

    public NotificationPublisher(IRabbitMQSettings rabbitMQSettings, ICurrentUserService currentUserService)
    {
        _factory = new ConnectionFactory() { Uri = new Uri(rabbitMQSettings.URI) };
        _currentUserService = currentUserService;
    }
  
    public Task SendNotification(string tweetCreatorId)
    {
        string exchange = $"{_currentUserService.UserId}_notifier";
        
        string queueName = $"{tweetCreatorId}_notification";

        using (var connection = _factory.CreateConnection())

        using (IModel channel = connection.CreateModel())
        {
            channel.ExchangeDeclare(exchange, ExchangeType.Direct);

            channel.QueueDeclare(queueName, true, false, false, null);

            channel.QueueBind(queueName, exchange, "");

            var body = Encoding.UTF8.GetBytes("new notification");

            channel.BasicPublish(exchange: exchange, routingKey: "", basicProperties: null, body: body);

        }
        return Task.CompletedTask;
    }
}
