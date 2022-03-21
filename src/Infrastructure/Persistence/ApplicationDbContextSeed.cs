using Domain.Entities;

namespace Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedFormDefinitionsAsync(ApplicationDbContext context)
        {
            if (context == null)
                return;

            if (!context.FormDefinitions.Any())
            {
                context.FormDefinitions.Add(new FormDefinition
                {
                    Label = ""
                });

                await context.SaveChangesAsync();
            }
        }
    }
}
