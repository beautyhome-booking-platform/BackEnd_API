using Application.ServiceCategories.Commands;
using Application.ServiceCategories.Responses;
using Domain.DTO;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceCategories.Handlers
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryCommand, GetServiceResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetServiceResponse> Handle(GetCategoryCommand request, CancellationToken cancellationToken)
        {
            var response = new GetServiceResponse();

            var serviceCategory = await _unitOfWork.ServiceCategoriesRepository.FindAsync(sc => (request.Id == null || sc.Id == request.Id) 
                                                                                &&(string.IsNullOrEmpty(request.Name) || sc.Name.Contains(request.Name))
                                                                                &&(!sc.IsDeleted)
                                                                                );
            if(serviceCategory.Count() > 0)
            {
                var serviceCategoryDTO = serviceCategory.Select(sc => new ServiceCategoryDTO
                {
                    Id = sc.Id,
                    Name = sc.Name,
                    Description = sc.Description,
                    ImageUrl = sc.ImageUrl
                }).ToList();
                response.IsSuccess = true;
                response.ServiceCategory = serviceCategoryDTO;
            }
            else
            {
                response.ErrorMessage = "Service Category not found or not have any service categories";
                return response;
            }
            return response;
        }
    }
}
