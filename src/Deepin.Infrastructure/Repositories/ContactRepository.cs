using Deepin.Application.Extensions;
using Deepin.Domain.ContactAggregate;
using Deepin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Deepin.Infrastructure.Repositories;

public class ContactRepository(ContactDbContext db) : RepositoryBase<Contact, ContactDbContext>(db), IContactRepository
{
    public async Task<IEnumerable<Contact>> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var nomalizedEmail = email.ToNomalized();
        if (string.IsNullOrWhiteSpace(nomalizedEmail))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        return await db.Contacts.Where(c => c.Email == nomalizedEmail).ToListAsync(cancellationToken);
    }
}
