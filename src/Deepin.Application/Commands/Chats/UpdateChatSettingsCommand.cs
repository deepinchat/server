using Deepin.Application.Interfaces;
using Deepin.Domain.ChatAggregate;
using MediatR;

namespace Deepin.Application.Commands.Chats;

public record UpdateChatSettingsCommand(
    Guid ChatId,
    ChatNotificationLevel NotificationLevel,
    bool IsMuted,
    bool IsPinned) : IRequest<Unit>;

public class UpdateChatSettingsCommandHandler(IUserContext userContext, IChatSettingsRepository chatSettingsRepository) : IRequestHandler<UpdateChatSettingsCommand, Unit>
{
    public async Task<Unit> Handle(UpdateChatSettingsCommand request, CancellationToken cancellationToken)
    {
        var chatSettings = await chatSettingsRepository.GetByChatIdAsync(request.ChatId, userContext.UserId, cancellationToken);
        if (chatSettings is null)
        {
            chatSettings = new ChatSettings(userContext.UserId, request.IsPinned, request.IsMuted, request.NotificationLevel);
            await chatSettingsRepository.AddAsync(chatSettings, cancellationToken);
        }
        else
        {
            if (request.IsPinned)
                chatSettings.Pin();
            else
                chatSettings.Unpin();

            if (request.IsMuted)
                chatSettings.Mute();
            else
                chatSettings.Unmute();

            chatSettings.UpdateNotificationLevel(request.NotificationLevel);
            await chatSettingsRepository.UpdateAsync(chatSettings, cancellationToken);
        }

        // Save changes to the repository
        await chatSettingsRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return Unit.Value;
    }
}