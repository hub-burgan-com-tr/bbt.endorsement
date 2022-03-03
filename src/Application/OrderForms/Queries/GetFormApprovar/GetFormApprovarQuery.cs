using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Models;
using MediatR;

namespace Application.OrderForms.Queries.GetFormApprovar
{
    public class GetFormApprovarQuery:IRequest<Response<List<GetFormApprovarDto>>>
    {
        public int FormId { get; set; }
    }
    /// <summary>
    /// Form Ve Onaycı Listesi
    /// </summary>
    public class GetFormApprovarQueryHandler : IRequestHandler<GetFormApprovarQuery, Response<List<GetFormApprovarDto>>>
    {
        public async Task<Response<List<GetFormApprovarDto>>> Handle(GetFormApprovarQuery request, CancellationToken cancellationToken)
        {
            var list = new List<GetFormApprovarDto>();
            return Response<List<GetFormApprovarDto>>.Success(list, 200);
        }
    }
}
