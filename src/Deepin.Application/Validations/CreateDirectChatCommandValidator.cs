using Deepin.Application.Commands.Chats;
using FluentValidation;

namespace Deepin.Application.Validations.Chats;

public class CreateDirectChatCommandValidator : AbstractValidator<CreateDirectChatCommand>
{
    public CreateDirectChatCommandValidator()
    {
        RuleFor(x => x.UserIds).NotEmpty().WithMessage("UserIds is required");
    }
}
