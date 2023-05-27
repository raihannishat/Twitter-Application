using Application.Comments.Commands.CreateComment;
using Application.Likes.Commands;
using Application.Notifications.Shared.Interfaces;
using Application.Retweets.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.SignalR.Notification;

public class NotificationHub : Hub
{
    private readonly IMediator _mediator;
    private readonly ICurrentUserService _currentUserService;
   

    public NotificationHub(IMediator mediator, ICurrentUserService currentUserService)
    {
        _mediator = mediator;
        _currentUserService = currentUserService;
    }

    public async Task SendComment(CreateCommentCommand command)
    {
        
        if (_currentUserService.UserId != command.TweetCreatorId)
        {
            await Clients.User(command.TweetCreatorId)
                .SendAsync("ReceiveNotification", "new notification");
        }
    }

    public async Task SendLike(LikeCommand command)
    {
       
        if (_currentUserService.UserId != command.TweetCreatorId)
        {
            await Clients.User(command.TweetCreatorId)
                .SendAsync("ReceiveNotification", "new notification");
        }
    }

    public async Task SendRetweet(RetweetCommand command)
    {
        if (_currentUserService.UserId != command.TweetCreatorId)
        {
            await Clients.User(command.TweetCreatorId)
                .SendAsync("ReceiveNotification", "new notification");
        }
    }

    public override async Task OnConnectedAsync()
    {
        var groupName = _currentUserService.UserId;

        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
