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
}

/// <summary>
/// Main client implementation for the Deepin API
/// </summary>
public class DeepinApiClient : IDeepinApiClient
{
    public DeepinApiClient(
        HttpClient httpClient,
        IOptionsMonitor<DeepinApiOptions> optionsMonitor,
        ILogger<DeepinApiClient> logger,
        ILogger<ChatsClient> chatsLogger,
        ILogger<MessagesClient> messagesLogger,
        ILogger<FilesClient> filesLogger,
        ILogger<UsersClient> usersLogger)
    {

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
}
