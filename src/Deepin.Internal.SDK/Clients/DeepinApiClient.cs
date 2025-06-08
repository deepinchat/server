using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Deepin.Internal.SDK.Configuration;

namespace Deepin.Internal.SDK.Clients;

/// <summary>
/// Main client interface for the Deepin API
/// </summary>
public interface IDeepinApiClient
{
    /// <summary>
    /// Client for chat-related operations
    /// </summary>
    IChatsClient Chats { get; }

    /// <summary>
    /// Client for message-related operations
    /// </summary>
    IMessagesClient Messages { get; }

    /// <summary>
    /// Client for file-related operations
    /// </summary>
    IFilesClient Files { get; }

    /// <summary>
    /// Client for user-related operations
    /// </summary>
    IUsersClient Users { get; }

    /// <summary>
    /// Updates the authentication token for all clients
    /// </summary>
    void SetAccessToken(string accessToken);

    /// <summary>
    /// Clears the authentication token for all clients
    /// </summary>
    void ClearAccessToken();
}

/// <summary>
/// Main client implementation for the Deepin API
/// </summary>
public class DeepinApiClient : IDeepinApiClient
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsMonitor<DeepinApiOptions> _optionsMonitor;

    public DeepinApiClient(
        HttpClient httpClient,
        IOptionsMonitor<DeepinApiOptions> optionsMonitor,
        ILogger<DeepinApiClient> logger,
        ILogger<ChatsClient> chatsLogger,
        ILogger<MessagesClient> messagesLogger,
        ILogger<FilesClient> filesLogger,
        ILogger<UsersClient> usersLogger)
    {
        _httpClient = httpClient;
        _optionsMonitor = optionsMonitor;

        // Create a wrapper that implements IOptions<T>
        var optionsWrapper = new OptionsWrapper<DeepinApiOptions>(optionsMonitor.CurrentValue);

        // Create individual clients with their specific loggers
        Chats = new ChatsClient(httpClient, optionsWrapper, chatsLogger);
        Messages = new MessagesClient(httpClient, optionsWrapper, messagesLogger);
        Files = new FilesClient(httpClient, optionsWrapper, filesLogger);
        Users = new UsersClient(httpClient, optionsWrapper, usersLogger);
    }

    /// <summary>
    /// Client for chat-related operations
    /// </summary>
    public IChatsClient Chats { get; }

    /// <summary>
    /// Client for message-related operations
    /// </summary>
    public IMessagesClient Messages { get; }

    /// <summary>
    /// Client for file-related operations
    /// </summary>
    public IFilesClient Files { get; }

    /// <summary>
    /// Client for user-related operations
    /// </summary>
    public IUsersClient Users { get; }

    /// <summary>
    /// Updates the authentication token for all clients
    /// </summary>
    public void SetAccessToken(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
        }

        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
    }

    /// <summary>
    /// Clears the authentication token for all clients
    /// </summary>
    public void ClearAccessToken()
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
