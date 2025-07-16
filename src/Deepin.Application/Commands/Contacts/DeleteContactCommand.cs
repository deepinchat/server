using System;
using Deepin.Domain.ContactAggregate;
using MediatR;

namespace Deepin.Application.Commands.Contacts;

public record DeleteContactCommand(Guid Id) : IRequest<bool>;

public class DeleteContactCommandHandler(IContactRepository contactRepository) : IRequestHandler<DeleteContactCommand, bool>
{
    public async Task<bool> Handle(DeleteContactCommand command, CancellationToken cancellationToken)
    {
        var contact = await contactRepository.FindByIdAsync(command.Id, cancellationToken);
        if (contact is null)
        {
            return false; // Contact not found
        }
        contact.Delete();
        await contactRepository.UpdateAsync(contact);
        await contactRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return true; // Contact deleted successfully
    }
}