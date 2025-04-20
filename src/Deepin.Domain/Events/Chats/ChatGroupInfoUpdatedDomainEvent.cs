using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Domain.Events;

public record ChatGroupInfoUpdatedDomainEvent(Chat Chat) : INotification;