namespace Application.Notifications.Shared.Interfaces;

public interface INotificationRepository : IRepository<Notification>
{
    Task<IEnumerable<Notification>> GetNotifications(Expression<Func<Notification, bool>> filterExpression, int pageNumber);
}
