using Domain.Entities;
using Infrastructure.Persistence;

namespace Migration.Console.App.Seed
{
    public static class ParamaterSeed
    {
        public static async Task SeedFormDefinitionsAsync(ApplicationDbContext context)
        {
            if (context == null)
                return;

            if (!context.ParameterTypes.Any())
            {
                var parameterType = context.ParameterTypes.Add(new ParameterType { Name = "İşlem", Created = DateTime.Now });
                parameterType.Entity.Parameters.Add(new Parameter { Text = "Sigorta Formları", Created = DateTime.Now,IsProcessNo=true });
                var parameterType2 = context.ParameterTypes.Add(new ParameterType { Name = "Aşama", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Parameter { Text = "Başvuru Formu", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Parameter { Text = "Teklif Formu", Created = DateTime.Now });

                var dysParameter = context.ParameterTypes.Add(new ParameterType { Name = "Dys Form Kategorileri" }).Entity;
                dysParameter.Parameters.Add(new Parameter { Text = "Esya", Created = DateTime.Now, DmsReferenceId = 1831, DmsReferenceKey=1210,DmsReferenceName= "Konut Sigorta Poliçesi" });
                dysParameter.Parameters.Add(new Parameter { Text = "Konut", Created = DateTime.Now, DmsReferenceId = 1831, DmsReferenceKey = 1210,DmsReferenceName= "Konut Sigorta Poliçesi" });               
                dysParameter.Parameters.Add(new Parameter { Text = "DASK", Created = DateTime.Now, DmsReferenceId = 1832, DmsReferenceKey = 1210 , DmsReferenceName = "Dask Sigorta Poliçesi" });
                dysParameter.Parameters.Add(new Parameter { Text = "Kasko", Created = DateTime.Now, DmsReferenceId = 1829, DmsReferenceKey = 1220, DmsReferenceName= "Kasko Sigorta Poliçesi" });
                dysParameter.Parameters.Add(new Parameter { Text = "Trafik", Created = DateTime.Now, DmsReferenceId = 1830, DmsReferenceKey = 1220, DmsReferenceName = "Trafik Sigorta Poliçesi" });               
                dysParameter.Parameters.Add(new Parameter { Text = "İsyeri", Created = DateTime.Now, DmsReferenceId = 1833, DmsReferenceKey = 1210, DmsReferenceName = "Diğer Elementer Sigorta Poliçesi" });
                dysParameter.Parameters.Add(new Parameter { Text = "Diger", Created = DateTime.Now, DmsReferenceId = 1833, DmsReferenceKey = 1200, DmsReferenceName = "Diğer Elementer Kasko Poliçesi" });
              
                await context.SaveChangesAsync();
            }
        }

    }
}
