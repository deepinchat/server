using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Domain.Events;

public record ChatMemberRemovedDomainEvent(Chat Chat, ChatMember ChatMember) : INotification;