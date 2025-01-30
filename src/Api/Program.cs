using System.Reflection;
using System.Text.Json.Serialization;
using Api.Extensions;
using Application;
using Application.Common.Models;
using Elastic.Apm.NetCoreAll;
using Elastic.Apm.SerilogEnricher;
using Elastic.CommonSchema.Serilog;
using Infrastructure;
using Infrastructure.Cache;
using Infrastructure.Configuration;
using Infrastructure.Configuration.Options;
using Infrastructure.Kafka;
using Infrastructure.Kafka.SettingModel;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

/// <summary>
///  
/// </summary>
IConfiguration Configuration;
var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment Environment = builder.Environment;
Log.Information("Endorsement API running - " + Environment.EnvironmentName);

if (Environment.EnvironmentName == "Development")
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
else if (Environment.EnvironmentName == "Prod")
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
else if (Environment.EnvironmentName == "Uat")
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
else if (Environment.EnvironmentName == "Test")
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
else if (Environment.EnvironmentName == "Preprod")
    Configuration = builder
        .Configuration
        .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();
else
    Configuration = builder
        .Configuration
        .AddJsonFile("appsettings.json", false, true)
        .AddEnvironmentVariables()
        .AddCommandLine(args)
        .AddUserSecrets<Program>()
        .Build();

Log.Logger = new LoggerConfiguration()
   .ReadFrom.Configuration(Configuration)
   .CreateLogger();

//Serilog.Debugging.SelfLog.Enable(msg =>
//{
//    Debug.Print(msg);
//    Debugger.Break();
//});

//builder.Logging.ClearProviders();
//builder.Logging.AddSerilog(Log.Logger);
//builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(Configuration)));

builder.Host.UseSerilog((context, services, configuration) => configuration
                    //.MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", Serilog.Events.LogEventLevel.Warning)
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services)
                    .Enrich.WithElasticApmCorrelationInfo()
                    .Enrich.FromLogContext().Enrich.WithEnvironmentName().Enrich.WithMachineName().Enrich.WithProcessId().Enrich.WithThreadId()
                    .WriteTo.Async(e =>
                    {
                        e.Elasticsearch(new ElasticsearchSinkOptions(new Uri(builder.Configuration["ElasticSearchSettings:Uri"]))
                        {
                            ModifyConnectionSettings = x => x.BasicAuthentication(builder.Configuration["ElasticSearchSettings:Username"], builder.Configuration["ElasticSearchSettings:Password"]),
                            AutoRegisterTemplate = true,
                            OverwriteTemplate = true,
                            IndexFormat = builder.Configuration["ElasticSearchSettings:IndexFormat"],
                            MinimumLogEventLevel = LogEventLevel.Information,
                            TypeName = null,
                            BatchPostingLimit = 1,
                            CustomFormatter = new EcsTextFormatter(),

                        });
                        e.Console(outputTemplate: "{Level}: [{ElasticApmTraceId} {ElasticApmTransactionId} {Message:lj:maxlength=10000} {NewLine}{Exception}");
                    }));
builder.Services.Configure<FormOptions>(x =>
{
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = int.MaxValue;
    x.MultipartHeadersLengthLimit = int.MaxValue;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

//if (!Environment.IsProduction())

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1.5",
        Title = "Endorsement API",
        Description = "Provides validation infrastructure for contracts that customers need to approve."
    });


    // To Enable authorization using Swagger (JWT)  
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        In = ParameterLocation.Header,
        Description = "Specify token with Bearer tag. example: Bearer {access_token}",
        BearerFormat = "JWT",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        // Scheme = "Bearer",
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                        {
                              new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new string[] {}

                        }
                });


    // options.SchemaFilter<EnumSchemaFilter>();
    options.UseInlineDefinitionsForEnums();

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
    options.CustomSchemaIds(x => x.FullName);
    options.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
});

var settings = builder.Configuration.Get<AppSettings>();
builder.Services.Configure<AppSettings>(options => Configuration.GetSection(nameof(AppSettings)).Bind(options));builder.Services.Configure<ContractDocumentCreatedSettings>(Configuration.GetSection(nameof(ContractDocumentCreatedSettings)));
builder.Services.Configure<ContractApprovedSettings>(Configuration.GetSection(nameof(ContractApprovedSettings)));

StaticValuesExtensions.SetStaticValues(settings);



builder.Services.AddHostedService<ContractDocumentCreatedWorker>();
builder.Services.AddHostedService<ContractApprovedWorker>();

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
//{
//    option.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateAudience = false, // Olu�turulacak token de�erini kimlerin/hangi originlerin/sitelerin kullanaca��n� belirledi�imiz aland�r.
//        ValidateIssuer = false, // Olu�turulacak token de�erini kimin da��tt���n� ifade edece�imiz aland�r.
//        ValidateLifetime = true, // Olu�turulan token de�erinin s�resini kontrol edecek olan do�rulamad�r.
//        ValidateIssuerSigningKey = true, // �retilecek token de�erinin uygulamam�za ait bir de�er oldu�unu ifade eden security key verisinin do�rulamas�d�r.
//        ValidIssuer = StaticValues.Issuer,
//        ValidAudience = StaticValues.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticValues.SecurityKey)),
//        ClockSkew = TimeSpan.Zero // �retilecek token de�erinin expire s�resinin belirtildi�i de�er kadar uzat�lmas�n� sa�layan �zelliktir. 
//    };
//});

// builder.Services.AddAuthentication(options =>
// {
//     // options.DefaultAuthenticateScheme = OAuthIntrospectionDefaults.AuthenticationScheme;
// }).AddOAuthIntrospection(options =>
// {
//     options.Authority = new Uri(StaticValues.Authority);
//     options.Audiences.Add(StaticValues.ClientId);
//     options.ClientId = StaticValues.ClientId;
//     options.ClientSecret = StaticValues.ClientSecret;
//     options.RequireHttpsMetadata = Environment.IsProduction();
// });

// Log.Information("StaticValues: " + StaticValues.Authority + " - " + StaticValues.ClientId + " - " + StaticValues.ClientSecret);

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicies();
//});

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder);

//builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
//builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


var app = builder.Build();

if (Environment.EnvironmentName == "Prod" || Environment.EnvironmentName == "Uat")
    app.UseAllElasticApm(Configuration);

app.UseSerilogRequestLogging();
app.AddUseMiddleware();
app.UseSession();


// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName == "Test")
{
    //app.ConfigureSwagger();
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
app.UseCors("corsapp");
app.UseHttpsRedirection();
app.UseRouting();

// app.UseAuthentication();
// app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();