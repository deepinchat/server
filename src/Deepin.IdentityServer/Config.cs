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
                ClientId = "deepinswaggerui",
                ClientName = "Deepin Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = {
                    "http://localhost:5000/swagger/oauth2-redirect.html",
                    "https://localhost:5000/swagger/oauth2-redirect.html",
                    "https://gateway.deepin.me/swagger/oauth2-redirect.html"
                    },
                AllowedScopes = [
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    .. ApiScopes.Select(s => s.Name)
                ],
                RequireConsent = false,
            },
            new Client
            {
                ClientId = "deepin.web.client",
                ClientName = "Deepin Web Client",
                ClientSecrets = {new Secret("deepin.web.client.secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                AllowOfflineAccess = true,
                RequirePkce = false,
                RequireClientSecret = true,
                RequireConsent = false,
                RedirectUris = {"https://localhost:4443/signin-oidc","http://localhost:4444/signin-oidc","http://localhost:4200/signin-oidc","https://deepin.chat/signin-oidc","https://deepin.me/signin-oidc"},
                PostLogoutRedirectUris = {"http://localhost:4200/signout-callback-oidc","https://deepin.chat/signout-callback-oidc","https://deepin.me/signout-callback-oidc"},
                AllowedCorsOrigins = {"https://localhost:4443","http://localhost:4444","http://localhost:4200" , "https://deepin.chat","https://deepin.me"},
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
                ]
            },
        };
}
