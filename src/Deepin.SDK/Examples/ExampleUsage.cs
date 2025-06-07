using Deepin.SDK.Clients;
using Deepin.SDK.Configuration;
using Deepin.SDK.Extensions;
using Deepin.SDK.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Deepin.SDK.Examples;

/// <summary>
/// Example demonstrating how to use the Deepin SDK
/// </summary>
public class ExampleUsage
{
    public static async Task Main(string[] args)
    {
        // Setup dependency injection
        var services = new ServiceCollection();
        
        // Add logging
        services.AddLogging(builder => 
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        // Add Deepin API client
        services.AddDeepinApiClient(options =>
        {
            options.BaseUrl = "https://api.deepin.example.com";
            options.AccessToken = "your-access-token";
            options.Timeout = TimeSpan.FromSeconds(30);
        });

        // Build service provider
        var serviceProvider = services.BuildServiceProvider();

        // Get the API client
        var apiClient = serviceProvider.GetRequiredService<IDeepinApiClient>();

        try
        {
            // Example: Get all chats
            Console.WriteLine("Getting user chats...");
            var chats = await apiClient.Chats.GetChatsAsync();
            Console.WriteLine($"Found {chats.Count} chats");

            // Example: Send a message
            if (chats.Count > 0)
            {
                var firstChat = chats.First();
                Console.WriteLine($"Sending message to chat: {firstChat.GroupInfo?.Name}");
                
                var message = await apiClient.Messages.SendMessageAsync(new SendMessageRequest
                {
                    Content = "Hello from Deepin SDK!",
                    ChatId = firstChat.Id,
                    Type = MessageType.Text
                });

                if (message != null)
                {
                    Console.WriteLine($"Message sent successfully! ID: {message.Id}");
                }
            }

            // Example: Search users
            Console.WriteLine("Searching for users...");
            var users = await apiClient.Users.SearchUsersAsync(new SearchUsersRequest
            {
                Query = "john",
                Limit = 10,
                Offset = 0
            });
            Console.WriteLine($"Found {users.Count} users matching 'john'");

            // Example: Upload a file (commented out as it requires actual file)
            /*
            using var fileStream = File.OpenRead("path/to/your/file.txt");
            var uploadResult = await apiClient.Files.UploadFileAsync(
                fileStream, 
                "example.txt", 
                "text/plain"
            );
            Console.WriteLine($"File uploaded with ID: {uploadResult?.Id}");
            */
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"API Error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}

/// <summary>
/// Example of using individual clients
/// </summary>
public class IndividualClientExample
{
    private readonly IChatsClient _chatsClient;
    private readonly IMessagesClient _messagesClient;
    private readonly ILogger<IndividualClientExample> _logger;

    public IndividualClientExample(
        IChatsClient chatsClient, 
        IMessagesClient messagesClient,
        ILogger<IndividualClientExample> logger)
    {
        _chatsClient = chatsClient;
        _messagesClient = messagesClient;
        _logger = logger;
    }

    public async Task SendWelcomeMessage(Guid chatId, string userName)
    {
        try
        {
            var chat = await _chatsClient.GetChatAsync(chatId);
            if (chat == null)
            {
                _logger.LogWarning("Chat with ID {ChatId} not found", chatId);
                return;
            }

            var welcomeMessage = $"Welcome to {chat.GroupInfo?.Name}, {userName}!";
            
            await _messagesClient.SendMessageAsync(new SendMessageRequest
            {
                Content = welcomeMessage,
                ChatId = chatId,
                Type = MessageType.Text
            });

            _logger.LogInformation("Welcome message sent to {ChatName}", chat.GroupInfo?.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send welcome message to chat {ChatId}", chatId);
            throw;
        }
    }
}
