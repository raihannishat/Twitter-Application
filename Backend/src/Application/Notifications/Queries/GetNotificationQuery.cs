using Application.Notifications.Shared.Models;

namespace Application.Notifications.Queries;

public class GetNotificationQuery : IRequest<Result<List<NotificationViewModel>>>
{
  public int PageNumber { get; set; }

    public GetNotificationQuery(int pageNumber)
    {
        PageNumber = pageNumber;
    }
}
