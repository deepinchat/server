namespace Deepin.Domain.ChatAggregate;

public interface IChatReadStatusRepository : IRepository<ChatReadStatus>
{

    Task<ChatReadStatus?> GetByChatIdAsync(Guid chatId, Guid userId, CancellationToken cancellationToken = default);
}