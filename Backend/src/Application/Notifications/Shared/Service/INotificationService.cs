namespace Application.Notifications.Shared.Service;

public interface INotificationService
{
    Task<IList<NotificationViewModel>> GetNotificationsAsync(int pageNumber);
    Task CreateNotificationAsync(Tweet tweet, string actionType);
}
