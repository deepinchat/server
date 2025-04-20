using Deepin.Application.Commands.Chats;
using FluentValidation;

namespace Deepin.Application.Validations.Chats;

public class CreateChatCommandValidator : AbstractValidator<CreateChatCommand>
{
    public CreateChatCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
    }
}
