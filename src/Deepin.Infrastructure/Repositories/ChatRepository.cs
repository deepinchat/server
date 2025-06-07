using Deepin.Domain;
using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Infrastructure.Repositories;
public class ChatRepository(ChatDbContext db) : IChatRepository
{
    private readonly ChatDbContext _db = db;
    public IUnitOfWork UnitOfWork => _db;
    public async Task AddAsync(Chat chat, CancellationToken cancellationToken = default)
    {
        await _db.Chats.AddAsync(chat);
    }

    public async Task<ChatReadStatus> AddOrUpdateReadStatusAsync(Guid id, Guid userId, Guid messageId, CancellationToken cancellationToken)
    {
        var readStatus = await _db.ChatReadStatuses
            .FirstOrDefaultAsync(x => x.ChatId == id && x.UserId == userId, cancellationToken);
        if (readStatus is null)
        {
            readStatus = new ChatReadStatus(
                chatId: id,
                userId: userId,
                messageId: messageId
            );
            await _db.ChatReadStatuses.AddAsync(readStatus, cancellationToken);
        }
        else
        {
            readStatus.ReadMessage(messageId);
            _db.ChatReadStatuses.Update(readStatus);
        }
        return readStatus;
    }

    public Task DeleteAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        _db.Chats.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<Chat?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var chat = await _db.Chats.FindAsync([id], cancellationToken: cancellationToken);
        if (chat is null)
        {
            return null;
        }
        await _db.Entry(chat).Reference(x => x.GroupInfo).LoadAsync();
        return chat;
    }

    public Task UpdateAsync(Chat entity, CancellationToken cancellationToken = default)
    {
        _db.Chats.Update(entity);
        return Task.CompletedTask;
    }
}

