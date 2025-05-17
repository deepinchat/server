using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Deepin.IdentityServer;


internal static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            };
    public static IEnumerable<ApiResource> ApiResources => [
        new ApiResource("monolith.api"){
            Scopes = [
                "download",
                "upload",
                "chat",
                "message",
                "presence",
                "user"
            ]
        }];
    public static IEnumerable<ApiScope> ApiScopes => ApiResources.SelectMany(r => r.Scopes).Select(s => new ApiScope(s));
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman Client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    .. ApiScopes.Select(s => s.Name)
                ],
            },
            new Client
            {
                ClientId = "deepinweb",
                ClientName = "Deepin Web Client",
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = true,
                RequirePkce = true,
                RequireClientSecret = false,
                RequireConsent = false,
                RedirectUris = {"http://localhost:4200/callback/signin","https://deepin.chat/callback/signin","https://deepin.me/callback/signin"},
                PostLogoutRedirectUris = {"http://localhost:4200/callback/signout","https://deepin.chat/callback/signout","https://deepin.me/callback/signout"},
                AllowedCorsOrigins = {"http://localhost:4200" , "https://deepin.chat","https://deepin.me"},
                AllowedScopes =
                [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "upload",
                    "download",
                    "chat",
                    "message",
                    "presence",
                    "user"
                ],
                AccessTokenLifetime = 3600,
                IdentityTokenLifetime = 1800,
            },
            new Client
            {
                ClientId = "deepinswaggerui",
                ClientName = "Deepin Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = {"http://localhost:5000/swagger/oauth2-redirect.html","https://localhost:5000/swagger/oauth2-redirect.html"},
                AllowedScopes = [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    .. ApiScopes.Select(s => s.Name)
                ],
                RequireConsent = false,
            }
        };
}
