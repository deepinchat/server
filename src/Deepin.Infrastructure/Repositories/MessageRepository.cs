using Deepin.Domain;
using Deepin.Domain.MessageAggregate;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;

public class MessageRepository(MessageDbContext db) : IMessageRepository
{
    public IUnitOfWork UnitOfWork => throw new NotImplementedException();

    public async Task AddAsync(Message entity, CancellationToken cancellationToken = default)
    {
        await db.Messages.AddAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(Message entity, CancellationToken cancellationToken = default)
    {
        db.Messages.Remove(entity);
        await Task.CompletedTask;
    }

    public async Task<Message?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await db.Messages.FindAsync(new object[] { id }, cancellationToken);
        if (message is null)
        {
            return null;
        }
        await db.Entry(message).Collection(m => m.Attachments).LoadAsync();

        return message;
    }

    public async Task UpdateAsync(Message entity, CancellationToken cancellationToken = default)
    {
        db.Messages.Update(entity);
        await Task.CompletedTask;
    }
}
