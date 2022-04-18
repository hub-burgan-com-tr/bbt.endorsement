using Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
               var parameterType= context.ParameterTypes.Add(new Domain.Entities.ParameterType { Name = "İşlem",Created=DateTime.Now});
                parameterType.Entity.Parameters.Add(new Domain.Entities.Parameter { Text = "Sigorta Formları", Created = DateTime.Now });
                var parameterType2 = context.ParameterTypes.Add(new Domain.Entities.ParameterType { Name = "Aşama", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Domain.Entities.Parameter { Text = "Başvuru Formu", Created = DateTime.Now });
                parameterType2.Entity.Parameters.Add(new Domain.Entities.Parameter { Text = "Teklif Formu", Created = DateTime.Now });
                await context.SaveChangesAsync();
            }
        }

    }
}
