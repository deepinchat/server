using AutoMapper;
using Deepin.Application.DTOs.Messages;
using Deepin.Application.IntegrationEvents;
using Deepin.Application.Interfaces;
using Deepin.Domain.MessageAggregate;
using MediatR;
using Newtonsoft.Json;

namespace Deepin.Application.Commands.Messages;

public class SendMessageCommandHandler(
    IMapper mapper,
    IEventBus eventBus,
    IUserContext userContext,
    IMessageRepository messageRepository) : IRequestHandler<SendMessageCommand, MessageDto>
{
    public async Task<MessageDto> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        var message = new Message(
            type: request.Type,
            chatId: request.ChatId,
            userId: userContext.UserId,
            text: request.Content,
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
            await eventBus.PublishAsync(new PushMessageIntegrationEvent(message.ChatId, message.Id), cancellationToken);
        }, cancellationToken);
        return mapper.Map<MessageDto>(message);
    }
}
