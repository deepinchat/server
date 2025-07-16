using AutoMapper;
using Deepin.Application.DTOs.Contacts;
using Deepin.Domain.ContactAggregate;
using MediatR;

namespace Deepin.Application.Commands.Contacts;

public record CreateContactCommand(Guid OwnerId, ContactRequest Request) : IRequest<ContactDto>;

public class CreateContactCommandHandler(IMapper mapper, IContactRepository contactRepository) : IRequestHandler<CreateContactCommand, ContactDto>
{
    public async Task<ContactDto> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        var contact = new Contact(
            createdBy: command.OwnerId,
            userId: command.Request.UserId,
            name: command.Request.Name,
            firstName: command.Request.FirstName,
            lastName: command.Request.LastName,
            company: command.Request.Company,
            birthday: command.Request.Birthday,
            email: command.Request.Email,
            phoneNumber: command.Request.PhoneNumber,
            address: command.Request.Address,
            notes: command.Request.Notes
        );

        await contactRepository.AddAsync(contact, cancellationToken);
        await contactRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return mapper.Map<ContactDto>(contact);
    }
}
