using Deepin.Domain.ContactAggregate;
using Deepin.Infrastructure.Data;

namespace Deepin.Infrastructure.Repositories;

public class ContactRepository(ContactDbContext db) : RepositoryBase<Contact, ContactDbContext>(db), IContactRepository
{

}
