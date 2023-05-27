using Application.Common.Interfaces;
using System.Security.Claims;

namespace WebApi.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;
    public CurrentUserService(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }
    public string UserId => _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public string UserEmail => _contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Email);

    public bool ClearSession()
    {
        _contextAccessor.HttpContext!.Session.Remove(ClaimTypes.NameIdentifier);
        _contextAccessor.HttpContext.Session.Remove(ClaimTypes.Email);
        _contextAccessor.HttpContext!.Session.Clear();
        _contextAccessor.HttpContext.User = null!;

        return true;
    }
}
