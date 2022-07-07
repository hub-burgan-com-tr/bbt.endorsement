using Domain.Entities;
using Infrastructure.Persistence;

namespace Migration.Console.App.Seed
{
    public static class ParamaterSeed
    {
        public static async Task SeedFormParametersAsync(ApplicationDbContext context)
        {
            if (context == null)
                return;

            if (!context.ParameterTypes.Any())
            {
                var islemParameterTypeId = "66265975-f6f4-42f4-97ef-d48dea141000";
                var islemParameterType = context.ParameterTypes.Add(new ParameterType { ParameterTypeId = islemParameterTypeId, Name = "İşlem", Created = DateTime.Now });
                islemParameterType.Entity.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea141001", ParameterTypeId = islemParameterTypeId, Text = "Sigorta Formları", Created = DateTime.Now });

                var asamaParameterTypeId = "66265975-f6f4-42f4-97ef-d48dea142000";
                var asamaParameterType = context.ParameterTypes.Add(new ParameterType { ParameterTypeId = asamaParameterTypeId, Name = "Aşama", Created = DateTime.Now });
                asamaParameterType.Entity.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea142001", ParameterTypeId= asamaParameterTypeId, Text = "Başvuru Formu", Created = DateTime.Now });
                asamaParameterType.Entity.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea142002", ParameterTypeId = asamaParameterTypeId, Text = "Teklif Formu", Created = DateTime.Now });

                var dysParameterTypeId = "66265975-f6f4-42f4-97ef-d48dea143000";
                var dysParameter = context.ParameterTypes.Add(new ParameterType { ParameterTypeId = dysParameterTypeId, Name = "Dys Form Kategorileri" }).Entity;
                dysParameter.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea143001", ParameterTypeId = dysParameterTypeId, Text = "Konut Esya", Created = DateTime.Now, DmsReferenceId = 1831, DmsReferenceKey=1210,DmsReferenceName= "Konut Sigorta Poliçesi" });
                dysParameter.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea143002", ParameterTypeId = dysParameterTypeId, Text = "Konut", Created = DateTime.Now, DmsReferenceId = 1831, DmsReferenceKey = 1210,DmsReferenceName= "Konut Sigorta Poliçesi" });               
                dysParameter.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea143003", ParameterTypeId = dysParameterTypeId, Text = "DASK", Created = DateTime.Now, DmsReferenceId = 1832, DmsReferenceKey = 1210 , DmsReferenceName = "Dask Sigorta Poliçesi" });
                dysParameter.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea143004", ParameterTypeId = dysParameterTypeId, Text = "Kasko", Created = DateTime.Now, DmsReferenceId = 1829, DmsReferenceKey = 1220, DmsReferenceName= "Kasko Sigorta Poliçesi" });
                dysParameter.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea143005", ParameterTypeId = dysParameterTypeId, Text = "Trafik", Created = DateTime.Now, DmsReferenceId = 1830, DmsReferenceKey = 1220, DmsReferenceName = "Trafik Sigorta Poliçesi" });               
                dysParameter.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea143006", ParameterTypeId = dysParameterTypeId, Text = "İsyeri", Created = DateTime.Now, DmsReferenceId = 1833, DmsReferenceKey = 1210, DmsReferenceName = "Diğer Elementer Sigorta Poliçesi" });
                dysParameter.Parameters.Add(new Parameter { ParameterId = "66265975-f6f4-42f4-97ef-d48dea143007", ParameterTypeId = dysParameterTypeId, Text = "Diger", Created = DateTime.Now, DmsReferenceId = 1833, DmsReferenceKey = 1200, DmsReferenceName = "Diğer Elementer Kasko Poliçesi" });
              
                await context.SaveChangesAsync();
            }
        }

    }
}
