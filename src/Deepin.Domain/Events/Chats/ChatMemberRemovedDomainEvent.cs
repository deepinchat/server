using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Domain.Events;

public record ChatMemberRemovedDomainEvent(ChatBase Chat, ChatMember ChatMember) : INotification;