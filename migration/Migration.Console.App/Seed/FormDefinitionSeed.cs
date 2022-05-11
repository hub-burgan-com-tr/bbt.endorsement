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
            var templateName = "sigorta_basvuruformu.txt";
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", templateName);
            var label = File.ReadAllText(path, Encoding.Default);
            var formdefinition = context.FormDefinitions.Add(new FormDefinition
            {
                FormDefinitionId = "b25635e8-1abd-4768-ab97-e1285999a61a",
                DocumentSystemId= "b25635e8-1abd-4768-ab97-e1285999a62b",
                Name = "Sigorta Başvuru Formu",
                Label = label.ToString(),
                Created = DateTime.Now,
                Tags = "",
                TemplateName = "tr-sigorta-basvuru-formu",
                RetryFrequence = 15,
                Mode = "Completed",
                Url = "",
                Type = ContentType.HTML.ToString(),
                ExpireInMinutes = 60,
                MaxRetryCount = 3,

            });                    
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
            formdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });
          
            List<FormDefinitionTag> formDefinitionTags = new List<FormDefinitionTag>();
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Hazine  Formları" });
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Mevduat Formları" });
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Sigorta Formları" });
            var formdefinitiontag =context.FormDefinitionTags.AddRangeAsync(formDefinitionTags);
            if (formdefinitiontag!=null)
            {
                foreach (var item in formDefinitionTags)
                {
                    if (item.Tag == "Sigorta Formları")
                    {
                        formdefinition.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap { FormDefinitionTagMapId = Guid.NewGuid().ToString(), FormDefinitionId = formdefinition.Entity.FormDefinitionId, FormDefinitionTagId = item.FormDefinitionTagId });
                    }
                }
            }
            await context.SaveChangesAsync();
        }
    }
}