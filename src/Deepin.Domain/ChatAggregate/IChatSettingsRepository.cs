namespace Deepin.Domain.ChatAggregate;

public interface IChatSettingsRepository : IRepository<ChatSettings>
{
    Task<ChatSettings?> GetByChatIdAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
}