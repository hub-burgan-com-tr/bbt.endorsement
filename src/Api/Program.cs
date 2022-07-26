using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Api.Extensions;
using Api.Providers;
using Application;
using Application.Common.Models;
using AspNet.Security.OAuth.Introspection;
using Infrastructure;
using Infrastructure.Configuration;
using Infrastructure.Configuration.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

/// <summary>
///  
/// </summary>
IConfiguration Configuration;
var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment Environment = builder.Environment;

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
else if (Environment.EnvironmentName == "Test")
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
builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(Configuration)));

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

if (!Environment.IsProduction())
{
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
}


var settings = builder.Configuration.Get<AppSettings>();
builder.Services.Configure<AppSettings>(options => Configuration.GetSection(nameof(AppSettings)).Bind(options));
StaticValuesExtensions.SetStaticValues(settings);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
//{
//    option.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateAudience = false, // Oluþturulacak token deðerini kimlerin/hangi originlerin/sitelerin kullanacaðýný belirlediðimiz alandýr.
//        ValidateIssuer = false, // Oluþturulacak token deðerini kimin daðýttýðýný ifade edeceðimiz alandýr.
//        ValidateLifetime = true, // Oluþturulan token deðerinin süresini kontrol edecek olan doðrulamadýr.
//        ValidateIssuerSigningKey = true, // Üretilecek token deðerinin uygulamamýza ait bir deðer olduðunu ifade eden security key verisinin doðrulamasýdýr.
//        ValidIssuer = StaticValues.Issuer,
//        ValidAudience = StaticValues.Audience,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(StaticValues.SecurityKey)),
//        ClockSkew = TimeSpan.Zero // Üretilecek token deðerinin expire süresinin belirtildiði deðer kadar uzatýlmasýný saðlayan özelliktir. 
//    };
//});

builder.Services.AddAuthentication(options =>
{
    // options.DefaultAuthenticateScheme = OAuthIntrospectionDefaults.AuthenticationScheme;
}).AddOAuthIntrospection(options =>
{
    options.Authority = new Uri(StaticValues.Authority);
    options.Audiences.Add(StaticValues.ClientId);
    options.ClientId = StaticValues.ClientId;
    options.ClientSecret = StaticValues.ApiClientSecret;
    options.RequireHttpsMetadata = Environment.IsProduction();
});

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

app.UseSerilogRequestLogging();
app.AddUseMiddleware();
app.UseSession();

Log.Information("Endorsement API running... - " + Environment.EnvironmentName);

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
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

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();