namespace Deepin.Domain.ChatAggregate;

public interface IChatRepository : IRepository<Chat>
{
    Task<ChatReadStatus> AddOrUpdateReadStatusAsync(Guid id, Guid userId, Guid messageId, CancellationToken cancellationToken = default);
}
