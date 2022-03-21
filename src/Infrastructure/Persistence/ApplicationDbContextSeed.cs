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
                    FormDefinitionId=Guid.NewGuid().ToString(),
                    Name= "Sigorta Onay Formu",
                    Label = "{\"_id\":\"62383a77aa714503010ebd41\",\"type\":\"form\",\"tags\":[],\"owner\":\"62303003a89aea78a3bbe972\",\"components\":[{\"label\":\"Columns\",\"columns\":[{\"components\":[{\"label\":\"TCKN\",\"tableView\":true,\"validate\":{\"required\":true},\"errors\":{\"required\":\"{{field}}girilmelidir.\"},\"key\":\"identityNo\",\"type\":\"textfield\",\"input\":true}],\"width\":4,\"offset\":0,\"push\":0,\"pull\":0,\"size\":\"md\",\"currentWidth\":4},{\"components\":[{\"label\":\"AdSoyad\",\"tableView\":true,\"validate\":{\"required\":true},\"errors\":{\"required\":\"{{field}}girilmelidir.\"},\"key\":\"nameSurname\",\"type\":\"textfield\",\"input\":true}],\"width\":4,\"offset\":0,\"push\":0,\"pull\":0,\"size\":\"md\",\"currentWidth\":4}],\"key\":\"columns\",\"type\":\"columns\",\"input\":false,\"tableView\":false}],\"revisions\":\"\",\"_vid\":0,\"title\":\"test\",\"display\":\"form\",\"access\":[{\"roles\":[],\"type\":\"create_own\"},{\"roles\":[],\"type\":\"create_all\"},{\"roles\":[],\"type\":\"read_own\"},{\"roles\":[\"623837a00d68984109e98e2c\",\"623837a00d68982f3fe98e31\",\"623837a00d68984dc1e98e36\"],\"type\":\"read_all\"},{\"roles\":[],\"type\":\"update_own\"},{\"roles\":[],\"type\":\"update_all\"},{\"roles\":[],\"type\":\"delete_own\"},{\"roles\":[],\"type\":\"delete_all\"},{\"roles\":[],\"type\":\"team_read\"},{\"roles\":[],\"type\":\"team_write\"},{\"roles\":[],\"type\":\"team_admin\"}],\"submissionAccess\":[{\"roles\":[],\"type\":\"create_own\"},{\"roles\":[],\"type\":\"create_all\"},{\"roles\":[],\"type\":\"read_own\"},{\"roles\":[],\"type\":\"read_all\"},{\"roles\":[],\"type\":\"update_own\"},{\"roles\":[],\"type\":\"update_all\"},{\"roles\":[],\"type\":\"delete_own\"},{\"roles\":[],\"type\":\"delete_all\"},{\"roles\":[],\"type\":\"team_read\"},{\"roles\":[],\"type\":\"team_write\"},{\"roles\":[],\"type\":\"team_admin\"}],\"controller\":\"\",\"properties\":{},\"settings\":{},\"path\":\"test\",\"name\":\"test\",\"project\":\"623837a00d68982154e98e25\",\"created\":\"2022-03-21T08:42:31.078Z\",\"modified\":\"2022-03-21T10:16:29.149Z\",\"machineName\":\"uhhtdmxwdexwqsq:test\"}",
                    Created=DateTime.Now,
                    Tags="",
                    TemplateName="sigorta_onayformu.pdf",

                });
                context.FormDefinitions.Add(new FormDefinition
                {
                    FormDefinitionId = Guid.NewGuid().ToString(),
                    Name = "Sigorta Teklif Formu",
                    Label = "{\"_id\":\"62383a77aa714503010ebd41\",\"type\":\"form\",\"tags\":[],\"owner\":\"62303003a89aea78a3bbe972\",\"components\":[{\"label\":\"Columns\",\"columns\":[{\"components\":[{\"label\":\"TCKN\",\"tableView\":true,\"validate\":{\"required\":true},\"errors\":{\"required\":\"{{field}}girilmelidir.\"},\"key\":\"identityNo\",\"type\":\"textfield\",\"input\":true}],\"width\":4,\"offset\":0,\"push\":0,\"pull\":0,\"size\":\"md\",\"currentWidth\":4},{\"components\":[{\"label\":\"AdSoyad\",\"tableView\":true,\"validate\":{\"required\":true},\"errors\":{\"required\":\"{{field}}girilmelidir.\"},\"key\":\"nameSurname\",\"type\":\"textfield\",\"input\":true}],\"width\":4,\"offset\":0,\"push\":0,\"pull\":0,\"size\":\"md\",\"currentWidth\":4}],\"key\":\"columns\",\"type\":\"columns\",\"input\":false,\"tableView\":false}],\"revisions\":\"\",\"_vid\":0,\"title\":\"test\",\"display\":\"form\",\"access\":[{\"roles\":[],\"type\":\"create_own\"},{\"roles\":[],\"type\":\"create_all\"},{\"roles\":[],\"type\":\"read_own\"},{\"roles\":[\"623837a00d68984109e98e2c\",\"623837a00d68982f3fe98e31\",\"623837a00d68984dc1e98e36\"],\"type\":\"read_all\"},{\"roles\":[],\"type\":\"update_own\"},{\"roles\":[],\"type\":\"update_all\"},{\"roles\":[],\"type\":\"delete_own\"},{\"roles\":[],\"type\":\"delete_all\"},{\"roles\":[],\"type\":\"team_read\"},{\"roles\":[],\"type\":\"team_write\"},{\"roles\":[],\"type\":\"team_admin\"}],\"submissionAccess\":[{\"roles\":[],\"type\":\"create_own\"},{\"roles\":[],\"type\":\"create_all\"},{\"roles\":[],\"type\":\"read_own\"},{\"roles\":[],\"type\":\"read_all\"},{\"roles\":[],\"type\":\"update_own\"},{\"roles\":[],\"type\":\"update_all\"},{\"roles\":[],\"type\":\"delete_own\"},{\"roles\":[],\"type\":\"delete_all\"},{\"roles\":[],\"type\":\"team_read\"},{\"roles\":[],\"type\":\"team_write\"},{\"roles\":[],\"type\":\"team_admin\"}],\"controller\":\"\",\"properties\":{},\"settings\":{},\"path\":\"test\",\"name\":\"test\",\"project\":\"623837a00d68982154e98e25\",\"created\":\"2022-03-21T08:42:31.078Z\",\"modified\":\"2022-03-21T10:16:29.149Z\",\"machineName\":\"uhhtdmxwdexwqsq:test\"}",
                    Created = DateTime.Now,
                    Tags = "",
                    TemplateName = "sigorta_teklifformu.pdf",

                });
                await context.SaveChangesAsync();
            }
        }
    }
}
