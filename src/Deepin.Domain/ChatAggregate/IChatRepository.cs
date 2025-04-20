namespace Deepin.Domain.ChatAggregate;

public interface IChatRepository : IRepository<Chat>
{
    Task<Chat?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Chat Add(Chat chat);
    Chat Update(Chat chat);
}
