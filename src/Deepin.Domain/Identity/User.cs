using Microsoft.AspNetCore.Identity;

namespace Deepin.Domain.Identity;

public class User : IdentityUser<Guid>
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public User()
    {
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
    public User(string userName, string email) : this()
    {
        UserName = userName;
        Email = email;
    }
}
