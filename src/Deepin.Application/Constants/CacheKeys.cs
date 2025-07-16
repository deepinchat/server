namespace Deepin.Chatting.Application.Constants;

public static class CacheKeys
{
    public static string GetChatByIdCacheKey(Guid id) => $"chat_{id}";
    public static string GetGroupChatsCacheKey(Guid userId) => $"group_chats_{userId}";
    public static string GetDirectChatsCacheKey(Guid userId) => $"direct_chats_{userId}";
    public static string GetUserByIdCacheKey(Guid id) => $"user_{id}";
    public static string GetFileByIdCacheKey(Guid id) => $"file_{id}";
    public static string GetMessageByIdCacheKey(Guid id) => $"message_{id}";
}
