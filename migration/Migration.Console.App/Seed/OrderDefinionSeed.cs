using Domain.Enums;
using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migration.Console.App.Seed
{
    public static class OrderDefinionSeed
    {
        public static async Task SeedFormDefinitionsAsync(ApplicationDbContext context)
        {
            if (context == null)
                return;
            if (!context.OrderDefinitions.Any())
            {
               
                var orderdefinition = context.OrderDefinitions.Add(new Domain.Entities.OrderDefinition
                {
                    OrderDefinitionId = Guid.NewGuid().ToString(),
                    Title="Sigorta Teklif Formu",
                    Created = DateTime.Now,
                    RetryFrequence = 15,
                    ExpireInMinutes = 60,
                    MaxRetryCount = 3,
                    ProcessType = "Sigorta Formları",
                    StateType= "Teklif Formu",

                });
                context.OrderDefinitionActions.Add(new Domain.Entities.OrderDefinitionAction { Created = DateTime.Now, Title = "Okudum, onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", OrderDefinitionActionId = Guid.NewGuid().ToString() });
                context.OrderDefinitionActions.Add(new Domain.Entities.OrderDefinitionAction { Created = DateTime.Now, Title = "Okudum, onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", OrderDefinitionActionId = Guid.NewGuid().ToString() });

                await context.SaveChangesAsync();
            }
        }

    }
}
