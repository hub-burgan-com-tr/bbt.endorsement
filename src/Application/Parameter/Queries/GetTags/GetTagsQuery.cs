﻿using Application.Common.Interfaces;
using Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Queries.GetTags
{
    public class GetTagsQuery : IRequest<Response<List<GetTagsDto>>>
    {
    }
    public class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, Response<List<GetTagsDto>>>
    {
        private IApplicationDbContext _context;

        public GetTagsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Response<List<GetTagsDto>>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var response = _context.FormDefinitionTags.Select(x => new GetTagsDto { FormDefinitionTagId = x.FormDefinitionTagId, Tag = x.Tag }).OrderBy(x => x.Tag).ToList();
            return Response<List<GetTagsDto>>.Success(response, 200);
        }
    }

}
