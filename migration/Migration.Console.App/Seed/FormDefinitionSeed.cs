using Infrastructure.Persistence;
using System.Text;
using Domain.Entities;
using Domain.Enums;

namespace Migration.Console.App.Seed;

public static class FormDefinitionSeed
{
    public static async Task SeedFormDefinitionAsync(ApplicationDbContext context)
    {
        if (context == null)
            return;
        if (!context.FormDefinitions.Any())
        {

            List<FormDefinitionTag> formDefinitionTags = new List<FormDefinitionTag>();
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Hazine  Formları" });
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Mevduat Formları" });
            formDefinitionTags.Add(new FormDefinitionTag { Created = DateTime.Now, FormDefinitionTagId = Guid.NewGuid().ToString(), Tag = "Sigorta Formları", IsProcessNo = true });
            var formdefinitiontags = context.FormDefinitionTags.AddRangeAsync(formDefinitionTags);
            await context.SaveChangesAsync();

            var parameters = context.Parameters.Where(x => x.ParameterType.Name == "Dys Form Kategorileri");
            int basvuru = 1000;
            int teklif = 2000;
            foreach (var parameter in parameters)
            {
                var basvuruFormuName = "Sigorta Başvuru Formu - " + parameter.Text;
                var basvuruTemplateName = "tr-sigorta-basvuru-formu-" + parameter.Text;

                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", basvuruTemplateName + ".txt");
                var label = File.ReadAllText(path, Encoding.Default);

                var basvuruTemplateHtml = "tr-sigorta-basvuru-formu-" + parameter.Text + ".html";
                var htmlTemplate = StaticValuesExtensions.HtmlToString(basvuruTemplateHtml);

                var formDefinitionId = "fd95116e-e7e0-4cdf-b734-11c414c3" + basvuru;
                var başvuruFormdefinition = context.FormDefinitions.Add(new FormDefinition
                {
                    FormDefinitionId = formDefinitionId,
                    DocumentSystemId = "b25635e8-1abd-4768-ab97-e1285999" + basvuru,
                    Name = basvuruFormuName,
                    Label = label.ToString(),
                    Created = DateTime.Now,
                    Tags = "",
                    TemplateName = basvuruTemplateName,
                    RetryFrequence = 15,
                    Mode = "Completed",
                    Url = "",
                    Type = ContentType.PDF.ToString(),
                    ExpireInMinutes = 60,
                    MaxRetryCount = 3,
                    DependencyReuse = false,
                    Source = "formio",
                    ParameterId = parameter.ParameterId.ToString(),
                    HtmlTemplate = htmlTemplate
                });
                başvuruFormdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
                başvuruFormdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });



                var teklifFormuName = "Sigorta Teklif Formu - " + parameter.Text;

                var teklifTemplateName = "tr-sigorta-teklif-formu-" + parameter.Text + ".txt";
                var teklifPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory!, "Files", teklifTemplateName);
                var tekliflabel = File.ReadAllText(teklifPath, Encoding.Default);
                var teklifFormdefinition = context.FormDefinitions.Add(new FormDefinition
                {
                    FormDefinitionId = "04619ad5-5bf0-4651-a7cd-ca6ffa39" + teklif,
                    DependencyFormId = formDefinitionId,
                    DocumentSystemId = "b25635e8-1abd-4768-ab97-e1285999" + teklif,
                    Name = teklifFormuName,
                    Label = tekliflabel.ToString(),
                    Created = DateTime.Now,
                    Tags = "",
                    TemplateName = "tr-sigorta-teklif-formu-" + parameter.Text,
                    RetryFrequence = 15,
                    Mode = "Completed",
                    Url = "",
                    Type = ContentType.PDF.ToString(),
                    ExpireInMinutes = 60,
                    MaxRetryCount = 3,
                    Source = "file",
                    ParameterId = parameter.ParameterId.ToString()
                });
                teklifFormdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onayladım", Choice = 1, Type = ActionType.Approve.ToString(), State = "Onay", FormDefinitionActionId = Guid.NewGuid().ToString() });
                teklifFormdefinition.Entity.FormDefinitionActions.Add(new FormDefinitionAction { Created = DateTime.Now, Title = "Okudum, onaylamadım", Choice = 2, Type = ActionType.Reject.ToString(), State = "Red", FormDefinitionActionId = Guid.NewGuid().ToString() });

                if (formdefinitiontags != null)
                {
                    foreach (var item in formDefinitionTags)
                    {
                        if (item.Tag == "Sigorta Formları")
                        {
                            başvuruFormdefinition.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap { FormDefinitionTagMapId = Guid.NewGuid().ToString(), FormDefinitionId = başvuruFormdefinition.Entity.FormDefinitionId, FormDefinitionTagId = item.FormDefinitionTagId });
                            teklifFormdefinition.Entity.FormDefinitionTagMaps.Add(new FormDefinitionTagMap { FormDefinitionTagMapId = Guid.NewGuid().ToString(), FormDefinitionId = teklifFormdefinition.Entity.FormDefinitionId, FormDefinitionTagId = item.FormDefinitionTagId });
                        }
                    }
                }

                await context.SaveChangesAsync();

                basvuru++;
                teklif++;
            }


        }
    }
}