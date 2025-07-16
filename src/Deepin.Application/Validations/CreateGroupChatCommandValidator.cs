using Deepin.Application.Commands.Chats;
using FluentValidation;

namespace Deepin.Application.Validations.Chats;

public class CreateGroupChatCommandValidator : AbstractValidator<CreateGroupChatCommand>
{
    public CreateGroupChatCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}
