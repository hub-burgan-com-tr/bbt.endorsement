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
            if(request.Model.FormType == Form.Order)
                response = OrderCreate(request.Model.StartRequest, request.Model.Person, request.ProcessInstanceKey);
            else if (request.Model.FormType == Form.FormOrder)
                response = FormOrderCreate(request.Model.StartFormRequest, request.Model.Person, request.ProcessInstanceKey);

            var documentList = _context.Documents.Where(x => x.OrderId == response.OrderId);
            var saveEntityDocuments = new List<SaveEntityDocumentResponse>();
            foreach (var item in documentList)
                saveEntityDocuments.Add(new SaveEntityDocumentResponse { DocumentId = item.DocumentId, Name = item.Name });

            response.Documents = saveEntityDocuments;

            return Response<SaveEntityResponse>.Success(response, 200);
        }

        private SaveEntityResponse FormOrderCreate(StartFormRequest startFormRequest, OrderPerson person, long processInstanceKey)
        {
            if (startFormRequest == null) return null;

            var documents = new List<Domain.Entities.Document>();

            var formDefinition = _saveEntityService.GetFormDefinition(startFormRequest.FormId).Result;
            var config = new Config
            {
                ExpireInMinutes = formDefinition.ExpireInMinutes,
                MaxRetryCount = formDefinition.MaxRetryCount,
                RetryFrequence = formDefinition.RetryFrequence,
            };

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
                mimeType = GetMimeType(formDefinition.Type);
            
            documents.Add(new Domain.Entities.Document
            {
                DocumentId = Guid.NewGuid().ToString(),
                Content = startFormRequest.Content,
                Name = startFormRequest.Title,
                Type = formDefinition.Type.ToString(),
                FileType = formDefinition.Type.ToString(),
                MimeType = mimeType,
                InsuranceType = startFormRequest.InsuranceType,
                Created = _dateTime.Now,
                DocumentActions = actions,
                FormDefinitionId = startFormRequest.FormId
            });

            var customerId = GetCustomerId(startFormRequest.Approver);
            var personId = GetPersonId(person);
            var order = new Order
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
            };

            if(!string.IsNullOrEmpty(startFormRequest.DependencyOrderId) && startFormRequest.Source == "file")
            {
                var orderGroup = _context.OrderGroups.FirstOrDefault(x => x.OrderMaps.Any(y => y.Order.CustomerId == customerId && y.OrderId == startFormRequest.DependencyOrderId));
                if (orderGroup != null)
                {
                    var orderMaps = _context.OrderMaps.FirstOrDefault(x => x.Order.CustomerId == customerId &&
                                                                           x.OrderGroupId == orderGroup.OrderGroupId && 
                                                                           x.Order.Documents.Any(y => y.FormDefinitionId == startFormRequest.FormId));
                    if (orderMaps == null)
                    {
                        var orderMap = _context.OrderMaps.Add(new OrderMap
                        {
                            OrderMapId = Guid.NewGuid().ToString(),
                            OrderGroupId = orderGroup.OrderGroupId,
                            Order = order,
                        }).Entity;
                        var entity = _context.OrderMaps.Add(orderMap).Entity;
                    }
                }
            }
            else
            {
                var orderGroup = new OrderGroup { IsCompleted = false, OrderMaps = new List<OrderMap>(), OrderGroupId = Guid.NewGuid().ToString() };
                if (orderGroup != null)
                {
                    orderGroup.OrderMaps.Add(new OrderMap { OrderMapId = Guid.NewGuid().ToString(), OrderGroupId = orderGroup.OrderGroupId, Order = order });
                    var entity = _context.OrderGroups.Add(orderGroup).Entity;
                }
            }

            _context.SaveChanges();

            return new SaveEntityResponse
            {
                OrderId = order.OrderId,
                ExpireInMinutes = order.Config.ExpireInMinutes,
                MaxRetryCount = order.Config.MaxRetryCount,
                RetryFrequence = order.Config.RetryFrequence
            };
        }

        private string GetCustomerId(OrderCustomer customer)
        {
            var customerId = _saveEntityService.GetCustomerAsync(customer.CitizenshipNumber).Result;
            if (customerId == null)
                customerId = _saveEntityService.CustomerSaveAsync(customer).Result;
            return customerId;
        }

        private string GetPersonId(OrderPerson person)
        {
            var customerId = _saveEntityService.GetPersonAsync(person.CitizenshipNumber).Result;
            if (customerId == null)
                customerId = _saveEntityService.PersonSaveAsync(person).Result;
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
                if(item.Type.ToString() == ((int)ContentType.PlainText).ToString())
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
                    MimeType = GetMimeType(item.FileType),
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
            }

            var customerId = GetCustomerId(startRequest.Approver);
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

            var entity = _context.Orders.Add(order).Entity;

            _context.SaveChanges();

            return new SaveEntityResponse
            {
                OrderId = order.OrderId,
                ExpireInMinutes = order.Config.ExpireInMinutes,
                MaxRetryCount = order.Config.MaxRetryCount,
                RetryFrequence= order.Config.RetryFrequence
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


        private string GetMimeType(string fileType)
        {
            if (fileType.Contains("image"))
            {
                return "image/png";
            }
            else if (fileType.ToLower().Contains("pdf"))
            {
                return "application/pdf";
            }
           else if (fileType.Contains("html"))
            {
                return "text/html";
            }
            else
            {
                return FileType.File.ToString();
            }

        }
    }
}
