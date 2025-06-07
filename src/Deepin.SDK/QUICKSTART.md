# Deepin.SDK Quick Start Guide

## Installation

### Via NuGet Package Manager
```bash
dotnet add package Deepin.SDK
```

### Via Package Manager Console
```powershell
Install-Package Deepin.SDK
```

## Basic Setup

### 1. Configure in Program.cs (.NET 6+)

```csharp
using Deepin.SDK.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add Deepin API client
builder.Services.AddDeepinApiClient(options =>
{
    options.BaseUrl = "https://your-deepin-api.com";
    options.AccessToken = "your-access-token";
});

var app = builder.Build();
```

### 2. Configure from appsettings.json

**appsettings.json:**
```json
{
  "DeepinApi": {
    "BaseUrl": "https://your-deepin-api.com",
    "AccessToken": "your-access-token",
    "Timeout": "00:00:30"
  }
}
```

**Program.cs:**
```csharp
builder.Services.AddDeepinApiClient(builder.Configuration);
```

## Usage Examples

### Using the Main Client

```csharp
public class ChatService
{
    private readonly IDeepinApiClient _apiClient;

    public ChatService(IDeepinApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<List<Chat>> GetUserChatsAsync()
    {
        return await _apiClient.Chats.GetChatsAsync();
    }

    public async Task<Message?> SendMessageAsync(int chatId, string content)
    {
        return await _apiClient.Messages.SendMessageAsync(new SendMessageRequest
        {
            Content = content,
            ChatId = chatId,
            Type = MessageType.Text
        });
    }
}
```

### Using Individual Clients

```csharp
public class UserService
{
    private readonly IUsersClient _usersClient;

    public UserService(IUsersClient usersClient)
    {
        _usersClient = usersClient;
    }

    public async Task<User?> GetUserAsync(int userId)
    {
        return await _usersClient.GetUserAsync(userId);
    }

    public async Task<List<User>> SearchUsersAsync(string query)
    {
        return await _usersClient.SearchUsersAsync(query);
    }
}
```

## API Coverage

### Chats
- ✅ Get chat by ID
- ✅ Get user's chats
- ✅ Search chats
- ✅ Create chat
- ✅ Update chat
- ✅ Create direct chat
- ✅ Delete chat
- ✅ Join/Leave chat
- ✅ Get chat members
- ✅ Read status management

### Messages
- ✅ Get message by ID
- ✅ Get messages by IDs
- ✅ Search messages
- ✅ Send message
- ✅ Get last messages

### Files
- ✅ Get file info
- ✅ Download file
- ✅ Upload file

### Users
- ✅ Get user by ID
- ✅ Get users by IDs
- ✅ Search users

## Authentication

### Set Access Token at Runtime
```csharp
apiClient.SetAccessToken("new-access-token");
```

### Clear Access Token
```csharp
apiClient.ClearAccessToken();
```

## Error Handling

```csharp
try
{
    var chat = await apiClient.Chats.GetChatAsync(chatId);
}
catch (HttpRequestException ex)
{
    // Handle API errors
    logger.LogError(ex, "Failed to get chat {ChatId}", chatId);
}
```

## File Operations

### Upload File
```csharp
using var fileStream = File.OpenRead("path/to/file.pdf");
var result = await apiClient.Files.UploadFileAsync(
    fileStream, 
    "document.pdf", 
    "application/pdf"
);
```

### Download File
```csharp
using var downloadStream = await apiClient.Files.DownloadFileAsync(fileId);
if (downloadStream != null)
{
    using var outputFile = File.Create("downloaded-file.pdf");
    await downloadStream.CopyToAsync(outputFile);
}
```

## Advanced Configuration

```csharp
builder.Services.AddDeepinApiClient(options =>
{
    options.BaseUrl = "https://api.deepin.com";
    options.AccessToken = "token";
    options.Timeout = TimeSpan.FromSeconds(60);
    options.DefaultHeaders.Add("X-Custom-Header", "value");
});
```

## Support

For more information, see the full [README.md](./README.md) file.
