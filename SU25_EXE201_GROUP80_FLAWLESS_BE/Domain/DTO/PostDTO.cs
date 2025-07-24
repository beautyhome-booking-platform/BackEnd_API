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
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string  AuthorId { get; set; } // Id của người viết bài
        public string AuthorName { get; set; } // Tên người viết bài (nếu có, không thì lấy từ AuthorId)
        public string AuthorAvatarUrl { get; set; } // Ảnh đại diện của người viết bài (nếu có, không thì lấy từ AuthorId)
        public string Title { get; set; }   // Tiêu đề bài viết
        public string Content { get; set; } // Nội dung

        public string? ThumbnailUrl { get; set; } // Ảnh đại diện (nếu có)

        public string? Tags { get; set; }  // Từ khóa / Tags (dễ tìm kiếm)

        public Guid? ServiceOptionId { get; set; }
    }
}
