using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class FeedbackDTO
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public Guid ServiceOptionId { get; set; }
        public Guid AppoinmentId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }
}
