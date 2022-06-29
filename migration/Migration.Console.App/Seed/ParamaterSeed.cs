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
                parameterType.Entity.Parameters.Add(new Parameter { Text = "Sigorta Formları", Created = DateTime.Now });
                var parameterType2 = context.ParameterTypes.Add(new ParameterType { Name = "Aşama", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Parameter { Text = "Başvuru Formu", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Parameter { Text = "Teklif Formu", Created = DateTime.Now });
                var dysParameter = context.ParameterTypes.Add(new ParameterType { Name = "Dys Form Kategorileri" }).Entity;
                dysParameter.Parameters.Add(new Parameter { Text = "Esya", Created = DateTime.Now, DmsReferenceId = 1565,DmsReferenceKey=1210 });
                dysParameter.Parameters.Add(new Parameter { Text = "Konut", Created = DateTime.Now, DmsReferenceId = 1565, DmsReferenceKey = 1210 });
                dysParameter.Parameters.Add(new Parameter { Text = "DASK", Created = DateTime.Now, DmsReferenceId = 1565, DmsReferenceKey = 1210 });
                dysParameter.Parameters.Add(new Parameter { Text = "Kasko", Created = DateTime.Now, DmsReferenceId = 1568, DmsReferenceKey = 1220 });
                dysParameter.Parameters.Add(new Parameter { Text = "Trafik", Created = DateTime.Now, DmsReferenceId = 1568, DmsReferenceKey = 1220 });
                dysParameter.Parameters.Add(new Parameter { Text = "Isyeri", Created = DateTime.Now, DmsReferenceId = 1565, DmsReferenceKey = 1210 });
                dysParameter.Parameters.Add(new Parameter { Text = "Diger", Created = DateTime.Now, DmsReferenceId = 1572, DmsReferenceKey = 1200 });

                await context.SaveChangesAsync();
            }
        }

    }
}
