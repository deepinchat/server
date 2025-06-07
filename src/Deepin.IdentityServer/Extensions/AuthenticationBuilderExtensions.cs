using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Deepin.IdentityServer.Extensions;

public static class AuthenticationBuilderExtensions
{

    public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string clientId, string clientSecret)
    {
        // You must first create an app with GitHub and add its ID and Secret to your user-secrets.
        // https://github.com/settings/applications/
        // https://docs.github.com/en/developers/apps/authorizing-oauth-apps
        return builder.AddOAuth("GitHub", "GitHub", o =>
        {
            o.ClientId = clientId;
            o.ClientSecret = clientSecret;
            o.CallbackPath = new PathString("/signin-github");
            o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            o.TokenEndpoint = "https://github.com/login/oauth/access_token";
            o.UserInformationEndpoint = "https://api.github.com/user";
            o.ClaimsIssuer = "OAuth2-Github";
            o.SaveTokens = true;
            // Retrieving user information is unique to each provider.
            o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            o.ClaimActions.MapJsonKey(ClaimTypes.Name, "login");
            o.ClaimActions.MapJsonKey("urn:github:name", "name");
            o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email", ClaimValueTypes.Email);
            o.ClaimActions.MapJsonKey("urn:github:url", "url");
            o.ClaimActions.MapJsonKey(JwtClaimTypes.Picture, "avatar_url");
            o.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    // Get the GitHub user
                    var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();

                    using (var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
                    {
                        context.RunClaimActions(user.RootElement);
                    }
                }
            };
        });
    }
}
