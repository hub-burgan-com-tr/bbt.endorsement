using Application.Common.Interfaces;
using Infrastructure.Persistence;
using Migration.Console.App.Seed;

namespace Migration.Console.App
{
    public interface IMigrationService
    {
        void Migrate();
    }
    public class MigrationService : IMigrationService
    {
        private readonly ApplicationDbContext _context;

        public MigrationService(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void Migrate()
        {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            FormDefinitionSeed.SeedFormDefinitionsAsync(_context).Wait();   
        }
    }
}
