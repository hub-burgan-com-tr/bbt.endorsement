using System.Reflection;
using System.Text.Json.Serialization;
using Application;
using Infrastructure;
using Infrastructure.Configuration;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers()
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Contract Approval API",
        Description = "Müşterilerin onaylaması gereken sözleşmeler için onaylatma altyapısı sunar.",
        Contact = new OpenApiContact
        {
            Name = "Contract Approval API",
            Url = new Uri("http://168.119.122.177:9090/my-approval")
        },
       
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    //options.SwaggerDoc("v1", new OpenApiInfo
    //{
    //    Version = "v1",
    //    Title = "Contract Approval API",
    //    Description = "Müşterilerin oanylaması gereken sözleşmeler için onaylatma altyapısı sunar."
    //});
        Description = "M��terilerin oanylamas� gereken s�zle�meler i�in onaylatma altyap�s� sunar."
    });

    options.CustomSchemaIds(x => x.FullName);

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.ConfigureSwagger();
    app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "bbt.endorsement.api v1");
                    c.RoutePrefix = "";
                });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
