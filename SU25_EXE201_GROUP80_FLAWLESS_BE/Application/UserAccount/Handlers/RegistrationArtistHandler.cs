using Application.UserAccount.Commands;
using Application.UserAccount.Responses;
using Domain.DTO;
using Domain.Entities;
using Domain.Models;
using Infrastructure.Mail;
using Infrastructure.ServiceHelp;
using Infrastructure.Storage;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Application.UserAccount.Handlers
{
    public class RegistrationArtistHandler : IRequestHandler<RegistrationArtistCommand, RegistrationArtistResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<UserApp> _userManager;
        private readonly BlobStorage _blobStorage;
        private readonly IEmailSender _emailSender;
        private readonly IClouddinaryStorage _clouddinaryStorage;
        IHubContext<NotificationHub> _hubContext;

        public RegistrationArtistHandler(IUnitOfWork unitOfWork, UserManager<UserApp> userManager, BlobStorage blobStorage, IEmailSender emailSender, IClouddinaryStorage clouddinaryStorage, IHubContext<NotificationHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _blobStorage = blobStorage;
            _emailSender = emailSender;
            _clouddinaryStorage = clouddinaryStorage;
            _hubContext = hubContext;
        }

        public async Task<RegistrationArtistResponse> Handle(RegistrationArtistCommand request, CancellationToken cancellationToken)
        {
            var response = new RegistrationArtistResponse();

            // Validate user exists and has Customer role
            var user = await _unitOfWork.UserAppRepository.FindAsync(x => x.Id == request.UserId);
            var singleUser = user.FirstOrDefault();
            if (singleUser == null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User not found";
                return response;
            }

            var isCustomer = await _userManager.IsInRoleAsync(singleUser, "Customer");
            if (!isCustomer)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User does not have the 'Customer' role";
                return response;
            }

            // Check if user already has a pending registration
            var waitConfirm = await _unitOfWork.ArtistProgressRepository.FindAsync(ap => ap.Artist.Id == request.UserId);
            var waitConfirmUser = waitConfirm.FirstOrDefault();
            if (waitConfirmUser != null)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "User has been register artist makeup, wait confirm from admin";
                return response;
            }

            // Upload CCCD images
            var cccdUrls = new List<string>();
            try
            {
                var frontUrl = await _clouddinaryStorage.UploadImageAsync(request.CCCDFrontImage);
                var backUrl = await _clouddinaryStorage.UploadImageAsync(request.CCCDBackImage);
                cccdUrls.Add(frontUrl);
                cccdUrls.Add(backUrl);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error uploading CCCD images: {ex.Message}";
                return response;
            }


            // Upload portfolio images
            var portfolioUrls = new List<string>();
            try
            {
                foreach (var file in request.PortfolioImages)
                {
                    var imageUrl = await _clouddinaryStorage.UploadImageAsync(file);
                    portfolioUrls.Add(imageUrl);
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Error uploading portfolio images: {ex.Message}";
                return response;
            }

            var interestedCategories = await _unitOfWork.ServiceRepository
                .FindAsync(sc => request.InterestedServiceIds.Contains(sc.Id));
            if(interestedCategories.Count() <= 0)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Interested service categories not found";
                return response;
            }

            // Create ArtistProgress
            var artistProgress = new ArtistProgress
            {
                Artist = singleUser,
                Points = 0,
                Rank = Domain.Enum.ArtistRank.Begginer,
                TotalCompletedAppointments = 0,
                TotalCancellations = 0,
                TotalReceive = 0,
                LastRankUpgradeDate = null,
                IsActive = false,
                Note = Domain.Enum.RequestStatus.Requested
            };
            _unitOfWork.ArtistProgressRepository.AddAsync(artistProgress);

            // Create InformationArtist
            var informationArtist = new InformationArtist
            {
                ArtistProgress = artistProgress,
                CCCD = cccdUrls,
                AreaId = request.AreaId,
                MinPrice = request.MinPrice,
                MaxPrice = request.MaxPrice,
                TermsAccepted = request.TermsAccepted,
                TermsAcceptedDate = request.TermsAccepted ? DateTime.UtcNow : null,
                YearsOfExperience = request.YearsOfExperience,
                PortfolioImages = portfolioUrls,
                AcceptUrgentBooking = request.AcceptUrgentBooking,
                CommonCosmetics = request.CommonCosmetics,
                InterestedServices = interestedCategories.ToList()
            };
            _unitOfWork.InformationArtistRepository.AddAsync(informationArtist);

            List<CertificateDTO> certificates;
            try
            {
                certificates = JsonConvert.DeserializeObject<List<CertificateDTO>>(request.CertificateJson);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"Invalid Certificate JSON format: {ex.Message}";
                return response;
            }

            foreach (var certDto in certificates)
            {
                var certificate = new Domain.Entities.Certificate
                {
                    ImageUrl = certDto.ImageUrl,
                    DegreeName = certDto.Name,
                    Institution = certDto.Institution,
                    Description = certDto.Description ?? string.Empty,
                    InformationArtistId = informationArtist.Id,
                    InformationArtistDetails = informationArtist,
                };
                _unitOfWork.CertificateRepository.AddAsync(certificate);
            }
            await _unitOfWork.SaveChangesAsync();

            // Send confirmation email
            var subject = "Đơn đăng ký Artist của bạn đã được gửi";
            var message = $"Chào {singleUser.UserName},<br/>" +
                          $"Đơn đăng ký làm Artist của bạn đã được gửi thành công và đang chờ admin xét duyệt.<br/>" +
                          "Cảm ơn bạn đã tham gia!";

            await _emailSender.SendEmailAsync(singleUser.Email, subject, message);

            response.IsSuccess = true;
            response.RegistrationResult = "Đơn đăng ký của bạn đã được gửi, đang chờ xét duyệt! Cảm ơn bạn đã tham gia";

            return response;

        }
    }
}
