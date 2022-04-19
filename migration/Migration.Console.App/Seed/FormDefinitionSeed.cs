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
                DocumentSystemId= "b25635e8-1abd-4768-ab97-e1285999a62b",
                Name = "Sigorta Onay Formu",
                Label = label.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "tr-sigorta-onay-formu",
                RetryFrequence = 15,
                Mode = "Completed",
                Url = "",
                Type = ContentType.HTML.ToString(),
                ExpireInMinutes = 60,
                MaxRetryCount = 3,

            });
        
                      
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });
            var templateNamepdf = "sigorta_teklifformu.txt";
            var pathpdf = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateNamepdf);
            var labelpdf = File.ReadAllText(pathpdf, Encoding.Default);
            var formdefinition2 = context.FormDefinitions.Add(new FormDefinition
            {
                FormDefinitionId = "b25635e8-1abd-4768-ab97-e1285999a62b",
                DocumentSystemId = "b25635e8-1abd-4768-ab97-e1285999a62c",
                Name = "Sigorta Teklif Formu",
                Label = labelpdf.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "tr-sigorta-teklif-formu",
                RetryFrequence = 15,
                Mode = "Completed",
                Url = "",
                Type = ContentType.PDF.ToString(),
                ExpireInMinutes = 60,
                MaxRetryCount = 3

            });
            formdefinition2.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition2.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum,onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });
            List<FormDefinitionTag> formDefinitionTags = new List<FormDefinitionTag>();
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Konut" });
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Kasko" });
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Trafik" });
            var formdefinitiontag = context.FormDefinitionTags.AddRangeAsync(formDefinitionTags);
            foreach (var item in formDefinitionTags)
            {
                formdefinition.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap {FormDefinitionTagMapId=Guid.NewGuid().ToString(), FormDefinitionId = formdefinition.Entity.FormDefinitionId, FormDefinitionTagId = item.FormDefinitionTagId });
                formdefinition2.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap { FormDefinitionTagMapId = Guid.NewGuid().ToString(), FormDefinitionId = formdefinition2.Entity.FormDefinitionId, FormDefinitionTagId = item.FormDefinitionTagId });
            }

            await context.SaveChangesAsync();
        }
    }
}

