using AutoMapper;
using Deepin.Application.Commands.Chats;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.IntegrationEvents;
using Deepin.Application.Interfaces;
using Deepin.Application.Requests.Messages;
using Deepin.Domain.MessageAggregate;
using MediatR;
using Newtonsoft.Json;

namespace Deepin.Application.Commands.Messages;

public record SendMessageCommand(MessageRequest Request, Guid? SenderId = null) : IRequest<MessageDto>;

public class SendMessageCommandHandler(
    IMapper mapper,
    IMediator mediator,
    IEventBus eventBus,
    IMessageRepository messageRepository) : IRequestHandler<SendMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var message = new Message(
            type: request.Type,
            chatId: request.ChatId,
            userId: command.SenderId,
            content: request.Content,
            parentId: request.ParentId,
            replyToId: request.ReplyToId,
            mentions: request.Mentions is null ? null : JsonConvert.SerializeObject(request.Mentions),
            metadata: request.Metadata?.ToString());
        if (request.Attachments != null)
        {
            foreach (var attachment in request.Attachments)
            {
                message.AddAttachment(new MessageAttachment(
                    type: attachment.Type,
                    fileId: attachment.FileId,
                    fileName: attachment.FileName,
                    fileSize: attachment.FileSize,
                    contentType: attachment.ContentType,
                    order: attachment.Order,
                    thumbnailFileId: attachment.ThumbnailFileId,
                    metadata: attachment.Metadata?.ToString()));
            }
        }
        await messageRepository.AddAsync(message, cancellationToken);
        await messageRepository.UnitOfWork.ExecuteInTransactionAsync(async () =>
        {
            await messageRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
            await mediator.Send(
                new AddChatMessageCommand(
                    ChatId: message.ChatId,
                    MessageId: message.Id,
                    SenderId: command.SenderId,
                    MessageType: message.Type.ToString(),
                    SentAt: message.CreatedAt), cancellationToken);
            await eventBus.PublishAsync(new MessageSentIntegrationEvent(message.ChatId, message.Id), cancellationToken);
        }, cancellationToken);
        return mapper.Map<MessageDto>(message);
    }
}
