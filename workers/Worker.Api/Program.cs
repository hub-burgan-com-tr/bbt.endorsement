using Application;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.ZeebeServices;
using Worker.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<EventHub>("/eventhub");

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var zeebeService = serviceProvider.GetRequiredService<IZeebeService>();
    if (zeebeService != null)
    {
        zeebeService.Deploy("ContractApproval.bpmn");
        zeebeService.StartWorkers("https://localhost:7130/eventhub");

        var contractApprovalService = serviceProvider.GetRequiredService<ContractApprovalService>();
        if (contractApprovalService != null)
            contractApprovalService.StartWorkers();
    }
}

app.Run();
