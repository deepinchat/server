using Deepin.API.Models.Users;
using Deepin.Application.DTOs.Users;
using Duende.IdentityModel;

namespace Deepin.API.Extensions;

public static class ModelExtensions
{
    public static UserProfile ToUserProfile(this UserDto userDto)
    {
        if (userDto == null) throw new ArgumentNullException(nameof(userDto));

        var profile = new UserProfile
        {
            Id = userDto.Id,
            UserName = userDto.UserName,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            CreatedAt = userDto.CreatedAt.DateTime,
            UpdatedAt = userDto.UpdatedAt.DateTime
        };
        if (userDto.Claims != null)
        {
            foreach (var claim in userDto.Claims)
            {
                switch (claim.ClaimType)
                {
                    case JwtClaimTypes.GivenName:
                        profile.GivenName = claim.ClaimValue;
                        break;
                    case JwtClaimTypes.FamilyName:
                        profile.FamilyName = claim.ClaimValue;
                        break;
                    case JwtClaimTypes.Name:
                        profile.DisplayName = claim.ClaimValue;
                        break;
                    case JwtClaimTypes.Picture:
                        profile.PictureId = claim.ClaimValue;
                        break;
                    case JwtClaimTypes.BirthDate:
                        profile.BirthDate = claim.ClaimValue;
                        break;
                    case JwtClaimTypes.ZoneInfo:
                        profile.ZoneInfo = claim.ClaimValue;
                        break;
                    case JwtClaimTypes.Locale:
                        profile.Locale = claim.ClaimValue;
                        break;
                    case "bio":
                        profile.Bio = claim.ClaimValue;
                        break;
                }
            }
        }
        return profile;
    }

}
