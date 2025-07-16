using AutoMapper;
using Deepin.Application.DTOs.Contacts;
using Deepin.Application.Interfaces;
using Deepin.Domain.ContactAggregate;
using MediatR;

namespace Deepin.Application.Commands.Contacts;

public record CreateContactCommand(CreateContactRequest Request) : IRequest<ContactDto>;

public class CreateContactCommandHandler(IMapper mapper, IContactRepository contactRepository, IUserContext userContext) : IRequestHandler<CreateContactCommand, ContactDto>
{
    public async Task<ContactDto> Handle(CreateContactCommand command, CancellationToken cancellationToken)
    {
        var contact = new Contact(
            ownerId: userContext.UserId,
            userId: command.Request.UserId,
            name: command.Request.Name,
            notes: command.Request.Notes);

        await contactRepository.AddAsync(contact, cancellationToken);
        await contactRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
        return mapper.Map<ContactDto>(contact);
    }
}
