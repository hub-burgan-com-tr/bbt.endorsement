using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Worker.App.Domain.Common;
using Worker.App.Domain.Entities;
using Worker.App.Application.Common.Interfaces;

namespace Worker.App.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    private readonly IDateTime _dateTime;
    private readonly IDomainEventService _domainEventService;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTime dateTime, IDomainEventService domainEventService) : base(options)
    {
        _dateTime = dateTime;
        _domainEventService = domainEventService;
    }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Reference> References { get; set; }
    public virtual DbSet<Callback> Callbacks { get; set; }
    public virtual DbSet<Config> Configs { get; set; }
    public virtual DbSet<Document> Documents { get; set; }
    public virtual DbSet<OrderHistory> OrderHistories { get; set; }
    public virtual DbSet<FormDefinition> FormDefinitions { get; set; }
    public virtual DbSet<FormDefinitionTag> FormDefinitionTags { get; set; }
    public virtual DbSet<DocumentAction> DocumentActions { get; set; }
    public virtual DbSet<FormDefinitionAction> FormDefinitionActions { get; set; }
    public virtual DbSet<Approver> Approvers { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }


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

        var result = await base.SaveChangesAsync(cancellationToken);
        await DispatchEvents(events);
        return result;
    }

    public override int SaveChanges()
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
        _ = DispatchEvents(events);
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


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Order>()
               .HasOne(s => s.Config)
               .WithOne(ad => ad.Order)
               .HasForeignKey<Config>(ad => ad.OrderId);

        builder.Entity<Order>()
                .HasOne(s => s.Reference)
                .WithOne(ad => ad.Order)
                .HasForeignKey<Reference>(ad => ad.OrderId);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}