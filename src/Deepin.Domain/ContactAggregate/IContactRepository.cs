namespace Deepin.Domain.ContactAggregate;

public interface IContactRepository : IRepository<Contact>
{
    Task<IEnumerable<Contact>> GetByEmailAsync(string email, CancellationToken cancellationToken);
}
