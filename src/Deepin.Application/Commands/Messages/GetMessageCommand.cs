using Deepin.Application.DTOs.Messages;
using MediatR;

namespace Deepin.Application.Commands.Messages;

public record GetMessageCommand(Guid Id) : IRequest<MessageDto?>;