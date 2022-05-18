using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Worker.App.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; }
        DbSet<OrderGroup> OrderGroups { get; }
        DbSet<OrderMap> OrderMaps { get; }
        DbSet<Reference> References { get; }
        DbSet<Callback> Callbacks { get; }
        DbSet<Config> Configs { get; }
        DbSet<FormDefinition> FormDefinitions { get; }
        DbSet<FormDefinitionAction> FormDefinitionActions { get; }
        DbSet<FormDefinitionTag> FormDefinitionTags { get; }
        DbSet<DocumentAction> DocumentActions { get; }
        DbSet<Domain.Entities.Document> Documents { get; }
        DbSet<OrderHistory> OrderHistories { get; }
        DbSet<Person> Persons { get; }
        DbSet<Customer> Customers { get; }
        DbSet<ParameterType> ParameterTypes { get; }
        DbSet<Parameter> Parameters { get; }
        DbSet<FormDefinitionTagMap> FormDefinitionTagMaps { get; }
        DbSet<OrderDefinition> OrderDefinitions { get; }
        DbSet<OrderDefinitionAction> OrderDefinitionActions { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
