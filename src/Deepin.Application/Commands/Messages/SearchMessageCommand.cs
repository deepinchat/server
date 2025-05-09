using Deepin.Application.DTOs;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.Requests.Messages;
using MediatR;

namespace Deepin.Application.Commands.Messages;

public record SearchMessageCommand(SearchMessageRequest Request) : IRequest<IPagedResult<MessageDto>>;