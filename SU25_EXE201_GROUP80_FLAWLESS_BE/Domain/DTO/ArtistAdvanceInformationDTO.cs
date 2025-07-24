using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ArtistAdvanceInformationDTO
    {
        public Guid Id { get; set; }

        // CCCD Information
        public List<string>? CCCD { get; set; }

        // Qualification Information
        public List<string>? Qualification { get; set; }
        public string? DegreeName { get; set; }
        public string? Institution { get; set; }
        public string? Description { get; set; }

        // Area Information
        public Guid? AreaId { get; set; }
        public string? AreaCity { get; set; }
        public string? AreaDistrict { get; set; }

        // Price Range
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        // Terms and Experience
        public bool? TermsAccepted { get; set; }
        public DateTime? TermsAcceptedDate { get; set; }
        public int? YearsOfExperience { get; set; }

        // Portfolio Images
        public List<string>? PortfolioImages { get; set; }

        // Social Media Links
        public string? FacebookUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? TiktokUrl { get; set; }
        public string? YoutubeUrl { get; set; }

        // Urgent Booking
        public bool? AcceptUrgentBooking { get; set; }

        // Common Cosmetics
        public string? CommonCosmetics { get; set; }
    }
}
