using Infrastructure.Persistence;
using System.Text;
using Application.Endorsements.Commands.NewOrders;
using Domain.Entities;

namespace Migration.Console.App.Seed;

public static class FormDefinitionSeed
{
    public static async Task SeedFormDefinitionsAsync(ApplicationDbContext context)
    {
        if (context == null)
            return;

        if (!context.FormDefinitions.Any())
        {
            var templateName = "sigorta_onayformu.txt";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateName);
            var label = File.ReadAllText(path, Encoding.Default);

            var formdefinition = context.FormDefinitions.Add(new FormDefinition
            {
                FormDefinitionId = Guid.NewGuid().ToString(),
                Name = "Sigorta Onay Formu",
                Label = label.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "sigorta_onayformu.pdf",
                RetryFrequence = 10,
                Mode = "Completed",
                Url = "",
                Type = ContentType.PDF.ToString(),
                ExpireInMinutes = 60,
                MaxRetryCount = 4,

            });
            formdefinition.Entity.FormDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "" });

            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anladım", IsDefault = true, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,Anlamadım", IsDefault = false, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });

            var formdefinition2 = context.FormDefinitions.Add(new FormDefinition
            {
                FormDefinitionId = Guid.NewGuid().ToString(),
                Name = "Sigorta Teklif Formu",
                Label = label.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "sigorta_onayformu.pdf",
                RetryFrequence = 10,
                Mode = "Completed",
                Url = "",
                ExpireInMinutes = 60,
                MaxRetryCount = 4,
                Type = ContentType.PDF.ToString(),


            });
            formdefinition2.Entity.FormDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "" });
            formdefinition2.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anladım", IsDefault = true, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition2.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,Anlamadım", IsDefault = false, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });
            await context.SaveChangesAsync();
        }
    }
}

