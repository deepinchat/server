using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using Deepin.IdentityServer.Constants;
using Duende.IdentityModel;
using Duende.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace Deepin.IdentityServer.Extensions;

public static class GitHubAuthenticationExtensions
{
    private const string DefaultCallbackPath = "/signin-github";
    private const string DefaultClaimsIssuer = "OAuth2-Github";
    public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string clientId, string clientSecret)
    {
        return builder.AddGitHub(clientId, clientSecret, _ => { });
    }

    public static AuthenticationBuilder AddGitHub(this AuthenticationBuilder builder, string clientId, string clientSecret, Action<OAuthOptions> configureOptions)
    {

        ArgumentException.ThrowIfNullOrWhiteSpace(clientId, nameof(clientId));
        ArgumentException.ThrowIfNullOrWhiteSpace(clientSecret, nameof(clientSecret));

        // You must first create an app with GitHub and add its ID and Secret to your user-secrets.
        // https://github.com/settings/applications/
        // https://docs.github.com/en/developers/apps/authorizing-oauth-apps
        return builder.AddOAuth("GitHub", "GitHub", o =>
        {
            o.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            o.ClientId = clientId;
            o.ClientSecret = clientSecret;
            o.CallbackPath = new PathString(DefaultCallbackPath);
            o.AuthorizationEndpoint = "https://github.com/login/oauth/authorize";
            o.TokenEndpoint = "https://github.com/login/oauth/access_token";
            o.UserInformationEndpoint = "https://api.github.com/user";
            o.ClaimsIssuer = DefaultClaimsIssuer;
            o.SaveTokens = true;

            o.Scope.Add("user:email");
            o.Scope.Add("read:user");

            o.ClaimActions.MapJsonKey(JwtClaimTypes.Subject, "id");
            o.ClaimActions.MapJsonKey(JwtClaimTypes.PreferredUserName, "login");
            o.ClaimActions.MapJsonKey(JwtClaimTypes.Name, "name");
            o.ClaimActions.MapJsonKey(JwtClaimTypes.Email, "email");
            o.ClaimActions.MapJsonKey(JwtClaimTypes.Picture, "avatar_url");
            o.ClaimActions.MapJsonKey(JwtClaimTypes.WebSite, "blog");
            o.ClaimActions.MapJsonKey(DeepinClaimTypes.Bio, "bio");
            o.ClaimActions.MapJsonKey(DeepinClaimTypes.Company, "company");
            o.ClaimActions.MapJsonKey(DeepinClaimTypes.Location, "location");
            o.ClaimActions.MapJsonKey(DeepinClaimTypes.GithubUrl, "html_url");

            o.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    try
                    {

                        await GetGitHubUserInfoAsync(context);

                        await GetGitHubUserEmailsAsync(context);
                    }
                    catch (Exception ex)
                    {
                        context.Fail($"Failed to retrieve user information from GitHub: {ex.Message}");
                    }
                },
                OnRemoteFailure = context =>
                {
                    context.Response.Redirect("/Account/Login?error=" + Uri.EscapeDataString(context.Failure?.Message ?? "Unknown error"));
                    context.HandleResponse();
                    return Task.CompletedTask;
                }
            };

            configureOptions?.Invoke(o);
        });
    }

    private static async Task GetGitHubUserInfoAsync(OAuthCreatingTicketContext context)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("Deepin", "1.0"));

        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        using var user = JsonDocument.Parse(content);
        context.RunClaimActions(user.RootElement);
    }

    private static async Task GetGitHubUserEmailsAsync(OAuthCreatingTicketContext context)
    {
        var existingEmail = context.Principal?.FindFirst(JwtClaimTypes.Email)?.Value;
        if (!string.IsNullOrEmpty(existingEmail))
        {
            return;
        }

        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user/emails");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.UserAgent.Add(new ProductInfoHeaderValue("DeepinChat", "1.0"));

            var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                using var emails = JsonDocument.Parse(content);

                foreach (var email in emails.RootElement.EnumerateArray())
                {
                    var emailAddress = email.GetProperty("email").GetString();
                    var isPrimary = email.TryGetProperty("primary", out var primaryProp) && primaryProp.GetBoolean();
                    var isVerified = email.TryGetProperty("verified", out var verifiedProp) && verifiedProp.GetBoolean();

                    if (isPrimary && isVerified && !string.IsNullOrEmpty(emailAddress))
                    {
                        var identity = (ClaimsIdentity)context.Principal!.Identity!;
                        identity.AddClaim(new Claim(JwtClaimTypes.Email, emailAddress, ClaimValueTypes.Email, DefaultClaimsIssuer));
                        break;
                    }
                }
            }
        }
        catch
        {

        }
    }
}
