namespace Deepin.Application.Interfaces;
public interface IUserContext
{
    Guid UserId { get; }
    string UserAgent { get; }
    string IpAddress { get; }
    string AccessToken { get; }
}

