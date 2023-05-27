namespace Application.Notifications.Shared.Interfaces;

public interface INotificationConsumer
{
    Task ReceiveNotification();
}
