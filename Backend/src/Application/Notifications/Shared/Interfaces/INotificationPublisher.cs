namespace Application.Notifications.Shared.Interfaces;

public interface INotificationPublisher
{
    Task SendNotification(string tweetCreatorId);
}
