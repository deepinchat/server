namespace Deepin.Chatting.Application.Constants;

public static class CacheKeys
{
    public static string GetChatByIdCacheKey(Guid chatId) => $"chat_{chatId}";
}
