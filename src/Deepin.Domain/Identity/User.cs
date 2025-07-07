using Deepin.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace Deepin.Domain.Identity;

public class User : IdentityUser<Guid>, IAggregateRoot
{
    private List<IdentityUserClaim<Guid>> _claims = [];
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public IReadOnlyCollection<IdentityUserClaim<Guid>> Claims => _claims;
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

    public void AddClaims(string claimType, string claimValue)
    {
        if (string.IsNullOrWhiteSpace(claimType))
        {
            throw new DomainException("Claim type cannot be null or empty.");
        }
        if (string.IsNullOrWhiteSpace(claimValue))
        {
            throw new DomainException("Claim value cannot be null or empty.");
        }
        if (_claims.Any(c => c.ClaimType == claimType))
        {
            return; // Claim already exists, no need to add it again
        }

        var claim = new IdentityUserClaim<Guid>
        {
            ClaimType = claimType,
            ClaimValue = claimValue
        };
        _claims.Add(claim);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ClearClaims()
    {
        _claims.Clear();
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
