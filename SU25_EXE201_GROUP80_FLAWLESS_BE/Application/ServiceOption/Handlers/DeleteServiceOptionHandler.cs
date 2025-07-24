using Application.ServiceOption.Commands;
using Application.ServiceOption.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceOption.Handlers
{
    public class DeleteServiceOptionHandler : IRequestHandler<DeleteServiceOptionCommand, DeleteServiceOptionResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteServiceOptionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DeleteServiceOptionResponse> Handle(DeleteServiceOptionCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteServiceOptionResponse();

            var serviceOption = await _unitOfWork.ServiceOptionRepository.GetByIdAsync(request.Id);

            if (serviceOption == null || serviceOption.IsDeleted)
            {
                response.ErrorMessage = "Service option not found";
                response.IsSuccess = false;
                return response;
            }

            serviceOption.IsDeleted = true; // Đánh dấu là đã xóa (soft-delete)

            // Nếu dùng hard-delete:
            _unitOfWork.ServiceOptionRepository.Update(serviceOption);

            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            return response;
        }
    }
}
