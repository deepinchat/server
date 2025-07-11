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
    private readonly HttpClient _httpClient;
    private readonly DeepinApiOptions _options;
    private readonly ILogger<DeepinApiClient> _logger;
    private Lazy<ChatsClient>? _chatsClient;
    private Lazy<MessagesClient>? _messagesClient;
    private Lazy<FilesClient>? _filesClient;
    private Lazy<UsersClient>? _usersClient;
    private Lazy<ContactApiClient>? _contactApiClient;
    public DeepinApiClient(
        HttpClient httpClient,
        IOptionsMonitor<DeepinApiOptions> optionsMonitor,
        ILogger<DeepinApiClient> logger)
    {
        _httpClient = httpClient;
        _options = optionsMonitor.CurrentValue;
        _logger = logger;
    }

    /// <summary>
    /// Client for chat-related operations
    /// </summary>
    public IChatsClient Chats
    {
        get
        {
            if (_chatsClient == null)
            {
                _chatsClient = new Lazy<ChatsClient>(() => new ChatsClient(_httpClient, Options.Create(_options), _logger));
            }
            return _chatsClient.Value;
        }
    }

    /// <summary>
    /// Client for message-related operations
    /// </summary>
    public IMessagesClient Messages
    {
        get
        {
            if (_messagesClient == null)
            {
                _messagesClient = new Lazy<MessagesClient>(() => new MessagesClient(_httpClient, Options.Create(_options), _logger));
            }
            return _messagesClient.Value;
        }
    }

    /// <summary>
    /// Client for file-related operations
    /// </summary>
    public IFilesClient Files
    {
        get
        {
            if (_filesClient == null)
            {
                _filesClient = new Lazy<FilesClient>(() => new FilesClient(_httpClient, Options.Create(_options), _logger));
            }
            return _filesClient.Value;
        }
    }

    /// <summary>
    /// Client for user-related operations
    /// </summary>
    public IUsersClient Users
    {
        get
        {
            if (_usersClient == null)
            {
                _usersClient = new Lazy<UsersClient>(() => new UsersClient(_httpClient, Options.Create(_options), _logger));
            }
            return _usersClient.Value;
        }
    }

    /// <summary>
    /// Client for contact-related operations
    /// </summary>
    public IContactApiClient Contacts
    {
        get
        {
            if (_contactApiClient == null)
            {
                _contactApiClient = new Lazy<ContactApiClient>(() => new ContactApiClient(_httpClient, Options.Create(_options), _logger));
            }
            return _contactApiClient.Value;
        }
    }
}
