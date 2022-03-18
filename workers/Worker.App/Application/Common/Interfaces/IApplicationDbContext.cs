using Microsoft.EntityFrameworkCore;
using Worker.App.Domain.Entities;
using Action = Worker.App.Domain.Entities.Action;

namespace Worker.App.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Order> Orders { get;}
        DbSet<Reference> References { get;  }
        DbSet<Callback> Callbacks { get; }
        DbSet<Config> Configs { get; }
        DbSet<FormDefinition> FormDefinitions { get; }
        DbSet<FormDefinitionTag> FormDefinitionTags { get; }
        DbSet<Action> Actions { get; }
        DbSet<Document> Documents { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
