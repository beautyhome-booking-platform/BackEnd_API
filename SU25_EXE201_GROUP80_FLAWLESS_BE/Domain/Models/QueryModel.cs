using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class QueryModel
    {
        public string QueryType { get; set; }     // "artist", "service", "feedback", "policy", ...
        public string ServiceType { get; set; }   // VD: "makeup đám cưới"
        public string Location { get; set; }      // VD: "TP.HCM"
        public string PriceRange { get; set; }    // VD: "dưới 1 triệu"
        public string ArtistName { get; set; }    // VD: "Nguyễn Văn A"
        public bool? Feedback { get; set; }
    }
}
