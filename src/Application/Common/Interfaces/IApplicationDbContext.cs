using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get;}
        DbSet<Reference> References { get;  }
        DbSet<Callback> Callbacks { get; }
        DbSet<Config> Configs { get; }
        DbSet<FormDefinition> FormDefinitions { get; }
        DbSet<FormDefinitionAction> FormDefinitionActions { get; }
        DbSet<FormDefinitionTag> FormDefinitionTags { get; }
        DbSet<DocumentAction> DocumentActions { get; }
        DbSet<Document> Documents { get; }
        DbSet<Approver> Approvers { get; }
        DbSet<Customer> Customers { get; }
        DbSet<OrderHistory> OrderHistories { get; }
        DbSet<ParameterType> ParameterTypes { get; }
        DbSet<Parameter> Parameters { get; }
        DbSet<FormDefinitionTagMap> FormDefinitionTagMaps { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
