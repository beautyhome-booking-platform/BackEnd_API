using Application.UserProgress.Commands;
using Application.UserProgress.Responses;
using Domain.Entities;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Handlers
{
    public class GetInformationCustomerHandler : IRequestHandler<GetInformationCustomerCommand, GetInformationCustomerResponse>
    {
        IUnitOfWork _unitOfWork;
        UserManager<UserApp> _userManager;
        public GetInformationCustomerHandler(IUnitOfWork unitOfWork, UserManager<UserApp> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<GetInformationCustomerResponse> Handle(GetInformationCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = new GetInformationCustomerResponse();

            // Step 1: Get all appointments with related data eager loaded
            var appointments = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Include(a => a.Customer)
                .Include(a => a.ArtistMakeup)
                .Include(a => a.AppointmentsDetails)
                    .ThenInclude(ad => ad.ServiceOption)
                        .ThenInclude(so => so.Service)
                .ToListAsync(cancellationToken);

            // Step 2: Group appointments by CustomerId
            var groupedByCustomer = appointments.GroupBy(a => a.CustomerId);

            var customersDto = new List<CustomerInfoDto>();

            foreach (var group in groupedByCustomer)
            {
                var customerAppointments = group.ToList();

                // Get user info from the first appointment's Customer navigation property
                var customer = customerAppointments.First().Customer;

                // Map Customer Info
                var customerDto = new CustomerInfoDto
                {
                    IdCustomer = customer.Id,
                    NameCustomer = customer.Name,
                    Avatar = customer.ImageUrl,
                    Gender = customer.Gender,
                    Phone = customer.PhoneNumber,
                    Email = customer.Email,
                    Address = customer.Address,
                    Note = customerAppointments.Select(a => a.Note).FirstOrDefault(),
                    Status = customerAppointments.Any(a => a.Status == AppointmentStatus.Confirmed) // example logic
                };

                // Map Artists (distinct)
                var artists = customerAppointments
                    .Select(a => a.ArtistMakeup)
                    .DistinctBy(artist => artist.Id) // Requires System.Linq or MoreLINQ
                    .Select(artist => new ArtistDto
                    {
                        Id = artist.Id,
                        Name = artist.Name
                    }).ToList();

                customerDto.Artists = artists;

                // Map Services from appointment details
                var services = customerAppointments
                    .SelectMany(a => a.AppointmentsDetails)
                    .Select(ad => new ServiceDto
                    {
                        Id = ad.ServiceOptionId.ToString(),
                        Name = ad.ServiceOption?.Name ?? "",
                        Description = ad.ServiceOption?.Description ?? "",
                        PaymentStatus = (int)ad.Appointment.Status, 
                        TimeBooking = ad.Appointment.AppointmentDate.ToString("yyyy-MM-dd HH:mm")
                    })
                    .ToList();

                customerDto.Services = services;

                customersDto.Add(customerDto);
            }

            response.Customers = customersDto;
            response.IsSuccess = true;
            return response;
        }
    }
}
