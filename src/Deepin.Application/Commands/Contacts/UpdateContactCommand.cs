using AutoMapper;
using Deepin.Application.DTOs.Contacts;
using Deepin.Domain.ContactAggregate;
using Deepin.Domain.Exceptions;
using MediatR;

namespace Deepin.Application.Commands.Contacts;

public record UpdateContactCommand(Guid Id, ContactRequest Request) : IRequest<ContactDto>;
public class UpdateContactCommandHandler(IMapper mapper, IContactRepository contactRepository) : IRequestHandler<UpdateContactCommand, ContactDto>
{
    public async Task<ContactDto> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
    {
        var contact = await contactRepository.FindByIdAsync(command.Id, cancellationToken);
        if (contact is null)
        {
            throw new EntityNotFoundException(nameof(Contact), command.Id);
        }
        contact.Update(
            command.Request.Name,
            command.Request.FirstName,
            command.Request.LastName,
            command.Request.Company,
            command.Request.Birthday,
            command.Request.Email,
            command.Request.PhoneNumber,
            command.Request.Address,
            command.Request.Notes
        );

        await contactRepository.UpdateAsync(contact, cancellationToken);
        await contactRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return mapper.Map<ContactDto>(contact);
    }
}