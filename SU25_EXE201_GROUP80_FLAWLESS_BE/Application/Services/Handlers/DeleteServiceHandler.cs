using Application.Services.Commands;
using Application.Services.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Handlers
{
    public class DeleteServiceHandler : IRequestHandler<DeleteServiceCommand, DeleteServiceResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteServiceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteServiceResponse> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteServiceResponse();

            var service = await _unitOfWork.ServiceRepository.GetByIdAsync(request.Id);

            if (service == null)
            {
                response.ErrorMessage = "Service not found";
                response.IsSuccess = false;
                return response;
            }

            service.IsDeleted = true; // Đánh dấu là đã xóa mềm

            // Nếu hard-delete:
            _unitOfWork.ServiceRepository.Update(service);

            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            return response;
        }
    }
}
