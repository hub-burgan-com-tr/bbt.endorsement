using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Workers.Commands.SaveEntities;
using Worker.App.Models;
using Zeebe.Client.Api.Worker;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Worker.App.Services;

public interface IContractApprovalService
{
    void StartWorkers();
}

public class ContractApprovalService : IContractApprovalService
{
    private IZeebeService _zeebeService;
    private static readonly string WorkerName = Environment.MachineName;
    private IServiceProvider _provider = null!;
    private ISender _mediator = null!;

    public ContractApprovalService(IZeebeService zeebeService, IServiceProvider provider)
    {
        _zeebeService = zeebeService;
        _provider = provider;
        _mediator = _provider.CreateScope().ServiceProvider.GetRequiredService<ISender>();
    }

    public void StartWorkers()
    {
        ApproveContract();
        ConsumeCallback();
        LoadContactInfo();
        SaveEntity();
        SendOtp();
        SendPush();
        UpdateEntity();
        DeleteEntity();
        ErrorHandler();
    }

    private void ErrorHandler()
    {
        Log.Information("ErrorHandler Worker registered");

        CreateWorker("ErrorHandler", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            Log.Information($"ErrorEntity data: '{variables.InstanceId}'");

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables("{\"Approve\":\"" + false + "\"}")
                .Send();
        });
    }

    private void SaveEntity()
    {
        Log.Information("SaveEntity Worker registered ");

        CreateWorker("SaveEntity", async (jobClient, job) =>
        {
            Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<Dictionary<string, object>>(job.CustomHeaders);
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);

            // var state = customHeaders["State"].ToString();

            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            Log.ForContext("OrderId", variables.InstanceId).Information($"SaveEntity");

            if (variables != null)
            {
                var _mediator = _provider.CreateScope().ServiceProvider.GetRequiredService<ISender>();
                var response = await _mediator.Send(new SaveEntityCommand
                {
                    Model = variables
                });
                variables.IsProcess = true;
            }
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }


    private void LoadContactInfo()
    {
        Log.Information("LoadContactInfo Worker registered ");

        CreateWorker("LoadContactInfo", async (jobClient, job) =>
        {

            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            if (variables != null)
                variables.RetryEnd = true;
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            Log.Information($"LoadContactInfo data: '{variables.InstanceId}'");
            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }
    private void SendOtp()
    {
        Log.Information("SendOtp Worker registered ");

        CreateWorker("SendOtp", async (jobClient, job) =>
        {
            try
            {
                var contractApprovalData = new ContractModel();
                //var customer = contractApprovalData.Request.Customer;

                var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);

                if (variables != null)
                    variables.Limit += 1;
                variables.IsProcess = true;

                string data = System.Text.Json.JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                Log.Information($"SendOtp data: '{variables.InstanceId}'");

                await jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data)
                    .Send();
            }
            catch (Exception ex)
            {
                await jobClient.NewThrowErrorCommand(job.Key).ErrorCode("500").ErrorMessage(ex.Message).Send();
            }
        });
    }

    private void SendPush()
    {
        Log.Information("SendPush Worker registered ");

        CreateWorker("SendPush", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);

            try
            {
                throw new Exception("Hata");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }


            if (variables != null)
                variables.Limit += 1;
            variables.IsProcess = true;

            string data = System.Text.Json.JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            //  _logger.LogInformation($"SendPush data: '{variables.InstanceId}'");

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }

    private void UpdateEntity()
    {
        Log.Information("UpdateEntity Worker registered ");

        CreateWorker("UpdateEntity", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            if (variables != null)
            {
                variables.Completed = false;
                string data = System.Text.Json.JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                Log.Information($"UpdateEntity data: '{variables.InstanceId}'");

                await jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data) // "{\"IsTimeOut\":\"" + true + "\"}"
                    .Send();
            }
        });
    }

    private void ApproveContract()
    {
        Log.Information("ApproveContract Worker registered ");

        CreateWorker("ApproveContract", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            if (variables != null)
            {
                variables.Approved = true;
                variables.Completed = true;
                variables.IsProcess = true;
            }
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            string[] parameters = { "", "" };
            Log.ForContext("OrderId", variables.InstanceId).Information($"ApproveContract data: '{variables.InstanceId}'");

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();

        });
    }
    private void DeleteEntity()
    {
        Log.Information("DeleteEntity Worker registered ");

        CreateWorker("DeleteEntity", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            Log.Information($"DeleteEntity data: '{variables.InstanceId}'");
            await jobClient.NewCompleteJobCommand(job.Key)
                      .Variables("{\"Approve\":\"" + true + "\"}")
                      .Send();

        });
    }

    private void ConsumeCallback()
    {
        Log.Information("ConsumeCallback Worker registered ");

        CreateWorker("ConsumeCallback", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            Log.Information($"ConsumeCallback data: '{variables.InstanceId}'");
            await jobClient.NewCompleteJobCommand(job.Key)
                      .Variables("{\"Approve\":\"" + true + "\"}")
                      .Send();

        });
    }


    private void CreateWorker(String jobType, JobHandler handleJob)
    {
        _zeebeService.Client().NewWorker()
               .JobType(jobType)
               .Handler(handleJob)
               .MaxJobsActive(5)
               .Name(WorkerName)
               .AutoCompletion()
               .PollInterval(TimeSpan.FromSeconds(50))
               .PollingTimeout(TimeSpan.FromSeconds(50))
               .Timeout(TimeSpan.FromSeconds(10))
               .Open();
    }
}