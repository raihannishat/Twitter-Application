using Application.Notifications.Queries;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;

[ApiController, Route("api")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationController : BaseApiController
{

    [HttpGet("notifications")]
    public async Task<IActionResult> GetAllNotification([FromQuery(Name = "page")] int pageNumber)
    {
        return HandleResult(await Mediator.Send(new GetNotificationQuery(pageNumber)));
    }
}