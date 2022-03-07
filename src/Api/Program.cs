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
        Description = "Provides validation infrastructure for contracts that customers need to approve.",
        Contact = new OpenApiContact
        {
            Name = "Contract Approval API",
            //Url = new Uri("http://168.119.122.177:9090/my-approval"),
            Email = ""
        },
        License = new OpenApiLicense
        {
            Name = "",
            //Url = new Uri("http://168.119.122.177:9090/my-approval"),
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
    options.CustomSchemaIds(x => x.FullName);
    options.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
});

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .AddUserSecrets<Program>()
    .Build();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder);

var app = builder.Build();
app.AddUseMiddleware();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
{
    app.ConfigureSwagger();
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
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