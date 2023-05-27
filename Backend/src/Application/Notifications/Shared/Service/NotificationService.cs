namespace Application.Notifications.Shared.Service;

public class NotificationService : INotificationService
{
    private readonly ICurrentUserService _currentUserService;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUserRepository _userRepository;

    public NotificationService(ICurrentUserService currentUserService,
        INotificationRepository notificationRepository,
        IUserRepository userRepository)
    {
        _currentUserService = currentUserService;
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
    }

    public async Task CreateNotificationAsync(Tweet tweet, string actionType)
    {
        if (tweet.UserId != _currentUserService.UserId)
        {
            var notification = new Notification
            {
                TweetId = tweet.Id,
                UserId = tweet.UserId!,
                ActionedUserId = _currentUserService.UserId,
                Action = actionType,
                TweetType = TweetConstants.Tweet,
                Time = DateTime.Now
            };

            await _notificationRepository.InsertOneAsync(notification);
        }
    }

    public async Task<IList<NotificationViewModel>> GetNotificationsAsync(int pageNumber)
    {
        var currentUserId = _currentUserService.UserId;

        var userNotifications = await _notificationRepository.GetNotifications(x =>
        x.UserId == currentUserId, pageNumber);

        var userCollection = _userRepository.GetCollection();

        var notifications = from notification in userNotifications
                            join user in userCollection on notification.ActionedUserId equals user.Id
                            select CreateNotificationViewModel(notification, user, currentUserId);

        return notifications.ToList();
    }

    private NotificationViewModel CreateNotificationViewModel(Notification notification, User user, string userId)
    {
        return new NotificationViewModel
        {
            UserId = userId,
            TweetId = notification.TweetId,
            Action = notification.Action,
            ActionedUserId = user.Id,
            ActionedUserName = user.Name,
            ActionedUserImage = user.Image,
            TweetType = notification.TweetType,
            Time = notification.Time
        };
    }
}
