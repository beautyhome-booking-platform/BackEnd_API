using Application.ServiceCategories.Responses;
using Application.Services.Commands;
using Application.Services.Responses;
using Domain.DTO;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Handlers
{
    public class GetServiceHandler : IRequestHandler<GetServiceCommand, GetServiceReponse>
    {
        IUnitOfWork _unitOfWork;
        public GetServiceHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetServiceReponse> Handle(GetServiceCommand request, CancellationToken cancellationToken)
        {
            var response = new GetServiceReponse();

            var services = await _unitOfWork.ServiceRepository.FindAsync(s =>
                                                                        (s.Id == request.Id || request.Id == null) &&
                                                                        (string.IsNullOrEmpty(request.Name) || s.Name.Contains(request.Name)) &&
                                                                        (s.ServiceCategoryId == request.CategoryId || request.CategoryId == null)&&
                                                                        (!s.IsDeleted));

            if (services.Count() > 0)
            {
                var serviceDTO = services.Select(s => new ServiceDTO
                {
                    Id = s.Id,
                    Name = s.Name,
                    Description = s.Description,
                    ImageUrl = s.ImageUrl,
                    ServiceCategoryId = s.ServiceCategoryId
                }).ToList();
                response.IsSuccess = true;
                response.serviceDTOs = serviceDTO;
            }
            else
            {
                response.ErrorMessage = "Service not found or not have any services";
                return response;
            }
            return response;
        }
    }
}
