using Deepin.Application.Commands.Chats;
using FluentValidation;

namespace Deepin.Application.Validations.Chats;

public class CreateDirectChatCommandValidator : AbstractValidator<CreateDirectChatCommand>
{
    public CreateDirectChatCommandValidator()
    {
        RuleFor(x => x.OwnerId).NotEmpty().WithMessage("Chat owner id is required");
        RuleFor(x => x.Others)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one other user id is required");
    }
}
