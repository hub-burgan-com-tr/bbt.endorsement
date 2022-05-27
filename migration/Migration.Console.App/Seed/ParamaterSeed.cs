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
               var parameterType= context.ParameterTypes.Add(new ParameterType { Name = "İşlem",Created=DateTime.Now});
                parameterType.Entity.Parameters.Add(new Parameter { Text = "Sigorta Formları", Created = DateTime.Now });
                var parameterType2 = context.ParameterTypes.Add(new ParameterType { Name = "Aşama", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Parameter {  Text = "Başvuru Formu", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Parameter {  Text = "Teklif Formu", Created = DateTime.Now });
                var dysParameter = context.ParameterTypes.Add(new ParameterType {Name = "Dys Form Kategorileri" }).Entity;
                dysParameter.Parameters.Add(new Parameter {  Text = "Konut Eşya", Created = DateTime.Now,DmsReferenceId=1 });
                dysParameter.Parameters.Add(new Parameter {  Text = "Konut", Created = DateTime.Now,DmsReferenceId=2 });
                dysParameter.Parameters.Add(new Parameter {  Text = "DASK", Created = DateTime.Now,DmsReferenceId=3 });
                dysParameter.Parameters.Add(new Parameter {  Text = "Kasko", Created = DateTime.Now,DmsReferenceId=4 });
                dysParameter.Parameters.Add(new Parameter {  Text = "Trafik", Created = DateTime.Now,DmsReferenceId=5 });
                dysParameter.Parameters.Add(new Parameter {  Text = "İşyeri", Created = DateTime.Now,DmsReferenceId=6 });
                dysParameter.Parameters.Add(new Parameter {  Text = "Diğer", Created = DateTime.Now,DmsReferenceId=7 });

                await context.SaveChangesAsync();
            }
        }

    }
}
