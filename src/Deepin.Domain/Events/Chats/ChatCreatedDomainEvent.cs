using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Domain.Events;

public record ChatCreatedDomainEvent(Chat Chat) : INotification;