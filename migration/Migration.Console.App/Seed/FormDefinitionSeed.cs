using Infrastructure.Persistence;
using System.Text;
using Application.Endorsements.Commands.NewOrders;
using Domain.Entities;
using Domain.Enums;

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
                FormDefinitionId = "b25635e8-1abd-4768-ab97-e1285999a62a",
                Name = "Sigorta Onay Formu",
                Label = label.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "tr-sigorta-onay-formu",
                RetryFrequence = 10,
                Mode = "Completed",
                Url = "",
                Type = ContentType.HTML.ToString(),
                ExpireInMinutes = 60,
                MaxRetryCount = 4,

            });
            formdefinition.Entity.FormDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "" });
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anlamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });
            var templateNamepdf = "sigorta_teklifformu.txt";
            var pathpdf = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateNamepdf);
            var labelpdf = File.ReadAllText(pathpdf, Encoding.Default);
            var formdefinition2 = context.FormDefinitions.Add(new FormDefinition
            {
                FormDefinitionId = "b25635e8-1abd-4768-ab97-e1285999a62b",
                Name = "Sigorta Teklif Formu",
                Label = labelpdf.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "tr-sigorta-teklif-formu",
                RetryFrequence = 10,
                Mode = "Completed",
                Url = "",
                Type = ContentType.PDF.ToString(),
                ExpireInMinutes = 60,
                MaxRetryCount = 4,

            });
            formdefinition2.Entity.FormDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "" });
            formdefinition2.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition2.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anlamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });

            await context.SaveChangesAsync();
        }
    }
}

