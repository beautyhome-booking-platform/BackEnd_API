using Application.Appointment.Commands;
using Application.Appointment.Responses;
using Domain.DTO;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Appointment.Handlers
{
    public class GetAppoinmentHandler : IRequestHandler<GetAppoinmentCommand, GetAppoinmentResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetAppoinmentHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetAppoinmentResponse> Handle(GetAppoinmentCommand request, CancellationToken cancellationToken)
        {
            var response = new GetAppoinmentResponse();

            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();
            var users = await _unitOfWork.UserAppRepository.GetAllAsync();
            var appointmentDetails = await _unitOfWork.AppointmentDetailRepository.GetAllAsync();
            var serviceOptions = await _unitOfWork.ServiceOptionRepository.GetAllAsync();

            // Nếu tất cả field đều null hoặc rỗng thì lấy hết
            bool isAllNull = !request.Id.HasValue
                && string.IsNullOrEmpty(request.UserId)
                && string.IsNullOrEmpty(request.ArtistId)
                && string.IsNullOrEmpty(request.Status);

            IEnumerable<Domain.Entities.Appointment> filteredAppointments = appointments;

            if (!isAllNull)
            {
                // Lọc theo các trường nếu có
                filteredAppointments = appointments.AsQueryable();

                if (request.Id.HasValue)
                    filteredAppointments = filteredAppointments.Where(a => a.Id == request.Id.Value);

                if (!string.IsNullOrEmpty(request.UserId))
                    filteredAppointments = filteredAppointments.Where(a => a.CustomerId == request.UserId);

                if (!string.IsNullOrEmpty(request.ArtistId))
                    filteredAppointments = filteredAppointments.Where(a => a.ArtistMakeupId == request.ArtistId);

                if (!string.IsNullOrEmpty(request.Status) && Enum.TryParse<Domain.Enum.AppointmentStatus>(request.Status, true, out var status))
                    filteredAppointments = filteredAppointments.Where(a => a.Status == status);
            }

            var result = filteredAppointments
                .ToList()
                .Select(a =>
                {
                    var customer = users.FirstOrDefault(u => u.Id == a.CustomerId);
                    var artist = users.FirstOrDefault(u => u.Id == a.ArtistMakeupId);

                    var details = appointmentDetails
                        .Where(d => d.AppointmentId == a.Id)
                        .Select(d =>
                        {
                            var serviceOption = serviceOptions.FirstOrDefault(s => s.Id == d.ServiceOptionId);
                            return new AppointmentDetailDTO
                            {
                                Id = d.Id,
                                ServiceOptionId = d.ServiceOptionId,
                                ServiceOptionName = serviceOption?.Name ?? "",
                                Quantity = d.Quantity,
                                Note = d.Note,
                                UnitPrice = d.UnitPrice
                            };
                        }).ToList();

                    return new AppointmentDTO
                    {
                        Id = a.Id,
                        CustomerId = a.CustomerId,
                        CustomerName = customer?.Name ?? "",
                        ImageUrlCustomer = customer?.ImageUrl,
                        ArtistMakeupId = a.ArtistMakeupId,
                        ArtistMakeupName = artist?.Name ?? "",
                        AppointmentDate = a.AppointmentDate,
                        Address = a.Address,
                        Note = a.Note,
                        Status = a.Status.ToString(),
                        VoucherId = a.VoucherId,
                        TotalAmount = a.TotalAmount,
                        TotalDiscount = a.TotalDiscount,
                        TotalAmountAfterDiscount = a.TotalAmountAfterDiscount,
                        DepositForApp = a.DepositForApp,
                        AmountToPayForArtist = a.AmountToPayForArtist,
                        AppointmentDetails = details
                    };
                }).ToList();

            response.Appointments = result;
            response.IsSuccess = true;
            return response;
        }
    }
}
