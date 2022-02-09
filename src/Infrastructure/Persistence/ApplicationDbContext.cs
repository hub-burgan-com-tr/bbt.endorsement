using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IDateTime _dateTime;
    private readonly IDomainEventService _domainEventService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTime dateTime, IDomainEventService domainEventService) : base(options)
    {
        _dateTime = dateTime;
        _domainEventService = domainEventService;
    }

    public virtual DbSet<Approval> Approvals { get; set; }
    public virtual DbSet<Reference> References { get; set; }
    public virtual DbSet<Callback> Callbacks { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Server=.;Database=ContractApproval;integrated security=sspi;Trusted_Connection=True;");
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    //entry.Entity.CreatedBy = _currentUserService.UserId;
                    entry.Entity.Created = _dateTime.Now;
                    break;

                case EntityState.Modified:
                    // entry.Entity.LastModifiedBy = _currentUserService.UserId;
                    entry.Entity.LastModified = _dateTime.Now;
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
                .Select(x => x.Entity.DomainEvents)
                .SelectMany(x => x)
                .Where(domainEvent => !domainEvent.IsPublished)
                .ToArray();

        var result = base.SaveChanges();
        // var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchEvents(events);
        return result;
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}