using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Post : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }   // Tiêu đề bài viết

        [Required]
        public string Content { get; set; } // Nội dung

        [MaxLength(300)]
        public string? ThumbnailUrl { get; set; } // Ảnh đại diện (nếu có)


        [ForeignKey("AuthorId")]
        public UserApp Author { get; set; } // Thông tin người viết

        [MaxLength(500)]
        public string? Tags { get; set; }  // Từ khóa / Tags (dễ tìm kiếm)

        public Guid? ServiceOptionId { get; set; }
        [ForeignKey(nameof(ServiceOptionId))]
        public ServiceOption? ServiceOption { get; set; }
    }
}
