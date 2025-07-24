using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserService.Handlers
{
    public class GetDataByQueryModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDataByQueryModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public async Task<List<string>> GetData(QueryModel entity)
        //{
        //    var results = new List<string>();
        //    if (string.IsNullOrEmpty(entity.QueryType))
        //    {
        //        results.Add("Câu hỏi của bạn chưa xác định được nội dung cần tra cứu.");
        //        return results;
        //    }

        //    var queryTypes = entity.QueryType
        //        .ToLower()
        //        .Split(',', StringSplitOptions.RemoveEmptyEntries)
        //        .Select(q => q.Trim())
        //        .ToList();

        //    // Nếu có artist
        //    if (queryTypes.Contains("artist"))
        //    {
        //        var artists = await _unitOfWork.InformationArtistRepository.FindAsync(
        //            ad => string.IsNullOrEmpty(entity.Location)
        //                    ? true
        //                    : (ad.Area.City.Contains(entity.Location) || ad.Area.District.Contains(entity.Location)),
        //            q => q.Include(x => x.Area)
        //                  .Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist)
        //        );

        //        foreach (var art in artists.Take(5))
        //        {
        //            var obj = new
        //            {
        //                ArtistId = art.ArtistProgress.Artist.Id,
        //                Name = art.ArtistProgress.Artist.Name,
        //                City = art.Area?.City,
        //                District = art.Area?.District,
        //                MinPrice = art.MinPrice,
        //                MaxPrice = art.MaxPrice,
        //                YearsOfExperience = art.YearsOfExperience
        //            };
        //            results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
        //        }
        //    }

        //    // Nếu có service
        //    if (queryTypes.Contains("service"))
        //    {
        //        var serviceOptions = await _unitOfWork.ServiceOptionRepository.FindAsync(
        //            so => true,
        //            q => q.Include(x => x.Service)
        //        );

        //        // Lọc theo artist nếu có
        //        if (!string.IsNullOrEmpty(entity.ArtistName))
        //        {
        //            var artists = await _unitOfWork.InformationArtistRepository.FindAsync(
        //                a => a.ArtistProgress.Artist.Name.Contains(entity.ArtistName),
        //                q => q.Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist)
        //            );
        //            var artistIds = artists
        //                .Where(a => a.ArtistProgress?.Artist != null)
        //                .Select(a => a.ArtistProgress.Artist.Id.ToString())
        //                .ToList();
        //            var servicesOfArtist = serviceOptions
        //                .Where(so => !string.IsNullOrEmpty(so.ArtistId) && artistIds.Contains(so.ArtistId))
        //                .Take(5)
        //                .ToList();
        //            foreach (var svc in servicesOfArtist)
        //            {
        //                var obj = new
        //                {
        //                    ServiceOptionId = svc.Id,
        //                    Name = svc.Name,
        //                    Price = svc.Price,
        //                    ArtistId = svc.ArtistId,
        //                    ServiceId = svc.ServiceId
        //                };
        //                results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
        //            }
        //        }
        //        // Lọc theo location nếu có (ưu tiên location nếu không có artist)
        //        else if (!string.IsNullOrEmpty(entity.Location))
        //        {
        //            var artists = await _unitOfWork.InformationArtistRepository.FindAsync(
        //                a => a.Area.City.Contains(entity.Location) || a.Area.District.Contains(entity.Location),
        //                q => q.Include(x => x.Area)
        //                      .Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist)
        //            );
        //            var artistIds = artists
        //                .Where(a => a.ArtistProgress?.Artist != null)
        //                .Select(a => a.ArtistProgress.Artist.Id.ToString())
        //                .ToList();
        //            var servicesOfLocation = serviceOptions
        //                .Where(so => !string.IsNullOrEmpty(so.ArtistId) && artistIds.Contains(so.ArtistId))
        //                .Take(5)
        //                .ToList();
        //            foreach (var svc in servicesOfLocation)
        //            {
        //                var obj = new
        //                {
        //                    ServiceOptionId = svc.Id,
        //                    Name = svc.Name,
        //                    Price = svc.Price,
        //                    ArtistId = svc.ArtistId,
        //                    ServiceId = svc.ServiceId
        //                };
        //                results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
        //            }
        //        }
        //        // Nếu không có artist/location, trả hết
        //        else
        //        {
        //            foreach (var svc in serviceOptions.Take(5))
        //            {
        //                var obj = new
        //                {
        //                    ServiceOptionId = svc.Id,
        //                    Name = svc.Name,
        //                    Price = svc.Price,
        //                    ArtistId = svc.ArtistId,
        //                    ServiceId = svc.ServiceId
        //                };
        //                results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
        //            }
        //        }
        //    }

        //    // Nếu có feedback
        //    if (queryTypes.Contains("feedback"))
        //    {
        //        // Feedback của artist
        //        if (!string.IsNullOrEmpty(entity.ArtistName))
        //        {
        //            var artists = await _unitOfWork.InformationArtistRepository.FindAsync(
        //                a => a.ArtistProgress.Artist.Name.Contains(entity.ArtistName),
        //                q => q.Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist)
        //            );
        //            var artistIds = artists
        //                .Where(a => a.ArtistProgress?.Artist != null)
        //                .Select(a => a.ArtistProgress.Artist.Id.ToString())
        //                .ToList();

        //            var serviceOptions = await _unitOfWork.ServiceOptionRepository.GetAllAsync();
        //            var serviceOptionIds = serviceOptions
        //                .Where(so => !string.IsNullOrEmpty(so.ArtistId) && artistIds.Contains(so.ArtistId))
        //                .Select(so => so.Id)
        //                .ToList();

        //            var feedbacks = await _unitOfWork.FeedbackRepository.FindAsync(
        //                fb => true,
        //                q => q.Include(x => x.User)
        //            );
        //            var topFeedbacks = feedbacks
        //                .Where(fb => serviceOptionIds.Contains(fb.ServiceOptionId))
        //                .OrderByDescending(fb => fb.CreateAt)
        //                .Take(5)
        //                .ToList();

        //            if (!topFeedbacks.Any())
        //                results.Add("Artist này chưa có feedback nào.");
        //            else
        //                foreach (var fb in topFeedbacks)
        //                {
        //                    var serviceOption = serviceOptions.FirstOrDefault(so => so.Id == fb.ServiceOptionId);
        //                    var obj = new
        //                    {
        //                        FeedbackId = fb.Id,
        //                        User = fb.User.Name,
        //                        Content = fb.Content,
        //                        Rating = fb.Rating,
        //                        ServiceOptionId = fb.ServiceOptionId,
        //                        ArtistId = serviceOption?.ArtistId
        //                    };
        //                    results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
        //                }
        //        }
        //        // Feedback của service
        //        else if (!string.IsNullOrEmpty(entity.ServiceType))
        //        {
        //            var serviceOptions = await _unitOfWork.ServiceOptionRepository.FindAsync(
        //                so => true,
        //                q => q.Include(x => x.Service)
        //            );
        //            var serviceOptionIds = serviceOptions
        //                .Where(so => so.Name.Contains(entity.ServiceType))
        //                .Select(so => so.Id)
        //                .ToList();

        //            var feedbacks = await _unitOfWork.FeedbackRepository.FindAsync(
        //                fb => true,
        //                q => q.Include(x => x.User)
        //            );
        //            var topFeedbacks = feedbacks
        //                .Where(fb => serviceOptionIds.Contains(fb.ServiceOptionId))
        //                .OrderByDescending(fb => fb.CreateAt)
        //                .Take(5)
        //                .ToList();

        //            if (!topFeedbacks.Any())
        //                results.Add("Dịch vụ này chưa có feedback nào.");
        //            else
        //                foreach (var fb in topFeedbacks)
        //                {
        //                    var serviceOption = serviceOptions.FirstOrDefault(so => so.Id == fb.ServiceOptionId);
        //                    var obj = new
        //                    {
        //                        FeedbackId = fb.Id,
        //                        User = fb.User.Name,
        //                        Content = fb.Content,
        //                        Rating = fb.Rating,
        //                        ServiceOptionId = fb.ServiceOptionId,
        //                        ArtistId = serviceOption?.ArtistId
        //                    };
        //                    results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
        //                }
        //        }
        //        else
        //        {
        //            results.Add("Vui lòng cung cấp tên artist hoặc tên dịch vụ để xem feedback.");
        //        }
        //    }

        //    // Nếu có location mà KHÔNG có artist/service (chỉ lấy thông tin artist/location)
        //    if (queryTypes.Contains("location") && !queryTypes.Contains("artist") && !queryTypes.Contains("service"))
        //    {
        //        var artists = await _unitOfWork.InformationArtistRepository.FindAsync(
        //            ia => true,
        //            q => q.Include(x => x.Area)
        //                  .Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist)
        //        );
        //        foreach (var art in artists.Where(a => a.Area != null).Take(5))
        //        {
        //            var obj = new
        //            {
        //                ArtistId = art.ArtistProgress.Artist.Id,
        //                Name = art.ArtistProgress.Artist.Name,
        //                City = art.Area?.City,
        //                District = art.Area?.District,
        //                MinPrice = art.MinPrice,
        //                MaxPrice = art.MaxPrice,
        //                YearsOfExperience = art.YearsOfExperience
        //            };
        //            results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
        //        }
        //    }

        //    if (queryTypes.Contains("policy"))
        //    {
        //        results.Add("Bạn có thể xem chi tiết chính sách hoàn/hủy dịch vụ tại trang Chính sách của hệ thống.");
        //    }
        //    if (queryTypes.Contains("event"))
        //    {
        //        results.Add("Các sự kiện nổi bật sẽ được cập nhật trên trang chủ hệ thống.");
        //    }

        //    if (!results.Any())
        //    {
        //        results.Add("Câu hỏi chưa đủ thông tin để tra cứu dữ liệu. Bạn có thể hỏi về dịch vụ, artist, feedback, location, policy, event...");
        //    }

        //    return results;
        //}
        public async Task<List<string>> GetData(QueryModel entity)
        {
            var results = new List<string>();
            if (string.IsNullOrEmpty(entity.QueryType))
            {
                results.Add("Câu hỏi của bạn chưa xác định được nội dung cần tra cứu.");
                return results;
            }

            var queryTypes = entity.QueryType
                .ToLower()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(q => q.Trim())
                .ToList();

            // Nếu có artist
            if (queryTypes.Contains("artist"))
            {
                var artists = await _unitOfWork.InformationArtistRepository.FindAsync(
                    ad => string.IsNullOrEmpty(entity.Location)
                            ? true
                            : (ad.Area.City.Contains(entity.Location) || ad.Area.District.Contains(entity.Location)),
                    q => q.Include(x => x.Area)
                          .Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist)
                );

                foreach (var art in artists.Take(5))
                {
                    var obj = new
                    {
                        ArtistId = art.ArtistProgress.Artist.Id,
                        Name = art.ArtistProgress.Artist.Name,
                        City = art.Area?.City,
                        District = art.Area?.District,
                        MinPrice = art.MinPrice,
                        MaxPrice = art.MaxPrice,
                        YearsOfExperience = art.YearsOfExperience
                    };
                    results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
                }
            }

            // Nếu có service (LUÔN trả về thông tin artist kèm service)
            if (queryTypes.Contains("service"))
            {
                // Lấy toàn bộ service option trước
                var serviceOptions = await _unitOfWork.ServiceOptionRepository.FindAsync(
                    so => true,
                    q => q.Include(x => x.Service)
                );

                // Chuẩn bị danh sách artist liên quan
                List<Domain.Entities.InformationArtist> artists = null;
                Dictionary<string, Domain.Entities.InformationArtist> artistDict = null;

                // Lọc theo artist
                if (!string.IsNullOrEmpty(entity.ArtistName))
                {
                    artists = (await _unitOfWork.InformationArtistRepository.FindAsync(
                        a => a.ArtistProgress.Artist.Name.Contains(entity.ArtistName),
                        q => q.Include(x => x.Area)
                              .Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist))
                    ).ToList();
                    artistDict = artists
                        .Where(a => a.ArtistProgress?.Artist != null)
                        .ToDictionary(a => a.ArtistProgress.Artist.Id.ToString(), a => a);
                }
                // Lọc theo location
                else if (!string.IsNullOrEmpty(entity.Location))
                {
                    artists = (await _unitOfWork.InformationArtistRepository.FindAsync(
                            a => a.Area.City.Contains(entity.Location) || a.Area.District.Contains(entity.Location),
                            q => q.Include(x => x.Area)
                                  .Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist))
                        ).ToList();
                    artistDict = artists
                        .Where(a => a.ArtistProgress?.Artist != null)
                        .ToDictionary(a => a.ArtistProgress.Artist.Id.ToString(), a => a);
                }
                
                else
                {
                    artists = (await _unitOfWork.InformationArtistRepository.FindAsync(
                            a => true,
                            q => q.Include(x => x.Area)
                                  .Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist))
                        ).ToList();
                    artistDict = artists
                        .Where(a => a.ArtistProgress?.Artist != null)
                        .ToDictionary(a => a.ArtistProgress.Artist.Id.ToString(), a => a);
                }

                // Lấy danh sách ArtistId hợp lệ
                var artistIds = artistDict.Keys.ToList();

                // Lọc ServiceOption theo artistIds
                var filteredServices = serviceOptions
                    .Where(so => !string.IsNullOrEmpty(so.ArtistId) && artistIds.Contains(so.ArtistId))
                    .Take(5)
                    .ToList();

                foreach (var svc in filteredServices)
                {
                    artistDict.TryGetValue(svc.ArtistId, out var artist);

                    var obj = new
                    {
                        ServiceOptionId = svc.Id,
                        ServiceName = svc.Name,
                        Price = svc.Price,
                        ArtistId = svc.ArtistId,
                        ServiceId = svc.ServiceId,
                        // Thông tin Artist liên quan
                        ArtistName = artist?.ArtistProgress?.Artist?.Name,
                        City = artist?.Area?.City,
                        District = artist?.Area?.District,
                        YearsOfExperience = artist?.YearsOfExperience
                    };
                    results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
                }
            }

            // Nếu có feedback
            if (queryTypes.Contains("feedback"))
            {
                // Feedback của artist
                if (!string.IsNullOrEmpty(entity.ArtistName))
                {
                    var artists = await _unitOfWork.InformationArtistRepository.FindAsync(
                        a => a.ArtistProgress.Artist.Name.Contains(entity.ArtistName),
                        q => q.Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist)
                    );
                    var artistIds = artists
                        .Where(a => a.ArtistProgress?.Artist != null)
                        .Select(a => a.ArtistProgress.Artist.Id.ToString())
                        .ToList();

                    var serviceOptions = await _unitOfWork.ServiceOptionRepository.GetAllAsync();
                    var serviceOptionIds = serviceOptions
                        .Where(so => !string.IsNullOrEmpty(so.ArtistId) && artistIds.Contains(so.ArtistId))
                        .Select(so => so.Id)
                        .ToList();

                    var feedbacks = await _unitOfWork.FeedbackRepository.FindAsync(
                        fb => true,
                        q => q.Include(x => x.User)
                    );
                    var topFeedbacks = feedbacks
                        .Where(fb => serviceOptionIds.Contains(fb.ServiceOptionId))
                        .OrderByDescending(fb => fb.CreateAt)
                        .Take(5)
                        .ToList();

                    if (!topFeedbacks.Any())
                        results.Add("Artist này chưa có feedback nào.");
                    else
                        foreach (var fb in topFeedbacks)
                        {
                            var serviceOption = serviceOptions.FirstOrDefault(so => so.Id == fb.ServiceOptionId);
                            var obj = new
                            {
                                FeedbackId = fb.Id,
                                User = fb.User.Name,
                                Content = fb.Content,
                                Rating = fb.Rating,
                                ServiceOptionId = fb.ServiceOptionId,
                                ArtistId = serviceOption?.ArtistId
                            };
                            results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
                        }
                }
                // Feedback của service
                else if (!string.IsNullOrEmpty(entity.ServiceType))
                {
                    var serviceOptions = await _unitOfWork.ServiceOptionRepository.FindAsync(
                        so => true,
                        q => q.Include(x => x.Service)
                    );
                    var serviceOptionIds = serviceOptions
                        .Where(so => so.Name.Contains(entity.ServiceType))
                        .Select(so => so.Id)
                        .ToList();

                    var feedbacks = await _unitOfWork.FeedbackRepository.FindAsync(
                        fb => true,
                        q => q.Include(x => x.User)
                    );
                    var topFeedbacks = feedbacks
                        .Where(fb => serviceOptionIds.Contains(fb.ServiceOptionId))
                        .OrderByDescending(fb => fb.CreateAt)
                        .Take(5)
                        .ToList();

                    if (!topFeedbacks.Any())
                        results.Add("Dịch vụ này chưa có feedback nào.");
                    else
                        foreach (var fb in topFeedbacks)
                        {
                            var serviceOption = serviceOptions.FirstOrDefault(so => so.Id == fb.ServiceOptionId);
                            var obj = new
                            {
                                FeedbackId = fb.Id,
                                User = fb.User.Name,
                                Content = fb.Content,
                                Rating = fb.Rating,
                                ServiceOptionId = fb.ServiceOptionId,
                                ArtistId = serviceOption?.ArtistId
                            };
                            results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
                        }
                }
                else
                {
                    results.Add("Vui lòng cung cấp tên artist hoặc tên dịch vụ để xem feedback.");
                }
            }

            // Nếu có location mà KHÔNG có artist/service (chỉ lấy thông tin artist/location)
            if (queryTypes.Contains("location") && !queryTypes.Contains("artist") && !queryTypes.Contains("service"))
            {
                var artists = await _unitOfWork.InformationArtistRepository.FindAsync(
                    ia => true,
                    q => q.Include(x => x.Area)
                          .Include(x => x.ArtistProgress).ThenInclude(ap => ap.Artist)
                );
                foreach (var art in artists.Where(a => a.Area != null).Take(5))
                {
                    var obj = new
                    {
                        ArtistId = art.ArtistProgress.Artist.Id,
                        Name = art.ArtistProgress.Artist.Name,
                        City = art.Area?.City,
                        District = art.Area?.District,
                        MinPrice = art.MinPrice,
                        MaxPrice = art.MaxPrice,
                        YearsOfExperience = art.YearsOfExperience
                    };
                    results.Add(System.Text.Json.JsonSerializer.Serialize(obj));
                }
            }

            if (queryTypes.Contains("policy"))
            {
                results.Add("Bạn có thể xem chi tiết chính sách hoàn/hủy dịch vụ tại trang Chính sách của hệ thống.");
            }
            if (queryTypes.Contains("event"))
            {
                results.Add("Các sự kiện nổi bật sẽ được cập nhật trên trang chủ hệ thống.");
            }

            if (!results.Any())
            {
                results.Add("Câu hỏi chưa đủ thông tin để tra cứu dữ liệu. Bạn có thể hỏi về dịch vụ, artist, feedback, location, policy, event...");
            }

            return results;
        }
    }
}
