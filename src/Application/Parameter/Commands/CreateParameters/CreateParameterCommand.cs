using Application.Common.Interfaces;
using Application.Common.Models;
using Domain.Entities;
using Domain.Enums;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Parameter.Commands.CreateParameters
{
    public class CreateParameterCommand : IRequest<Response<bool>>
    {
     
        public int ParameterTypeId { get; set; }
        public string Text { get; set; }
        public int? DmsReferenceId { get; set; }
        public int? DmsReferenceKey { get; set; }
        public string DmsReferenceName { get; set; }


    }


    public class CreateParameterCommandCommandHandler : IRequestHandler<CreateParameterCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public CreateParameterCommandCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(CreateParameterCommand request, CancellationToken cancellationToken)
        {
            CreateParameterCommandValidator validator = new CreateParameterCommandValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            var IsText = _context.Parameters.Any(x=>x.Text==request.Text.Trim());
            if (IsText)
                throw new Exception("Aynı Parametre Daha Önce Eklenmiştir");
            int result = 0;             
                var parameter = _context.Parameters.Add(new Domain.Entities.Parameter
                {
                   
                    DmsReferenceKey=request.DmsReferenceKey,
                    DmsReferenceId=request.DmsReferenceId,
                    ParameterTypeId=request.ParameterTypeId,
                    Text=request.Text,
                    DmsReferenceName=request.DmsReferenceName,
                    Created=DateTime.Now,
                });
                
                result = _context.SaveChanges();
            

            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }



}
