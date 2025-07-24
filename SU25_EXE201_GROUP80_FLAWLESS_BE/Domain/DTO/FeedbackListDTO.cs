using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class FeedbackListDTO
    {
        public Guid Id { get; set; }
        public Customerdto Customer { get; set; }
        public Artistdto Artist { get; set; }
        public ServiceOptiondto ServiceOption { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime? DateTime { get; set; }
    }

    public class Customerdto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class Artistdto
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ServiceOptiondto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    
}
