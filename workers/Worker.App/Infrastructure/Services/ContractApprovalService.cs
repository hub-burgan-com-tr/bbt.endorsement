using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;
using System.Text.Json;
using System.Text.Json.Serialization;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Worker.App.Application.Documents.Commands.CreateDMSDocuments;
using Worker.App.Application.Documents.Commands.TSIZL;
using Worker.App.Application.Messagings.Commands.PersonSendMailTemplates;
using Worker.App.Application.Messagings.Commands.SendMailTemplates;
using Worker.App.Application.Messagings.Commands.SendSmsTemplates;
using Worker.App.Application.Orders.Commands.UpdateOrderGroups;
using Worker.App.Application.Orders.Queries.GetOrderDocuments;
using Worker.App.Application.Workers.Commands.ApproveContracts;
using Worker.App.Application.Workers.Commands.ConsumeCallback;
using Worker.App.Application.Workers.Commands.DeleteEntities;
using Worker.App.Application.Workers.Commands.LoadContactInfos;
using Worker.App.Application.Workers.Commands.SaveEntities;
using Worker.App.Application.Workers.Commands.UpdateEntities;
using Worker.App.Application.Workers.Queries.GetOrderConfigs;
using Worker.App.Application.Workers.Queries.GetOrderStates;
using Worker.App.Models;
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

        SaveEntity();
        SaveHistory();

        LoadContactInfo();
        SendOtp();
        SendPush();

        UpdateEntity();
        DeleteEntity();
        ErrorHandler();

        PersonalSendMail();
        CheckAllApproved();
        CreateDMSDocument();
        CreateTSIZL();
    }

    private void SaveEntity()
    {
        // Log.Information("SaveEntity Worker registered ");

        CreateWorker("SaveEntity", async (jobClient, job) =>
        {
            Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<Dictionary<string, object>>(job.CustomHeaders);
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("SaveEntity");
            try
            {
                // var state = customHeaders["State"].ToString();

                //Log.ForContext("OrderId", variables.InstanceId).Information($"SaveEntity");
                //Log.ForContext("OrderId", variables.InstanceId).Information("Internals: " + StaticValues.Internals);
                //Log.ForContext("OrderId", variables.InstanceId).Information("Sso: " + StaticValues.Sso);
                //Log.ForContext("OrderId", variables.InstanceId).Information("DMSService: " + StaticValues.DMSService);
                //Log.ForContext("OrderId", variables.InstanceId).Information("TemplateEngine: " + StaticValues.TemplateEngine);
                //Log.ForContext("OrderId", variables.InstanceId).Information("MessagingGateway: " + StaticValues.MessagingGateway);
                //variables.Urls = new string[] 
                //                { 
                //                    "Internals: " + StaticValues.Internals,
                //                    "Sso: " + StaticValues.Sso,
                //                    "DMSService: " + StaticValues.DMSService,
                //                    "TemplateEngine: " + StaticValues.TemplateEngine,
                //                    "MessagingGateway: " + StaticValues.MessagingGateway
                //                };

                if (variables != null)
                {
                    var response = _mediator.Send(new SaveEntityCommand
                    {
                        Model = variables,
                        ProcessInstanceKey = job.ProcessInstanceKey
                    }).Result;


                    var data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                    Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"SaveEntity");

                    var success = jobClient.NewCompleteJobCommand(job.Key)
                        .Variables(data)
                        .Send();
                }
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
                string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                var success = jobClient.NewCompleteJobCommand(job.Key)
                    .Variables(data)
                    .Send();
            }
        });
    }


    private void SaveHistory()
    {
        // Log.Information("SaveHistory Worker registered ");

        CreateWorker("SaveHistory", async (jobClient, job) =>
        {
            Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<Dictionary<string, object>>(job.CustomHeaders);
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("SaveHistory");
            try
            {
                // var state = customHeaders["State"].ToString();

                Log.ForContext("OrderId", variables.InstanceId).Information($"SaveHistory");

                if (variables != null)
                {
                    if (variables.IsProcess == true)
                    {
                        _mediator.Send(new CreateOrderHistoryCommand
                        {
                            OrderId = variables.InstanceId.ToString(),
                            State = "Yeni Onay Emri Oluşturuldu",
                            Description = "",
                            IsStaff = true
                        });

                        var response = await _mediator.Send(new GetOrderDocumentQuery { OrderId = variables.InstanceId.ToString() });
                        foreach (var document in response.Data)
                        {
                            _mediator.Send(new CreateOrderHistoryCommand
                            {
                                OrderId = variables.InstanceId,
                                State = "Onay Belgesi Geldi",
                                Description = document.Name,
                                DocumentId = document.DocumentId,
                                IsStaff = true
                            });
                        }
                    }
                    var data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                    Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"SaveHistory");

                    var success = jobClient.NewCompleteJobCommand(job.Key)
                        .Variables(data)
                        .Send();
                }
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                variables.IsProcess = false;
                variables.Error = ex.Message;
                string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

                var success = jobClient.NewCompleteJobCommand(job.Key)
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
            variables.Services.Add("LoadContactInfo");
            if (variables != null)
            {
                variables.RetryEnd = true;
                var orderConfig = _mediator.Send(new GetOrderConfigCommand { OrderId = variables.InstanceId }).Result;
                if (orderConfig != null)
                {
                    variables.Device = orderConfig.Data.Device;
                    variables.NotPersonalMail = orderConfig.Data.NotPersonalMail;
                    variables.NoNotification = orderConfig.Data.NoNotification;
                }
            }
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"LoadContactInfo");

            try
            {
                Log.ForContext("OrderId", variables.InstanceId).Information($"LoadContactInfo");

                //variables.Device = false;
                variables.IsProcess = true;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                //variables.IsProcess = false;
                variables.Error = ex.Message;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            var success = jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }
    private void SendOtp()
    {
        // Log.Information("SendOtp Worker registered ");

        CreateWorker("SendOtp", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("SendOtp");
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            try
            {
                var contractApprovalData = new ContractModel();

                if (variables != null)
                    variables.Limit += 1;
                variables.IsProcess = true;

                Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"LoadContactInfo");

                var person = new Response<LoadContactInfoResponse>();
                try
                {
                    person = await _mediator.Send(new LoadContactInfoCommand { InstanceId = variables.InstanceId, EmailSendType = EmailSendType.Customer });

                }
                catch (Exception ex)
                {
                    Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);

                }
                if (person.Data != null)
                {
                    //foreach (var gsmPhone in Users.GsmPhones())
                    // {


                    try
                    {
                        var responseSms = _mediator.Send(new SendSmsTemplateCommand
                        {
                            OrderId = variables.InstanceId,
                            GsmPhone = person.Data.Customer.GsmPhone, // gsmPhone
                            CustomerNumber = person.Data.Customer.CustomerNumber,
                        }).Result;
                        var orderHistoryCommand = new CreateOrderHistoryCommand
                        {
                            OrderId = variables.InstanceId.ToString(),
                            State = "Hatırlatma Mesajı(Sms)",
                            Description = "",
                            IsStaff = false,
                        };
                        if (responseSms.Data != null)
                        {
                            orderHistoryCommand.Request = responseSms.Data.Request;
                            orderHistoryCommand.Response = responseSms.Data.Response;
                            orderHistoryCommand.CustomerId = responseSms.Data.CustomerId;
                        }
                        _mediator.Send(orderHistoryCommand);
                    }
                    catch (Exception ex)
                    {
                        Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                    }


                    try
                    {
                        var responseMail = _mediator.Send(new SendMailTemplateCommand
                        {
                            OrderId = variables.InstanceId,
                            Email = person.Data.Customer.Email,  // email,
                        }).Result;
                        var emailHistory = new CreateOrderHistoryCommand
                        {
                            OrderId = variables.InstanceId.ToString(),
                            State = "Hatırlatma Mesajı(Mail)",
                            Description = "",
                            IsStaff = false,
                        };

                        if (responseMail.Data != null)
                        {
                            emailHistory.Request = responseMail.Data.Request;
                            emailHistory.Response = responseMail.Data.Response;
                            emailHistory.CustomerId = responseMail.Data.CustomerId;
                        }
                        _mediator.Send(emailHistory);
                    }
                    catch (Exception ex)
                    {
                        Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                    }

                    _mediator.Send(new CreateOrderHistoryCommand
                    {
                        OrderId = variables.InstanceId.ToString(),
                        State = "Hatırlatma Mesajı",
                        Description = "",
                        IsStaff = true
                    });

                }
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                // variables.IsProcess = false;
                variables.Error = ex.Message;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                // await jobClient.NewThrowErrorCommand(job.Key).ErrorCode("500").ErrorMessage(ex.Message).Send();
            }
             

            var success = jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }

    private void SendPush()
    {
        //Log.Information("SendPush Worker registered ");

        CreateWorker("SendPush", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("SendPush");

            if (variables != null)
                variables.Limit += 1;

            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            try
            {
                Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"SendPush");

                _mediator.Send(new CreateOrderHistoryCommand
                {
                    OrderId = variables.InstanceId.ToString(),
                    State = "Hatırlatma Mesajı (Push Notification)",
                    Description = "",
                    IsStaff = true
                });
                variables.IsProcess = true;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                //variables.IsProcess = false;
                variables.Error = ex.Message;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }

            var success = jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }
    private void CreateTSIZL()
    {

        CreateWorker("CreateTSIZL", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("CreateTSIZL");



            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            try
            {
                Log.ForContext("OrderId", variables.InstanceId).Information($"CreateTSIZL");
                var command = _mediator.Send(new TSIZLCommand
                {
                    InstanceId = variables.InstanceId
                }).Result;

                //_mediator.Send(new CreateOrderHistoryCommand
                //{
                //    OrderId = variables.InstanceId.ToString(),
                //    State = "CreateTSIZL Calistirildi",
                //    Description = command.ToString(),
                //    IsStaff = true
                //});

                variables.IsProcess = true;
                Log.ForContext("OrderId", variables.InstanceId)
                .ForContext("TSIZLReferenceNumber", command.Data.ReferenceNumber)
                .ForContext("TSIZLHasError", command.Data.HasError.ToString())
                .ForContext("TSIZLErrorMessage", command.Data.ErrorMessage)
                .ForContext("OrderId", variables.InstanceId)
                .ForContext("variable", data)
                .Information($"CreateTSIZL");
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                //variables.IsProcess = false;
                variables.Error = ex.Message;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }

            var success = jobClient.NewCompleteJobCommand(job.Key)
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
            variables.Services.Add("UpdateEntity");
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            if (variables != null)
            {
                variables.Completed = false;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"UpdateEntity");

                var response = await _mediator.Send(new UpdateEntityCommand { OrderId = variables.InstanceId });
                if (response.StatusCode == 200)
                {
                    _mediator.Send(new CreateOrderHistoryCommand
                    {
                        OrderId = variables.InstanceId,
                        State = "Yeni Onay Emri Zaman Aşımına Uğradı",
                        Description = "",
                        IsStaff = true
                    });
                }
            }

            var success = jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }

    private void ApproveContract()
    {
        //Log.Information("ApproveContract Worker registered ");

        CreateWorker("ApproveContract", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("ApproveContract");
            if (variables != null)
            {
                var orderConfig = _mediator.Send(new GetOrderConfigCommand { OrderId = variables.InstanceId }).Result;
                if (orderConfig != null)
                {
                    variables.RetryFrequence = orderConfig.Data.RetryFrequence;
                    variables.ExpireInMinutes = orderConfig.Data.ExpireInMinutes;
                    variables.MaxRetryCount = orderConfig.Data.MaxRetryCount;
                    variables.Device = orderConfig.Data.Device;
                    variables.NotPersonalMail = orderConfig.Data.NotPersonalMail;
                    variables.NoNotification = orderConfig.Data.NoNotification;
                }
                variables.IsProcess = true;
                variables.Completed = false;
            }
            string data = "";


            try
            {
                // foreach (var item in variables.Documents)
                //{
                //    var document = _mediator.Send(new UpdateDocumentActionCommand
                //    {
                //        OrderId = variables.InstanceId,
                //        DocumentId = item.DocumentId,
                //        ActionId = item.ActionId
                //    });
                //}

                var orderState = await _mediator.Send(new ApproveContractCommand { Model = variables, OrderId = variables.InstanceId });
                if (orderState.Data.OrderState != OrderState.Pending)
                {
                    foreach (var item in orderState.Data.Documents)
                    {
                        _mediator.Send(new CreateOrderHistoryCommand
                        {
                            OrderId = variables.InstanceId,
                            DocumentId = item.DocumentId,
                            State = item.ActionTitle,
                            Description = item.DocumentName,
                            IsStaff = true
                        });
                    }
                }
                Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"ApproveContract");

                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                // variables.IsProcess = false;
                variables.Error = ex.Message;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }

            var success = jobClient.NewCompleteJobCommand(job.Key)
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
            variables.Services.Add("DeleteEntity");
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
             

            try
            {
                var response = await _mediator.Send(new DeleteEntityCommand { OrderId = variables.InstanceId.ToString() });

                if (response != null && response.Data.OrderState == OrderState.Cancel && response.Data.IsUpdated)
                {
                    _mediator.Send(new CreateOrderHistoryCommand
                    {
                        OrderId = variables.InstanceId.ToString(),
                        State = "Emir iptal edildi",
                        Description = "",
                        IsStaff = true
                    });
                }
                variables.IsProcess = true;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                // variables.IsProcess = false;
                variables.Error = ex.Message;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"DeleteEntity");

            var success = jobClient.NewCompleteJobCommand(job.Key)
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
            variables.Services.Add("ConsumeCallback");
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            var callback = await _mediator.Send(new ConsumeCallbackCommand
            {
                OrderId = variables.InstanceId.ToString(),

            });
            Log.ForContext("variables", data).ForContext("CalbackResponse", JsonSerializer.Serialize(callback, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } })).Information($"ConsumeCallback");

            _mediator.Send(new CreateOrderHistoryCommand
            {
                OrderId = variables.InstanceId.ToString(),
                State = "Consume Callback status = " + callback.StatusCode,
                Description = callback.Data.Content
            });

     
            var success = jobClient.NewCompleteJobCommand(job.Key)
                      .Variables("{\"Approve\":\"" + true + "\"}")
                      .Send();

        });
    }


    private void CheckAllApproved()
    {
        CreateWorker("CheckAllApproved", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("CheckAllApproved");
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });


            try
            {

                variables.IsProcess = true;
                var response = await _mediator.Send(new GetOrderStateCommand { OrderId = variables.InstanceId });

                if (response.Data.OrderState == OrderState.Reject || response.Data.OrderState == OrderState.Approve)
                {
                    _mediator.Send(new CreateOrderHistoryCommand
                    {
                        OrderId = variables.InstanceId,
                        State = "Workflow tamamlandı",
                        Description = "",
                        IsStaff = true
                    });
                    variables.IsProcess = true;
                    variables.Approved = true;
                    variables.Completed = true;

                    if (response.Data.OrderState == OrderState.Approve)
                    {
                        await _mediator.Send(new UpdateOrderGroupCommand
                        {
                            OrderId = variables.InstanceId
                        });
                    }
                }
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", variables.InstanceId).Error(ex, ex.Message);
                //variables.IsProcess = false;
                variables.Error = ex.Message;
                data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            }
            Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"CheckAllApproved");

            var success = jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }


    private void PersonalSendMail()
    {
        CreateWorker("PersonalSendMail", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("PersonalSendMail");

            if (variables != null)
                variables.Limit += 1;
            variables.IsProcess = true;

            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });

            var instanceId = variables.InstanceId;
            try
            {
                // var person = await _mediator.Send(new LoadContactInfoPersonCommand { InstanceId = instanceId });

                var person = await _mediator.Send(new LoadContactInfoCommand { InstanceId = variables.InstanceId, EmailSendType = EmailSendType.Person });
                if (person.Data != null)
                {
                    var responseEmail = await _mediator.Send(new PersonSendMailTemplateCommand
                    {
                        OrderId = instanceId,
                        Email = person.Data.Customer.BusinessEmail
                    });

                    var createOrderHistoryCommand = new CreateOrderHistoryCommand
                    {
                        OrderId = instanceId,
                        State = "PY Hatırlatma Mesajı(Email)",
                        Description = "",
                        IsStaff = false
                    };
                    if (responseEmail.Data != null)
                    {
                        createOrderHistoryCommand.Request = responseEmail.Data.Request;
                        createOrderHistoryCommand.Response = responseEmail.Data.Response;
                        createOrderHistoryCommand.PersonId = responseEmail.Data.PersonId;
                    }

                    _mediator.Send(createOrderHistoryCommand);

                    data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                }
                else
                {
                    Log.ForContext("OrderId", instanceId).Information(person.Message);
                }
            }
            catch (Exception ex)
            {
                Log.ForContext("OrderId", instanceId).Error(ex, ex.Message);
            }
            Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"PersonalSendMail");

            var success = jobClient.NewCompleteJobCommand(job.Key)
                .Variables(data)
                .Send();
        });
    }

    private async Task<bool> SendPersonalMail(string instanceId)
    {
        try
        {
            var person = await _mediator.Send(new LoadContactInfoPersonCommand { InstanceId = instanceId });

            if (person.Data != null)
            {
                var responseEmail = _mediator.Send(new PersonSendMailTemplateCommand
                {
                    OrderId = instanceId,
                    Email = person.Data.Customer.BusinessEmail
                }).Result;

                var createOrderHistoryCommand = new CreateOrderHistoryCommand
                {
                    OrderId = instanceId,
                    State = "PY Hatırlatma Mesajı(Email)",
                    Description = "",
                    IsStaff = false
                };
                if (responseEmail.Data != null)
                {
                    createOrderHistoryCommand.Request = responseEmail.Data.Request;
                    createOrderHistoryCommand.Response = responseEmail.Data.Response;
                    createOrderHistoryCommand.PersonId = responseEmail.Data.PersonId;
                }

                _mediator.Send(createOrderHistoryCommand);
            }
            else
            {
                Log.ForContext("OrderId", instanceId).Information(person.Message);
            }
        }
        catch (Exception ex)
        {
            Log.ForContext("OrderId", instanceId).Error(ex, ex.Message);
        }
        return true;
    }

    private void CreateDMSDocument()
    {
        // Log.Information("SaveEntity Worker registered ");

        CreateWorker("CreateDMSDocument", async (jobClient, job) =>
        {
            Dictionary<string, object> customHeaders = JsonSerializer.Deserialize<Dictionary<string, object>>(job.CustomHeaders);
            Dictionary<string, object> _variables = JsonSerializer.Deserialize<Dictionary<string, object>>(job.Variables);
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("CreateDMSDocument");
            variables.DmsRetryLimit += 1;


            if (variables != null)
            {
                try
                {
                    var dms = _mediator.Send(new CreateDMSDocumentCommand
                    {
                        InstanceId = variables.InstanceId
                    }).Result;

                    variables.IsProcess = true;
                    variables.DmsIds = dms.Data.Select(x => new DMSDocumentResponse { DmsReferenceKey = x.DmsReferenceKey, DmsReferenceName = x.DmsReferenceName, DmsRefId = x.DmsRefId }).ToList();
                    //throw new Exception();
                    var tsizAny  = _mediator.Send(new TSIZLAnyCommand
                    {
                        InstanceId = variables.InstanceId
                    }).Result;
                    variables.IsTSIZL = tsizAny.Data;
                }
                catch (Exception ex)
                {
                    Log.ForContext("OrderId", variables.InstanceId)
                    .ForContext("DmsRetryLimit", variables.DmsRetryLimit).Error(ex, "CreateDMSDocumentException");

                    variables.Error = ex.Message;
                    variables.IsProcess = false;
                }

                var data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
                Log.ForContext("OrderId", variables.InstanceId).ForContext("variable", data).Information($"CreateDMSDocument");

                var success = jobClient.NewCompleteJobCommand(job.Key)
                   .Variables(data)
                   .Send();
            }

        });
    }


    private void ErrorHandler()
    {
        //Log.Information("ErrorHandler Worker registered");

        CreateWorker("ErrorHandler", async (jobClient, job) =>
        {
            var variables = JsonConvert.DeserializeObject<ContractModel>(job.Variables);
            variables.Services.Add("ErrorHandler");
            string data = JsonSerializer.Serialize(variables, new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } });
            Log.ForContext("variable",data).Error($"ErrorEntity data");

            var success = jobClient.NewCompleteJobCommand(job.Key)
                .Variables("{\"Approve\":\"" + false + "\"}")
                .Send();
        });
    }


    private void CreateWorker(String jobType, JobHandler handleJob)
    {
        _zeebeService.Client().NewWorker()
               .JobType(jobType)
               .Handler(handleJob)
               .MaxJobsActive(250)
               .Name(WorkerName)
               .AutoCompletion()
               .PollInterval(TimeSpan.FromSeconds(60))
               .PollingTimeout(TimeSpan.FromSeconds(60))
               .Timeout(TimeSpan.FromSeconds(60))
               .AutoCompletion()
               .Open();
    }
}