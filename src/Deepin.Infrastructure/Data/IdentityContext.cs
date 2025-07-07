using System.Data;
using Deepin.Domain;
using Deepin.Domain.Identity;
using Deepin.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Deepin.Infrastructure.Data;

public class IdentityContext : IdentityDbContext<User, Role, Guid>, IUnitOfWork
{
    public const string DEFAULT_SCHEMA = "identity";
    private readonly IMediator? _mediator;
    public IdentityContext(DbContextOptions<IdentityContext> options, IMediator? mediator = null) : base(options)
    {
        _mediator = mediator;
    }

    public async Task ExecuteInTransactionAsync(Func<Task> action, CancellationToken cancellationToken = default)
    {
        if (action == null) throw new ArgumentNullException(nameof(action));

        var strategy = Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);
            try
            {
                await action();
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        // Dispatch Domain Events collection. 
        // Choices:
        // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
        // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
        // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
        // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
        if (_mediator is not null)
            await _mediator.DispatchDomainEventsAsync(this);

        // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
        // performed through the DbContext will be committed
        _ = await base.SaveChangesAsync(cancellationToken);

        return true;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasDefaultSchema(DEFAULT_SCHEMA);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        builder.Entity<User>(b =>
        {
            b.ToTable("users");
            b.Property(x => x.CreatedAt).HasColumnType("timestamp with time zone").ValueGeneratedOnAdd().HasDefaultValueSql("now()");
            b.Property(x => x.UpdatedAt).HasColumnType("timestamp with time zone").ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("now()");
            b.HasMany(x => x.Claims)
                .WithOne()
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");

        builder.Entity<Role>(b =>
        {
            b.ToTable("roles");
            b.Property(x => x.CreatedAt).HasColumnType("timestamp with time zone").ValueGeneratedOnAdd().HasDefaultValueSql("now()");
            b.Property(x => x.UpdatedAt).HasColumnType("timestamp with time zone").ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("now()");
        });
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
    }
}
public class IdentityContextFactory : IDesignTimeDbContextFactory<IdentityContext>
{
    public IdentityContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<IdentityContext>();
        optionsBuilder.UseNpgsql("Host=localhost");
        return new IdentityContext(optionsBuilder.Options);
    }
}