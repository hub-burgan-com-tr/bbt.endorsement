
using Application;
using Infrastructure;
using Infrastructure.Services;
using Infrastructure.ZeebeServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Worker.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHostedService<ZeebeWorkService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var zeebeService = serviceProvider.GetRequiredService<IZeebeService>();
    if (zeebeService != null)
    {
        zeebeService.Deploy("ContractApproval.bpmn");
    }

    while (true)
    {
        // open job worker
        using (var signal = new EventWaitHandle(false, EventResetMode.AutoReset))
        {
            var contractApprovalService = serviceProvider.GetRequiredService<IContractApprovalService>();
            if (contractApprovalService != null)
                contractApprovalService.StartWorkers();

            // blocks main thread, so that worker can run
            signal.WaitOne();
        }
    }
}
