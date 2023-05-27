namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    public string UserId { get;}
    public string UserEmail { get; }
    public bool ClearSession();
}
