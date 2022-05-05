using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Documents.Commands.UpdateDocumentStates;
using Worker.App.Application.Workers.Commands.ApproveContracts;
using Worker.App.Application.Workers.Commands.DeleteEntities;
using Worker.App.Application.Workers.Commands.LoadContactInfos;
using Worker.App.Application.Workers.Commands.SaveEntities;
using Worker.App.Application.Workers.Commands.UpdateEntities;
using Worker.App.Application.Workers.Queries.GetOrderConfigs;
using Worker.App.Application.Workers.Queries.GetOrderStates;
using Worker.AppApplication.Documents.Commands.CreateOrderHistories;
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

        CustomerSendSms();
        CustomerSendMail();
        CheckAllApproved();
    }

    private void SaveEntity()
    {
       // Log.Information("SaveEntity Worker registered ");

        CreateWorker("SaveEntity", async (jobClient, job) =>
        {
            Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<Dictionary<string, object>>(job.CustomHeaders);
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            try
            {
                // var state = customHeaders["State"].ToString();
             
                Log.ForContext("OrderId", variables.OrderId).Information($"SaveEntity");

                if (variables != null)
                {
                    var response = await _mediator.Send(new SaveEntityCommand
                    {
                        Model = variables,
                        ProcessInstanceKey = job.ProcessInstanceKey
                    });
                    variables.IsProcess = true;
                    variables.Device = true;

                    if(response != null)
                    {
                        var history = _mediator.Send(new CreateOrderHistoryCommand
                        {
                            OrderId = variables.OrderId.ToString(),
                            State = "Yeni Onay Emri Oluşturuldu",
                            Description = ""
                        });

                        foreach (var document in response.Data.Documents)
                        {
                            await _mediator.Send(new CreateOrderHistoryCommand
                            {
                                OrderId = variables.OrderId,
                                State = "Onay Belgesi Geldi",
                                Description  = document.Name
                            });
                        }
                    }
                }
                string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                await jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data)
                    .Send();
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.OrderId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
                string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                
                await jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data)
                    .Send();
            }
        });
    }


    private void LoadContactInfo()
    {
        //Log.Information("LoadContactInfo Worker registered ");

        CreateWorker("LoadContactInfo", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            if (variables != null)
                variables.RetryEnd = true;
            try
            {
                string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                var person = await _mediator.Send(new LoadContactInfoCommand { InstanceId = variables.OrderId });
                if (person.Data.Person != null)
                {
                    if (person.Data.Person.Devices.Any())
                    {
                        variables.Device = true;
                        var device = person.Data.Person.Devices.FirstOrDefault();
                    }
                    else
                    {
                        variables.Device = false;
                        var gsmPhone = person.Data.Person.GsmPhones.FirstOrDefault();
                        var phone = gsmPhone.County.ToString() + gsmPhone.Prefix.ToString() + gsmPhone.Number.ToString();
                    }
                }

                Log.ForContext("OrderId", variables.OrderId).Information($"LoadContactInfo");

                //var history = _mediator.Send(new CreateOrderHistoryCommand
                //{
                //    OrderId = variables.InstanceId.ToString(),
                //    State = "Müşteri Bilgileri",
                //    Description = ""
                //});
                await jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data)
                    .Send();
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.OrderId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
                string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                await jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data)
                    .Send();
            }
        });
    }
    private void SendOtp()
    {
       // Log.Information("SendOtp Worker registered ");

        CreateWorker("SendOtp", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            try
            {
                var contractApprovalData = new ContractModel();
                //var customer = contractApprovalData.Request.Customer;


                if (variables != null)
                    variables.Limit += 1;
                variables.IsProcess = true;

                string data = System.Text.Json.JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                Log.ForContext("OrderId", variables.OrderId).Information($"SendOtp");

                var history = _mediator.Send(new CreateOrderHistoryCommand
                {
                    OrderId = variables.OrderId.ToString(),
                    State = "Hatırlatma Mesajı (Sms)",
                    Description = ""
                });
                await jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data)
                    .Send();
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.OrderId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
                await jobClient.NewThrowErrorCommand(job.Key).ErrorCode("500").ErrorMessage(ex.Message).Send();
            }
        });
    }

    private void SendPush()
    {
        //Log.Information("SendPush Worker registered ");

        CreateWorker("SendPush", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);

            if (variables != null)
                variables.Limit += 1;
            variables.IsProcess = true;

            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            try
            {
                Log.ForContext("OrderId", variables.OrderId).Information($"SendPush");

                var history = _mediator.Send(new CreateOrderHistoryCommand
                {
                    OrderId = variables.OrderId.ToString(),
                    State = "Hatırlatma Mesajı (Push Notification)",
                    Description = ""
                });
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.OrderId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
            }

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }

    private void UpdateEntity()
    {
        //Log.Information("UpdateEntity Worker registered ");

        CreateWorker("UpdateEntity", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            if (variables != null)
            {
                variables.Completed = false;
                string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                Log.ForContext("OrderId", variables.OrderId).Information($"UpdateEntity");

                var response = await _mediator.Send(new UpdateEntityCommand { OrderId = variables.OrderId });
                if (response != null && response.Data.IsUpdated)
                {
                    var history = _mediator.Send(new CreateOrderHistoryCommand
                    {
                        OrderId = variables.OrderId,
                        State = "Yeni Onay Emri Zaman Aşımına Uğradı",
                        Description = ""
                    });
                }

                await jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data)
                    .Send();
            }
        });
    }

    private void ApproveContract()
    {
        //Log.Information("ApproveContract Worker registered ");

        CreateWorker("ApproveContract", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            if (variables != null)
            {
                var orderConfig = _mediator.Send(new GetOrderConfigCommand { OrderId = variables.OrderId }).Result;
                if(orderConfig != null)
                {
                    variables.RetryFrequence = orderConfig.Data.RetryFrequence;
                    variables.ExpireInMinutes = orderConfig.Data.ExpireInMinutes;
                    variables.MaxRetryCount = orderConfig.Data.MaxRetryCount;
                }
                variables.IsProcess = true;
                variables.Completed = false;
            }
            string data = "";

            Log.ForContext("OrderId", variables.OrderId).Information($"ApproveContract");
            try
            {
                 foreach (var item in variables.Documents)
                {
                    var document = _mediator.Send(new UpdateDocumentActionCommand
                    {
                        OrderId = variables.OrderId,
                        DocumentId = item.DocumentId,
                        ActionId = item.ActionId
                    });
                }

                var orderState = await _mediator.Send(new ApproveContractCommand { OrderId = variables.OrderId });
                if(orderState.Data.OrderState != OrderState.Pending)
                {
                    foreach (var item in orderState.Data.Documents)
                    {
                        await _mediator.Send(new CreateOrderHistoryCommand
                        {
                            OrderId = variables.OrderId,
                            DocumentId = item.DocumentId,
                            State = item.ActionTitle,
                            Description = item.DocumentName
                        });
                    }
                }
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.OrderId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }
    private void DeleteEntity()
    {
        // Log.Information("DeleteEntity Worker registered ");

        CreateWorker("DeleteEntity", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            Log.ForContext("OrderId", variables.OrderId).Information($"DeleteEntity");

            var response = await _mediator.Send(new DeleteEntityCommand { OrderId = variables.OrderId.ToString() });

            if (response != null && response.Data.OrderState == OrderState.Cancel && response.Data.IsUpdated)
            {
                var history = _mediator.Send(new CreateOrderHistoryCommand
                {
                    OrderId = variables.OrderId.ToString(),
                    State = "Emir iptal edildi",
                    Description = ""
                });
            }

            await jobClient.NewCompleteJobCommand(job.Key)
                      .Variables(data)
                      .Send();

        });
    }

    private void ConsumeCallback()
    {
       // Log.Information("ConsumeCallback Worker registered ");

        CreateWorker("ConsumeCallback", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            Log.ForContext("OrderId", variables.OrderId).Information($"ConsumeCallback");

            //var history = _mediator.Send(new CreateOrderHistoryCommand
            //{
            //    OrderId = variables.InstanceId.ToString(),
            //    State = "Consume Callback",
            //    Description = ""
            //});

            await jobClient.NewCompleteJobCommand(job.Key)
                      .Variables("{\"Approve\":\"" + true + "\"}")
                      .Send();

        });
    }

    private void CheckAllApproved()
    {
        CreateWorker("CheckAllApproved", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            try
            {
                Log.ForContext("OrderId", variables.OrderId).Information($"CheckAllApproved");

                variables.IsProcess = true;
                var response = await _mediator.Send(new GetOrderStateCommand { OrderId = variables.OrderId });

                if (response.Data.OrderState == OrderState.Reject || response.Data.OrderState == OrderState.Approve)
                {
                    variables.Completed = true;
                    variables.Approved = true;
                    await _mediator.Send(new CreateOrderHistoryCommand
                    {
                        OrderId = variables.OrderId,
                        State = "Workflow tamamlandı",
                        Description = ""
                    });
                }

            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.OrderId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
            }

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }

    private void CustomerSendMail()
    {
        CreateWorker("CustomerSendMail", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);

            if (variables != null)
                variables.Limit += 1;
            variables.IsProcess = true;

            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            try
            {
                Log.ForContext("OrderId", variables.OrderId).Information($"CustomerSendMail");

            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.OrderId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
            }

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }

    private void CustomerSendSms()
    {
        CreateWorker("CustomerSendSms", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);

            if (variables != null)
                variables.Limit += 1;
            variables.IsProcess = true;

            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            try
            {
                Log.ForContext("OrderId", variables.OrderId).Information($"CustomerSendSms");

            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.OrderId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
            }

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }

    private void ErrorHandler()
    {
        //Log.Information("ErrorHandler Worker registered");

        CreateWorker("ErrorHandler", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            Log.Information($"ErrorEntity data");

            await jobClient.NewCompleteJobCommand(job.Key)
                .Variables("{\"Approve\":\"" + false + "\"}")
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