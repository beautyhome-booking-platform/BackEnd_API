using Application.ServiceCategories.Commands;
using Application.ServiceCategories.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceCategories.Handlers
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, DeleteCategoryResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeleteCategoryResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var response = new DeleteCategoryResponse();

            var category = await _unitOfWork.ServiceCategoriesRepository.GetByIdAsync(request.Id);

            if (category == null || category.IsDeleted)
            {
                response.ErrorMessage = "Category not found";
                response.IsSuccess = false;
                return response;
            }
            
            category.IsDeleted = true; // Soft delete

            // Nếu muốn hard delete
            _unitOfWork.ServiceCategoriesRepository.Update(category);

            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            return response;
        }
    }
}
