using MediatR;
using Worker.App.Application.Common.Interfaces;
using Worker.App.Application.Common.Models;
using Domain.Enums;
using System.Text;
using System.Net.Http.Headers;
using Serilog;

namespace Worker.App.Application.Workers.Commands.DeleteEntities
{
    public class DeleteEntityCommand : IRequest<Response<DeleteEntityResponse>>
    {
        public string OrderId { get; set; }
    }

    public class DeleteEntityCommandHandler : IRequestHandler<DeleteEntityCommand, Response<DeleteEntityResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public DeleteEntityCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<DeleteEntityResponse>> Handle(DeleteEntityCommand request, CancellationToken cancellationToken)
        {
            var order = _context.Orders.FirstOrDefault(x => x.OrderId == request.OrderId);

            if (order == null && order.State != OrderState.Pending.ToString())
            {
                var state = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());
                return Response<DeleteEntityResponse>.Success(new DeleteEntityResponse { OrderState = state, IsUpdated = false }, 200);
            }

            order.State = OrderState.Cancel.ToString();
            order.LastModified = _dateTime.Now;
            order = _context.Orders.Update(order).Entity;

            var documents = order.Documents.Where(x => x.OrderId == order.OrderId);
            foreach (var document in documents)
            {
                document.State = OrderState.Cancel.ToString();
                _context.Documents.Update(document);
            }

            _context.SaveChanges();

            var authToken = _context.Configs.Where(x => x.OrderId == request.OrderId).Select(x => x.ContractAuthToken).FirstOrDefault();
            var client = new HttpClient();
            Uri uri = new Uri(StaticValues.AmorphieWorkflowUrl + request.OrderId + "/transition/cancel-contract");
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
            httpRequest.Content = content;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken.Replace("Bearer ", ""));
            client.DefaultRequestHeaders.Add("User", StaticValues.ContractUserCode);
            client.DefaultRequestHeaders.Add("Behalf-Of-User", StaticValues.ContractUserCode);

            var result = await client.SendAsync(httpRequest);
            var responseContent = await result.Content.ReadAsStringAsync();

            if (result.IsSuccessStatusCode)
            {
                Log.ForContext("OrderId", request.OrderId)
                .ForContext("HttpResponseStatus", result.StatusCode)
                .Information($"Contract canceled started.");
            }
            else
            {
                Log.ForContext("OrderId", request.OrderId)
                .ForContext("HttpResponseStatus", result.StatusCode)
                .Error($"Contract Cancel Request Error. Content: " + responseContent);
            }

            var orderState = (OrderState)Enum.Parse(typeof(OrderState), order.State.ToString());
            return Response<DeleteEntityResponse>.Success(new DeleteEntityResponse { OrderState = orderState, IsUpdated = true }, 200);
        }
    }
}
