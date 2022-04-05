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
                FormDefinitionId = "b25635e8-1abd-4768-ab97-e1285999a62a",
                Name = "Sigorta Onay Formu",
                Label = label.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "tr-sigorta-onay-formu",
                RetryFrequence = 10,
                Mode = "Completed",
                Url = "",
                Type = ContentType.PDF.ToString(),
                ExpireInMinutes = 60,
                MaxRetryCount = 4,

            });
            formdefinition.Entity.FormDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "" });
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,Anlamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });
           
            var formdefinition2 = context.FormDefinitions.Add(new FormDefinition
            {
                FormDefinitionId = "fff57322-7417-4805-acb0-3691e8540020",
                Name = "Sigorta Teklif Formu",
                Label = label.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "tr-sigorta-teklif-formu",
                RetryFrequence = 10,
                Mode = "Completed",
                Url = "",
                ExpireInMinutes = 60,
                MaxRetryCount = 4,
                Type = ContentType.PDF.ToString(),
            });
            formdefinition2.Entity.FormDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "" });
            formdefinition2.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition2.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,Anlamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });


            var templateName3 = "sigorta_onayformu_test.txt";
            var path3 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateName3);
            var label3 = File.ReadAllText(path3, Encoding.Default);
            var formdefinition3 = context.FormDefinitions.Add(new FormDefinition
            {
                FormDefinitionId = "fff57322-7417-4805-acb0-3691e8540021",
                Name = "Sigorta Teklif Formu Test",
                Label = label3.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "Ugur",
                RetryFrequence = 10,
                Mode = "Completed",
                Url = "",
                ExpireInMinutes = 60,
                MaxRetryCount = 4,
                Type = ContentType.PDF.ToString(),
            });
            formdefinition3.Entity.FormDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "" });
            formdefinition3.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,anladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition3.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,Anlamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });


            await context.SaveChangesAsync();
        }
    }
}

