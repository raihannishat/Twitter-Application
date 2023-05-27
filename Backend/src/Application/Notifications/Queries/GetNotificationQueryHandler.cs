namespace Application.Notifications.Queries;

public class GetNotificationQueryHandler : IRequestHandler<GetNotificationQuery, Result<List<NotificationViewModel>>>
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<GetNotificationQueryHandler> _logger;

    public GetNotificationQueryHandler(INotificationService notificationService,
        ILogger<GetNotificationQueryHandler> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<Result<List<NotificationViewModel>>> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await _notificationService.GetNotificationsAsync(request.PageNumber);

            if (response != null)
            {
                _logger.LogInformation(QueryMessages<GetNotificationQuery>.QueryExecutedSuccessfully);

                return Result<List<NotificationViewModel>>.Success(response.ToList());
            }
            _logger.LogError(QueryMessages<GetNotificationQuery>.FailedToExecuteQuery);

            return Result<List<NotificationViewModel>>.Failure(QueryMessages<GetNotificationQuery>.FailedToExecuteQuery);
        }
        catch (Exception ex)
        {
            _logger.LogError(QueryMessages<GetNotificationQuery>.FailedToExecuteQueryException, ex.Message);

            return Result<List<NotificationViewModel>>.Failure(QueryMessages<GetNotificationQuery>.FailedToExecuteQueryException);
        }
    }
}
