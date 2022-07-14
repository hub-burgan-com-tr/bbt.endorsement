using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using MediatR;


namespace Application.Parameter.Commands.UpdateParameters
{
    public class UpdateParameterCommand : IRequest<Response<bool>>
    {
        public string ParameterId { get; set; }
        public string Text { get; set; }
        public int? DmsReferenceId { get; set; }
        public int? DmsReferenceKey { get; set; }
        public string DmsReferenceName { get; set; }

    }


    public class UpdateParameterCommandHandler : IRequestHandler<UpdateParameterCommand, Response<bool>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDateTime _dateTime;

        public UpdateParameterCommandHandler(IApplicationDbContext context, IDateTime dateTime)
        {
            _context = context;
            _dateTime = dateTime;
        }

        public async Task<Response<bool>> Handle(UpdateParameterCommand request, CancellationToken cancellationToken)
        {
            UpdateParameterCommandValidator validator = new UpdateParameterCommandValidator();
            var respnse = validator.Validate(request);
            validator.ValidateAndThrow(request);
            var parameter = _context.Parameters.FirstOrDefault(x => x.ParameterId == request.ParameterId);
            if (parameter != null)
            {
                var IsText = _context.Parameters.Any(x => x.Text == request.Text.Trim()&&x.ParameterId!=parameter.ParameterId);

                if (IsText)
                    throw new Exception("Aynı Parametre Daha Önce Eklenmiştir");
            }
      
            int result = 0;
            if (parameter != null)
            {
                parameter.DmsReferenceId = request.DmsReferenceId;
                parameter.DmsReferenceKey = request.DmsReferenceKey;
                parameter.Created = DateTime.Now;
                parameter.DmsReferenceName = request.DmsReferenceName;
                _context.Parameters.Update(parameter);
                result = _context.SaveChanges();
            }

            return Response<bool>.Success(result > 0 ? true : false, 200);
        }
    }

}
