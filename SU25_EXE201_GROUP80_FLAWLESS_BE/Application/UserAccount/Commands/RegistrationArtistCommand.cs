using Application.UserAccount.Responses;
using Domain.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserAccount.Commands
{
    public class RegistrationArtistCommand : IRequest<RegistrationArtistResponse>
    {
        [Required]
        public string UserId { get; set; }

        // CCCD Information
        [Required]
        public IFormFile CCCDFrontImage { get; set; }
        [Required]
        public IFormFile CCCDBackImage { get; set; }

        [Required]
        public string CertificateJson { get; set; }

        // Area Information
        [Required]
        public Guid AreaId { get; set; }

        // Price Range
        [Required]
        public decimal MinPrice { get; set; }
        [Required]
        public decimal MaxPrice { get; set; }

        // Terms and Experience
        [Required]
        public bool TermsAccepted { get; set; }
        [Required]
        public int YearsOfExperience { get; set; }

        // Portfolio Images
        [Required]
        public List<IFormFile> PortfolioImages { get; set; }


        // Urgent Booking
        [Required]
        public bool AcceptUrgentBooking { get; set; }

        // Common Cosmetics
        public string? CommonCosmetics { get; set; }

        public List<Guid> InterestedServiceIds { get; set; } = new List<Guid>();

    }

    public class CertificateRequest()
    {
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public string Institution { get; set; }
        public string Description { get; set; }

    }
}
