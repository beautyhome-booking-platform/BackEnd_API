using Domain.DTO;
using Domain.Enum;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserProgress.Responses
{
    public class GetInformationCustomerResponse : BaseResponse
    {
        public List<CustomerInfoDto> Customers { get; set; }
    }
    public class CustomerInfoDto
    {
        public string IdCustomer { get; set; }
        public string NameCustomer { get; set; }
        public string Avatar { get; set; }
        public string RoleName { get; set; } = "Customer"; // Default role name for customers
        public List<ArtistDto> Artists { get; set; }
        public List<ServiceDto> Services { get; set; }
        public bool Status { get; set; } = false;
        public Gender Gender { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string? Note { get; set; }
    }

    public class ArtistDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ServiceDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PaymentStatus { get; set; }
        public string TimeBooking { get; set; }
    }
}
