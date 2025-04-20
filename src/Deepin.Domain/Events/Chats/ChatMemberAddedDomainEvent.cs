using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Domain.Events;

public record ChatMemberAddedDomainEvent(Chat Chat, ChatMember ChatMember) : INotification;