namespace Deepin.Chatting.Application.Constants;

public static class CacheKeys
{
    public static string GetChatByIdCacheKey(Guid id) => $"chat_{id}";
    public static string GetFileByIdCacheKey(Guid id) => $"file_{id}";
}
