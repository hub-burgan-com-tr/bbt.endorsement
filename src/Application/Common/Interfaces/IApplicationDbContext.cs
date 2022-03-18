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
        DbSet<FormDefinition> Forms { get; }
        DbSet<Domain.Entities.Action> Actions { get; }
        DbSet<Document> Documents { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
