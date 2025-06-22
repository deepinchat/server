using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Domain.Events;

public record ChatMemberAddedDomainEvent(ChatBase Chat, ChatMember ChatMember) : INotification;