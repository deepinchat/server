namespace Deepin.Domain.ContactAggregate;

/// <summary>
/// Repository interface for the Contact aggregate root.
/// Extends the generic IRepository with Contact-specific methods if needed.
/// </summary>
public interface IContactRepository : IRepository<Contact>
{
    // Add Contact-specific query methods here if needed in the future.
    // For example:
    // Task<Contact?> FindByOwnerAndUserIdAsync(Guid ownerId, Guid userId, CancellationToken cancellationToken = default);
    // Task<IEnumerable<Contact>> GetStarredContactsByOwnerAsync(Guid ownerId, CancellationToken cancellationToken = default);
}
