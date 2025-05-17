using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;

namespace Deepin.Infrastructure.Extensions;

public static class HttpClientExtensions
{
    private static readonly int DefaultTimeout = 30;
    private static readonly IEnumerable<TimeSpan> Delay = Backoff.LinearBackoff(TimeSpan.FromMilliseconds(200), 3);
    private static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = HttpPolicyExtensions
        .HandleTransientHttpError()
        .Or<TimeoutRejectedException>()
        .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
        .WaitAndRetryAsync(Delay);
    private static readonly AsyncTimeoutPolicy<HttpResponseMessage> TimeoutPolicy =
        Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(DefaultTimeout));

    public static IHttpClientBuilder AddDefaultPolicies(this IHttpClientBuilder builder)
    {
        builder
        .AddPolicyHandler(RetryPolicy)
        .AddPolicyHandler(TimeoutPolicy)
        .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();
        return builder;
    }
}
public class HttpClientAuthorizationDelegatingHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization == null)
        {
            var accessToken = _httpContextAccessor.HttpContext?.GetTokenAsync("access_token").Result ?? string.Empty;
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}