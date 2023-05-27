using Application.Photos.Commands.CoverPhoto;
using Application.Photos.Commands.ProfilePhoto;
using Microsoft.AspNetCore.Mvc;


namespace WebApi.Controllers;
[ApiController, Route("api/photos")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class PhotoController : BaseApiController
{

    [HttpPost("profile")]
    public async Task<IActionResult> Add([FromForm]ProfilePhotoUploadCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

    [HttpPost("cover")]
    public async Task<IActionResult> AddCoverPhoto([FromForm] CoverPhotoUploadCommand command)
    {
        return HandleResult(await Mediator.Send(command));
    }

}