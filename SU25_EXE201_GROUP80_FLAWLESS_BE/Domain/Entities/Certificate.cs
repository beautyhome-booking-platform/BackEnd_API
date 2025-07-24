using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Certificate : BaseEntity
    {
        public string ImageUrl { get; set; }          // Đường dẫn đến ảnh chứng chỉ
        public string DegreeName { get; set; }         // Tên bằng cấp (VD: Chuyên viên trang điểm)
        public string Institution { get; set; }        // Tên trường, nơi cấp
        public string? Description { get; set; }        // Mô tả thêm (nếu cần)
        public Guid InformationArtistId { get; set; }    // Khóa ngoại đến thông tin nghệ sĩ
        [ForeignKey(nameof(InformationArtistId))]
        public InformationArtist InformationArtistDetails { get; set; }  // Thông tin nghệ sĩ liên kết
    }
}
