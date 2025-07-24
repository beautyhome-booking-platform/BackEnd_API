using Application.Area.Commands;
using Application.Area.Responses;
using Domain.DTO;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Area.Handlers
{
    public class GetDistrictsHandler : IRequestHandler<GetDistrictsCommand, GetDistricsResponse>
    {
        IUnitOfWork _unitOfWork;

        public GetDistrictsHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetDistricsResponse> Handle(GetDistrictsCommand request, CancellationToken cancellationToken)
        {
            var response = new GetDistricsResponse();

            var areas = await _unitOfWork.AreaRepository.FindAsync(a => (a.City == request.City) && (!a.IsDeleted));

            // Lấy danh sách district duy nhất, kèm Id đầu tiên (nếu có nhiều record cùng tên quận)
            var districts = areas
                .GroupBy(a => a.District)
                .Select(g => new AreaDTO
                {
                    Id = g.First().Id,
                    Name = g.Key,
                })
                .OrderBy(d => d.Name)
                .ToList();

            response.Districts = districts;
            response.IsSuccess = true;

            return response;
        }
    }
}
