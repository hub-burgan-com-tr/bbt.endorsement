using Microsoft.EntityFrameworkCore;
using Worker.App.Domain.Entities;
using DocumentAction = Worker.App.Domain.Entities.DocumentAction;

namespace Worker.App.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get; }
        DbSet<Reference> References { get; }
        DbSet<Callback> Callbacks { get; }
        DbSet<Config> Configs { get; }
        DbSet<FormDefinition> FormDefinitions { get; }
        DbSet<FormDefinitionAction> FormDefinitionActions { get; }
        DbSet<FormDefinitionTag> FormDefinitionTags { get; }
        DbSet<DocumentAction> DocumentActions { get; }
        DbSet<Document> Documents { get; }
        DbSet<OrderHistory> OrderHistories { get; }
        DbSet<Approver> Approvers { get; }
        DbSet<Customer> Customers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
