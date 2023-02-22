using MediatR;
using Worker.App.Application.Common.Models;
using Worker.App.Application.Common.Interfaces;
using Domain.Models;
using Domain.Entities;
using Domain.Enums;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Worker.App.Application.Workers.Commands.SaveEntities
{
    public class SaveEntityCommand : IRequest<Response<SaveEntityResponse>>
    {
        public ContractModel Model { get; set; }   
        public long ProcessInstanceKey { get; set; }
    }

    public class SaveEntityCommandHandler : IRequestHandler<SaveEntityCommand, Response<SaveEntityResponse>>
    {
        private IApplicationDbContext _context;
        private ISaveEntityService _saveEntityService;
        private IDateTime _dateTime;

        public SaveEntityCommandHandler(IApplicationDbContext context, ISaveEntityService saveEntityService, IDateTime dateTime)
        {
            _context = context;
            _saveEntityService = saveEntityService;
            _dateTime = dateTime;
        }
        public async Task<Response<SaveEntityResponse>> Handle(SaveEntityCommand request, CancellationToken cancellationToken)
        {
            var response = new SaveEntityResponse();
            try
            {
                if (request.Model.FormType == Form.Order)
                    response = OrderCreate(request.Model.StartRequest, request.Model.Person, request.ProcessInstanceKey);
                else if (request.Model.FormType == Form.FormOrder)
                    response = FormOrderCreate(request.Model.StartFormRequest, request.Model.Person, request.ProcessInstanceKey, request.Model.ContentData);
                //var documentList = _context.Documents.Where(x => x.OrderId == response.OrderId);
                //var saveEntityDocuments = new List<SaveEntityDocumentResponse>();
                //foreach (var item in documentList)
                //    saveEntityDocuments.Add(new SaveEntityDocumentResponse { DocumentId = item.DocumentId, Name = item.Name });

                //response.Documents = saveEntityDocuments;
            }
            catch (Exception ex)
            {
                return Response<SaveEntityResponse>.Fail(ex.Message, 201);
            }

            return Response<SaveEntityResponse>.Success(response, 200);
        }

        private SaveEntityResponse FormOrderCreate(StartFormRequest startFormRequest, OrderPerson person, long processInstanceKey, string contentData)
        {
            if (startFormRequest == null) return null;


            var formDefinition = _saveEntityService.GetFormDefinition(startFormRequest.FormId).Result;
            var config = new Config
            {
                ExpireInMinutes = formDefinition.ExpireInMinutes,
                MaxRetryCount = formDefinition.MaxRetryCount,
                RetryFrequence = formDefinition.RetryFrequence,
               
            };
            if (startFormRequest.OrderConfig!=null)
            {
                config.Device = startFormRequest.OrderConfig.Device;
                config.IsPersonalMail = startFormRequest.OrderConfig.IsPersonalMail;
            }

            var actions = new List<DocumentAction>();

            foreach (var action in formDefinition.Actions)
            {
                actions.Add(new DocumentAction
                {
                    DocumentActionId = Guid.NewGuid().ToString(),
                    Created = _dateTime.Now,
                    Choice = action.Choice,
                    Title = action.Title,
                    Type = action.Choice==(int)ActionType.Approve ? ActionType.Approve.ToString() : ActionType.Reject.ToString()
                });
            }

            var mimeType = formDefinition.Type.ToString() == ContentType.HTML.ToString() ? "text/html" : "application/pdf";
            if (startFormRequest.Source == "file")
                mimeType = startFormRequest.FileType;

            var document = new Domain.Entities.Document
            {
                DocumentId = Guid.NewGuid().ToString(),
                Content = startFormRequest.Content,
                ContentData = contentData,
                Name = startFormRequest.Title,
                Type = startFormRequest.Source == "formio" ? formDefinition.Type.ToString() : GetFileType(startFormRequest.FileType),
                FileType = startFormRequest.Source == "formio" ? formDefinition.Type.ToString() : GetFileType(startFormRequest.FileType),
                MimeType = mimeType,
                InsuranceType = startFormRequest.InsuranceType,
                Created = _dateTime.Now,
                DocumentActions = actions,
                FormDefinitionId = startFormRequest.FormId
            };

            var documents = new List<Domain.Entities.Document>();

            var customer = GetCustomerId(startFormRequest.Approver);
            if(customer.StatusCode != 200)
            {
                customer = GetCustomerId(startFormRequest.Approver);
            }

            var customerId = customer.Data;
            var personId = GetPersonId(person);

            if (!string.IsNullOrEmpty(startFormRequest.DependencyOrderId) && startFormRequest.Source == "file")
            {
                var orderGroup = _context.OrderGroups.FirstOrDefault(x => x.OrderMaps.Any(y => y.Order.CustomerId == customerId && y.OrderId == startFormRequest.DependencyOrderId));
                if (orderGroup != null)
                {
                    //var dependencyOrderDocument = _context.Documents
                    //                                        .Include(x => x.FormDefinition.Parameter)
                    //                                        .FirstOrDefault(x => x.OrderId == startFormRequest.DependencyOrderId);
                    //if (dependencyOrderDocument != null)
                    //{
                    //    document.InsuranceType = dependencyOrderDocument.InsuranceType;
                    //    document.DocumentInsuranceTypes = new List<DocumentInsuranceType> { new DocumentInsuranceType { DocumentInsuranceTypeId = Guid.NewGuid().ToString(), ParameterId = dependencyOrderDocument.FormDefinition.Parameter.ParameterId } };
                    //    documents = new List<Domain.Entities.Document> { document };
                    //}

                    documents = new List<Domain.Entities.Document> { document };
                    var orderMap = _context.OrderMaps.Add(new OrderMap
                    {
                        OrderMapId = Guid.NewGuid().ToString(),
                        OrderGroupId = orderGroup.OrderGroupId,
                        OrderId = startFormRequest.Id.ToString(),
                        OrderNumber = 2, // Teklif
                        DocumentId = document.DocumentId,
                        Order = new Order
                        {
                            OrderId = startFormRequest.Id.ToString(),
                            ProcessInstanceKey = processInstanceKey,
                            DocumentSystemId = formDefinition.DocumentSystemId,

                            State = OrderState.Pending.ToString(),
                            Title = startFormRequest.Title,
                            Created = _dateTime.Now,
                            Config = config,
                            CustomerId = customerId,
                            PersonId = personId,

                            Reference = new Reference
                            {
                                ProcessNo = startFormRequest.Reference.ProcessNo,
                                Created = _dateTime.Now,
                                Process = startFormRequest.Title,
                                State = startFormRequest.Title,
                            },
                            Documents = documents
                        }      
                    }).Entity;
                    var entity = _context.OrderMaps.Add(orderMap).Entity;
                }
            }
            else
            {
                //var documentInsuranceTypes = GetDocumentInsuranceType(startFormRequest.InsuranceType);
                //document.DocumentInsuranceTypes = documentInsuranceTypes;
                documents = new List<Domain.Entities.Document> { document };

                var orderGroupId = Guid.NewGuid().ToString();
                var orderGroup = new OrderGroup
                {
                    OrderGroupId = Guid.NewGuid().ToString(),
                    IsCompleted = false,
                    OrderMaps = new List<OrderMap>
                    {
                        new OrderMap
                        {
                            OrderMapId = Guid.NewGuid().ToString(),
                            OrderId = startFormRequest.Id.ToString(),
                            OrderGroupId= orderGroupId,
                            OrderNumber = 1, // Başvuru
                            Order = new Order
                            {
                                OrderId = startFormRequest.Id.ToString(),
                                ProcessInstanceKey = processInstanceKey,
                                DocumentSystemId = formDefinition.DocumentSystemId,

                                State = OrderState.Pending.ToString(),
                                Title = startFormRequest.Title,
                                Created = _dateTime.Now,
                                Config = config,
                                CustomerId = customerId,
                                PersonId = personId,

                                Reference = new Reference
                                {
                                    ProcessNo = startFormRequest.Reference.ProcessNo,
                                    Created = _dateTime.Now,
                                    Process = startFormRequest.Title,
                                    State = startFormRequest.Title,
                                },
                                Documents = documents,
                            },
                            DocumentId = document.DocumentId
                        }
                    }
                };
                var entity = _context.OrderGroups.Add(orderGroup).Entity;
            }

           var i = _context.SaveChanges();

            var order = _context.Orders
                .Where(x => x.OrderId == startFormRequest.Id)
                .Select(x => new SaveEntityResponse
                {
                    OrderId = x.OrderId,
                    ExpireInMinutes = x.Config.ExpireInMinutes,
                    MaxRetryCount = x.Config.MaxRetryCount,
                    RetryFrequence = x.Config.RetryFrequence,
                    Documents = x.Documents.Select(x => new SaveEntityDocumentResponse { DocumentId = x.DocumentId, Name = x.Name }).ToList()
                }).FirstOrDefault();

            return order;
        }

        private List<DocumentInsuranceType> GetDocumentInsuranceType(string insuranceType)
        {
            var documentInsuranceTypes= new List<DocumentInsuranceType>();
            foreach (var type in insuranceType.Split(","))
            {
                var parameter = _context.Parameters.FirstOrDefault(x => x.DmsReferenceId != null && x.Text.ToLower() == type.ToLower().Trim());
                if (parameter != null)
                    documentInsuranceTypes.Add(new DocumentInsuranceType { DocumentInsuranceTypeId = Guid.NewGuid().ToString(), ParameterId = parameter.ParameterId });
            }
            return documentInsuranceTypes;
        }

        private Response<string> GetCustomerId(OrderCustomer customer)
        {
            try
            {
                var customerId = _saveEntityService.GetCustomerAsync(customer.CitizenshipNumber).Result;
                if (customerId == null)
                    customerId = _saveEntityService.CustomerSaveAsync(customer).Result;
                else
                    customerId = _saveEntityService.CustomerUpdateAsync(customer).Result;
                return Response<string>.Success(customerId, 200);
            }
            catch (Exception ex)
            {
                return Response<string>.Fail(ex.Message, 201);
            }
        }

        private string GetPersonId(OrderPerson person)
        {
            var customerId = _saveEntityService.GetPersonAsync(person.CitizenshipNumber).Result;
            if (customerId == null)
                customerId = _saveEntityService.PersonSaveAsync(person).Result;
            else
                customerId = _saveEntityService.PersonUpdateAsync(person).Result;
            return customerId;
        }

        private SaveEntityResponse OrderCreate(StartRequest startRequest, OrderPerson person, long processInstanceKey)
        {
            if (startRequest == null) return null;

            var documents = new List<Domain.Entities.Document>();
            foreach (var item in startRequest.Documents)
            {
                var actions = new List<DocumentAction>();
                if (item.Actions != null)
                {
                    foreach (var action in item.Actions)
                    {
                        actions.Add(new DocumentAction
                        {
                            DocumentActionId = Guid.NewGuid().ToString(),
                            Created = _dateTime.Now,
                            Choice = action.Choice,
                            Title = action.Title,
                            Type = action.Choice == (int)ActionType.Approve ? ActionType.Approve.ToString() : ActionType.Reject.ToString()
                        });
                    }
                }

                string type = Enum.GetName(typeof(ContentType), item.Type);

                var content = item.Content;
                if (item.Type.ToString() == ((int)ContentType.PlainText).ToString())
                {
                    var plainTextBytes = Encoding.UTF8.GetBytes(content);
                    content = "data:text/plain;base64," + Convert.ToBase64String(plainTextBytes);
                }

                documents.Add(new Domain.Entities.Document
                {
                    DocumentId = Guid.NewGuid().ToString(),
                    Content = content,
                    Name = item.Title,
                    Type = type,
                    FileType = item.Type.ToString() == ((int)ContentType.PlainText).ToString() ? ContentType.PlainText.ToString() : GetFileType(item.FileType),
                    MimeType = item.FileType,
                    Created = _dateTime.Now,
                    DocumentActions = actions
                });
            }

            var config = new Config();
            if (startRequest.Config != null)
            {
                config.MaxRetryCount = startRequest.Config.MaxRetryCount;
                config.RetryFrequence = startRequest.Config.RetryFrequence;
                config.ExpireInMinutes = startRequest.Config.ExpireInMinutes;
                config.IsPersonalMail = startRequest.Config.IsPersonalMail;
                config.Device = startRequest.Config.Device; 

            }
            var customer = GetCustomerId(startRequest.Approver);
            if (customer.StatusCode != 200)
            {
                customer = GetCustomerId(startRequest.Approver);
            }

            var customerId = customer.Data;
            var personId = GetPersonId(person);
            var order = new Order
            {
                OrderId = startRequest.Id,
                DocumentSystemId = "b25635e8-1abd-4768-ab97-e1285999a62b",
                ProcessInstanceKey = processInstanceKey,
                State = OrderState.Pending.ToString(),
                Title = startRequest.Title,
                Created = _dateTime.Now,
                Config = config,
                CustomerId = customerId,
                PersonId = personId,

                Reference = new Reference
                {
                    ProcessNo = startRequest.Reference.ProcessNo,
                    Created = _dateTime.Now,
                    Process = startRequest.Reference.Process,
                    State = startRequest.Reference.State,
                },
                Documents = documents,
            };

            //var orderGroup = new OrderGroup { IsCompleted = false, OrderMaps = new List<OrderMap>(), OrderGroupId = Guid.NewGuid().ToString() };
            //orderGroup.OrderMaps.Add(new OrderMap { OrderMapId = Guid.NewGuid().ToString(), OrderGroupId = orderGroup.OrderGroupId, Order = order });
            //var entity = _context.OrderGroups.Add(orderGroup).Entity;

            _context.Orders.Add(order);
            _context.SaveChanges();

            return new SaveEntityResponse
            {
                OrderId = order.OrderId,
                ExpireInMinutes = order.Config.ExpireInMinutes,
                MaxRetryCount = order.Config.MaxRetryCount,
                RetryFrequence = order.Config.RetryFrequence,
                Documents = order.Documents.Select(x => new SaveEntityDocumentResponse { DocumentId = x.DocumentId, Name = x.Name }).ToList()
            };
        }

        private string GetFileType(string fileType)
        {
            if (fileType.Contains("image"))
            {
                return FileType.Image.ToString();
            }
            else if (fileType.ToLower().Contains("pdf"))
            {
                return FileType.PDF.ToString();
            }
            if (fileType.Contains("html"))
            {
                return FileType.HTML.ToString();
            }
            else
            {
                return FileType.File.ToString();
            }

        }

    }
}
