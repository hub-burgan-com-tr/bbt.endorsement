using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;

namespace Migration.Console.App.Seed
{
    public static class OrderDefinionSeed
    {
        public static async Task SeedOrderDefinitionsAsync(ApplicationDbContext context)
        {
            if (context == null)
                return;
            if (!context.OrderDefinitions.Any())
            {
                var orderdefinition = context.OrderDefinitions.Add(new Domain.Entities.OrderDefinition
                {
                    OrderDefinitionId = Guid.NewGuid().ToString(),
                    Title = "Sigorta Teklif Formu",
                    Created = DateTime.Now,
                    RetryFrequence = 15,
                    ExpireInMinutes = 60,
                    MaxRetryCount = 3,
                    ProcessType = "Sigorta Formları",
                    StateType = "Teklif Formu",

                });
                context.OrderDefinitionActions.Add(new OrderDefinitionAction { OrderDefinitionId = orderdefinition.Entity.OrderDefinitionId, Created = DateTime.Now, Title = "Okudum, onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", OrderDefinitionActionId = Guid.NewGuid().ToString() });
                context.OrderDefinitionActions.Add(new OrderDefinitionAction { OrderDefinitionId = orderdefinition.Entity.OrderDefinitionId, Created = DateTime.Now, Title = "Okudum, onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", OrderDefinitionActionId = Guid.NewGuid().ToString() });

                await context.SaveChangesAsync();
            }
        }

    }
}
