using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Domain.Events;

public record ChatDeletedDomainEvent(ChatBase Chat) : INotification;