using Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ArtistDTO
    {
        // User Information
        public string Id { get; set; }
        public string Name { get; set; }
        public string TagName { get; set; }
        public Gender Gender { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }

        
        }
}
