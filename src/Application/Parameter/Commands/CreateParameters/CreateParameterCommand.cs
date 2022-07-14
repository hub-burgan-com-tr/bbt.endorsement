using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;

namespace Application.Parameter.Commands.CreateParameters
{
    public class CreateParameterCommand : IRequest<Response<bool>>
    {
        public string ParameterTypeId { get; set; }
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
            var response = validator.Validate(request);
            validator.ValidateAndThrow(request);
            var IsText = _context.Parameters.Any(x => x.Text == request.Text.Trim() && x.ParameterTypeId == request.ParameterTypeId);
            if (IsText)
                throw new Exception("Aynı Parametre Daha Önce Eklenmiştir");
            int result = 0;
            var parameter = _context.Parameters.Add(new Domain.Entities.Parameter
            {
                ParameterId=Guid.NewGuid().ToString(),
                DmsReferenceKey = request.DmsReferenceKey,
                DmsReferenceId = request.DmsReferenceId,
                ParameterTypeId = request.ParameterTypeId,
                Text = request.Text,
                DmsReferenceName = request.DmsReferenceName,
                Created = DateTime.Now,
            });

            result = _context.SaveChanges();
            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }
}