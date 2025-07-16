using System.Security.Claims;
using Deepin.Domain.Identity;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Deepin.IdentityServer.Extensions;

public static class UserManagerExtensions
{
    public static async Task<User> AutoProvisionUserAsync(
        this UserManager<User> userManager,
        string provider,
        string providerUserId,
        List<Claim> claims)
    {
        if (userManager is null)
        {
            throw new ArgumentNullException(nameof(userManager));
        }

        if (string.IsNullOrWhiteSpace(provider))
        {
            throw new ArgumentException("Provider must not be null or empty.", nameof(provider));
        }

        if (string.IsNullOrWhiteSpace(providerUserId))
        {
            throw new ArgumentException("Provider user ID must not be null or empty.", nameof(providerUserId));
        }

        var user = new User
        {
            UserName = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.PreferredUserName)?.Value ?? providerUserId,
            Email = claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Email)?.Value,
            EmailConfirmed = true
        };
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        // Add external login
        var loginInfo = new UserLoginInfo(provider, providerUserId, provider);
        result = await userManager.AddLoginAsync(user, loginInfo);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to add external login: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        // Add claims
        result = await userManager.AddClaimsAsync(user, claims);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to add claims: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
        return user;
    }
}
