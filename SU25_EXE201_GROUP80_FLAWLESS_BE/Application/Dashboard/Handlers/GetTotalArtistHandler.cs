using Application.Dashboard.Commands;
using Application.Dashboard.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dashboard.Handlers
{
    public class GetTotalArtistHandler : IRequestHandler<GetTotalArtistCommand, GetTotalArtistResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetTotalArtistHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetTotalArtistResponse> Handle(GetTotalArtistCommand request, CancellationToken cancellationToken)
        {
            var response = new GetTotalArtistResponse();

            // Lấy toàn bộ ArtistProgress (chỉ artist mới có bản ghi này)
            var artistProgresses = await _unitOfWork.ArtistProgressRepository.GetAllAsync();
            var users = await _unitOfWork.UserAppRepository.GetAllAsync();

            // Join để lấy thông tin user tương ứng với từng artist
            var artistInfos = artistProgresses
                .Where(p => p.Artist.Id != null)
                .Select(p =>
                {
                    var user = users.FirstOrDefault(u => u.Id == p.Artist.Id);
                    return new
                    {
                        User = user,
                        StartDate = p.CreateAt,
                        UpdateAt = p.UpdateAt,
                        LockoutEnabled = user?.LockoutEnabled ?? false,
                        LockoutEnd = user?.LockoutEnd
                    };
                })
                .Where(x => x.StartDate.HasValue)
                .ToList();

            // Tổng artist toàn hệ thống
            response.TotalArtistAllYear = artistInfos.Count();

            // Lấy tất cả các năm có artist bắt đầu hoạt động
            var years = artistInfos
                .Select(x => x.StartDate.Value.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToList();

            foreach (var year in years)
            {
                var yearDto = new TotalArtistPerYearDto { Year = year };

                // Artist đã hoạt động đến hết năm này
                yearDto.TotalArtistPerYear = artistInfos.Count(a => a.StartDate.Value.Year <= year);

                var monthsDict = new Dictionary<string, TotalArtistMonthDto>();

                for (int m = 1; m <= 12; m++)
                {
                    var monthKey = new DateTime(year, m, 1).ToString("MMM", CultureInfo.InvariantCulture).ToLower();

                    // Artist đã hoạt động đến hết tháng này
                    var totalArtist = artistInfos.Count(a =>
                        (a.StartDate.Value.Year < year) ||
                        (a.StartDate.Value.Year == year && a.StartDate.Value.Month <= m)
                    );

                    // Artist mới trong tháng này
                    var newArtist = artistInfos.Count(a =>
                        a.StartDate.Value.Year == year && a.StartDate.Value.Month == m
                    );

                    // Artist bị ban trong tháng này
                    var now = DateTime.UtcNow;
                    var banArtist = artistInfos.Count(a =>
                        a.LockoutEnabled &&
                        a.LockoutEnd.HasValue &&
                        a.LockoutEnd.Value > now &&
                        a.UpdateAt.HasValue &&
                        a.UpdateAt.Value.Year == year &&
                        a.UpdateAt.Value.Month == m
                    );

                    monthsDict[monthKey] = new TotalArtistMonthDto
                    {
                        TotalArtist = totalArtist,
                        NewArtist = newArtist,
                        BanArtist = banArtist
                    };
                }

                yearDto.Months = monthsDict;
                response.PerYear.Add(yearDto);
            }

            response.IsSuccess = true;
            return response;
        }
    }
}
