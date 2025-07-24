using Application.UserAccount.Commands;
using Application.UserAccount.Responses;
using Domain.Entities;
using Infrastructure.Mail;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Handlers
{
    public class AcceptArtistHandler : IRequestHandler<AcceptArtistCommand, AcceptArtistResponse>
    {
        IUnitOfWork _unitOfWork;
        UserManager<UserApp> _userManager;
        IEmailSender _emailSender;

        public AcceptArtistHandler(IUnitOfWork unitOfWork, UserManager<UserApp> userManager, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
        }
        public async Task<AcceptArtistResponse> Handle(AcceptArtistCommand request, CancellationToken cancellationToken)
        {
            var response = new AcceptArtistResponse();

            if (string.IsNullOrEmpty(request.Id))
            {
                response.ErrorMessage = "User Id không được để trống";
                return response;
            }

            var user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                response.ErrorMessage = "User không tồn tại";
                return response;
            }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles.Contains("Artist"))
            {
                response.ErrorMessage = "User đã là Artist rồi";
                return response;
            }

            if (roles.Contains("Admin"))
            {
                response.ErrorMessage = "User đang ở role Admin, không thể đổi role";
                return response;
            }


            if (!request.Accept)
            {
                response.IsSuccess = false;
                response.Message = "User không được chấp nhận làm Artist, vui lòng cung cấp lại thông tin";

                if (request.Id != null)
                {
                    var artistProgress = (await _unitOfWork.ArtistProgressRepository.FindAsync(ap => ap.Artist.Id == request.Id)).FirstOrDefault();
                    if (artistProgress != null)
                    {
                        artistProgress.Note = Domain.Enum.RequestStatus.Rejected;  // Cập nhật trường note
                        _unitOfWork.ArtistProgressRepository.Update(artistProgress);
                        await _unitOfWork.SaveChangesAsync();
                    }
                }

                var subjectReject = "Đơn đăng ký Artist của bạn đã bị từ chối";
                var messageReject = $"Chào {user.UserName},<br/>" +
                                    "Rất tiếc, đơn đăng ký làm Artist của bạn không được chấp nhận.<br/>" +
                                    "Vui lòng kiểm tra và cung cấp lại thông tin cần thiết.";

                await _emailSender.SendEmailAsync(user.Email, subjectReject, messageReject);
            }
            else
            {
                if (Guid.TryParse(request.Id, out Guid userGuid))
                {
                    var addRoleResult = await _userManager.AddToRoleAsync(user, "Artist");
                    if (!addRoleResult.Succeeded)
                    {
                        response.ErrorMessage = "Thêm role Artist thất bại: " + string.Join(", ", addRoleResult.Errors.Select(e => e.Description));
                        return response;
                    }

                    if (await _userManager.IsInRoleAsync(user, "Customer"))
                    {
                        var removeRoleResult = await _userManager.RemoveFromRoleAsync(user, "Customer");
                        if (!removeRoleResult.Succeeded)
                        {
                            response.ErrorMessage = "Xóa role Customer thất bại: " + string.Join(", ", removeRoleResult.Errors.Select(e => e.Description));
                            return response;
                        }
                    }

                    var userProgress = (await _unitOfWork.UserProgressRepository
                        .FindAsync(up => up.User.Id == request.Id))
                        .FirstOrDefault();
                    if (userProgress != null)
                    {
                        _unitOfWork.UserProgressRepository.Remove(userProgress);
                    }

                    var artistProgress = (await _unitOfWork.ArtistProgressRepository
                        .FindAsync(ap => ap.Artist.Id == request.Id))
                        .FirstOrDefault();
                    if (artistProgress == null)
                    {
                        // Tạo mới ArtistProgress với IsActive = true
                        artistProgress = new ArtistProgress
                        {
                            Artist = user,
                            Points = 0,
                            Rank = Domain.Enum.ArtistRank.Begginer,
                            TotalCompletedAppointments = 0,
                            TotalCancellations = 0,
                            TotalReceive = 0,
                            LastRankUpgradeDate = null,
                            IsActive = true
                        };

                        _unitOfWork.ArtistProgressRepository.AddAsync(artistProgress);
                    }
                    else
                    {
                        // Nếu đã có thì bật IsActive lên true
                        artistProgress.IsActive = true;
                        artistProgress.Note = Domain.Enum.RequestStatus.Accepted;
                        _unitOfWork.ArtistProgressRepository.Update(artistProgress);
                    }

                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    response.ErrorMessage = "User Id không hợp lệ";
                    return response;
                }

                response.IsSuccess = true;
                response.Message = "User đã được chấp nhận làm Artist thành công";

                var subjectAccept = "Chúc mừng! Đơn đăng ký Artist của bạn đã được chấp nhận";
                var messageAccept = $"Chào {user.UserName},<br/>" +
                                    "Đơn đăng ký làm Artist của bạn đã được admin chấp nhận.<br/>" +
                                    "Bạn có thể bắt đầu nhận công việc ngay bây giờ!";

                await _emailSender.SendEmailAsync(user.Email, subjectAccept, messageAccept);
            }

            return response;


        }
    }
}
