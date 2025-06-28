using Deepin.Domain.ChatAggregate;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;

public class ChatMessageRepository(ChatDbContext db) : RepositoryBase<ChatMessage, ChatDbContext>(db), IChatMessageRepository
{
}