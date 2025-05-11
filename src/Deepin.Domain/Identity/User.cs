using Microsoft.AspNetCore.Identity;

namespace Deepin.Domain.Identity;

public class User : IdentityUser<Guid>
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}
