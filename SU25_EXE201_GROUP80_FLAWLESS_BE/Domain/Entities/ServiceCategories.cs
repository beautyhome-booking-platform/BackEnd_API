﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ServiceCategories : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public  ICollection<Service> Services { get; set; } = new List<Service>();

    }
}
     
        
