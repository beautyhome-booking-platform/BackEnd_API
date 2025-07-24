using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InformationArtist : BaseEntity
    {
        public Guid ArtistProgressId { get; set; }
        [ForeignKey(nameof(ArtistProgressId))]
        public ArtistProgress ArtistProgress { get; set; }

        public IList<string> CCCD { get; set; } = new List<string>(); // Lưu hình ảnh CCCD nếu có dạng chuỗi JSON

        public Guid? AreaId { get; set; }
        [ForeignKey(nameof(AreaId))]
        public Area Area { get; set; }
        public decimal MinPrice { get; set; }          // Giá thấp nhất
        public decimal MaxPrice { get; set; }          // Giá cao nhất

        // Chấp nhận điều khoản
        public bool TermsAccepted { get; set; }        // Checkbox chấp nhận điều khoản
        public DateTime? TermsAcceptedDate { get; set; } // Ngày chấp nhận điều khoản

        // Số năm kinh nghiệm
        public int YearsOfExperience { get; set; }     // Số năm kinh nghiệm

        // Album ảnh thực tế
        public IList<string> PortfolioImages { get; set; } = new List<string>(); // Lưu hình ảnh portfolio dạng chuỗi JSON

        // Nhận lịch gấp
        public bool AcceptUrgentBooking { get; set; } = false;  // Checkbox nhận lịch gấp

        // Loại mỹ phẩm thường dùng
        public string? CommonCosmetics { get; set; }   // Danh sách mỹ phẩm thường dùng 
        public ICollection<Service> InterestedServices { get; set; } = new List<Service>();
    }
}
