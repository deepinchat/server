using Deepin.Application.DTOs.Messages;
using Deepin.Application.Requests.Messages;
using MediatR;

namespace Deepin.Application.Commands.Messages;

public record SendMessageCommand(SendMessageRequest Request) : IRequest<MessageDto>;