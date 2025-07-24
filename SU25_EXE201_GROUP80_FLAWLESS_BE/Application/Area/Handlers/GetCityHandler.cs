using Application.Area.Commands;
using Application.Area.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Area.Handlers
{
    public class GetCityHandler : IRequestHandler<GetCityCommand, GetCityResponpse>
    {
        IUnitOfWork _unitOfWork;
        public GetCityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetCityResponpse> Handle(GetCityCommand request, CancellationToken cancellationToken)
        {
            var response = new GetCityResponpse();

            var cities = await _unitOfWork.AreaRepository.GetAllAsync();

            // Lấy danh sách city duy nhất, bỏ city null hoặc empty và sắp xếp
            response.CityList = cities
                .Where(c => (!string.IsNullOrEmpty(c.City)) && (!c.IsDeleted))
                .Select(c => c.City)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            response.IsSuccess = true;

            return response;
        }
    }
}
