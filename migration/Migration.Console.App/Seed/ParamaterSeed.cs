using Domain.Entities;
using Infrastructure.Persistence;

namespace Migration.Console.App.Seed
{
    public static  class ParamaterSeed
    {
        public static async Task SeedFormDefinitionsAsync(ApplicationDbContext context)
        {
            if (context == null)
                return;
            if (!context.ParameterTypes.Any())
            {
               var parameterType= context.ParameterTypes.Add(new ParameterType { Id = 1, Name = "İşlem",Created=DateTime.Now});
                parameterType.Entity.Parameters.Add(new Parameter { Id = 1, Text = "Sigorta Formları", Created = DateTime.Now });

                var parameterType2 = context.ParameterTypes.Add(new ParameterType { Id = 2, Name = "Aşama", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Parameter { Id = 101, Text = "Başvuru Formu", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Parameter { Id = 102, Text = "Teklif Formu", Created = DateTime.Now });

                var dysParameter = context.ParameterTypes.Add(new ParameterType { Id = 3, Name = "Dys Form Kategorileri" }).Entity;
                dysParameter.Parameters.Add(new Parameter { Id = 201, Text = "Konut Eşya" });
                dysParameter.Parameters.Add(new Parameter { Id = 202, Text = "Konut" });
                dysParameter.Parameters.Add(new Parameter { Id = 203, Text = "DASK" });
                dysParameter.Parameters.Add(new Parameter { Id = 204, Text = "Kasko" });
                dysParameter.Parameters.Add(new Parameter { Id = 205, Text = "Trafik" });
                dysParameter.Parameters.Add(new Parameter { Id = 206, Text = "İşyeri" });
                dysParameter.Parameters.Add(new Parameter { Id = 207, Text = "Diğer" });

                await context.SaveChangesAsync();
            }
        }

    }
}
