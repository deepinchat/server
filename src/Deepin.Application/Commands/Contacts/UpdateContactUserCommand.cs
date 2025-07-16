using Deepin.Domain.ContactAggregate;
using MediatR;

namespace Deepin.Application.Commands.Contacts;


public record UpdateContactUserCommand(Guid UserId, string Email) : IRequest<bool>;
public class UpdateContactUserCommandHandler(IContactRepository contactRepository) : IRequestHandler<UpdateContactUserCommand, bool>
{

    public async Task<bool> Handle(UpdateContactUserCommand request, CancellationToken cancellationToken)
    {
        var contacts = await contactRepository.GetByEmailAsync(request.Email, cancellationToken);
        if (contacts == null || !contacts.Any())
        {
            return false; // No contacts found with the given email
        }

        foreach (var contact in contacts)
        {
            contact.UpdateUserId(request.UserId);
            await contactRepository.UpdateAsync(contact, cancellationToken);
        }

        await contactRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return true;
    }
}