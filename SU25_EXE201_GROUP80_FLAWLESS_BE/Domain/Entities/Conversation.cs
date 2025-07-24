using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Conversation : BaseEntity
    {
        [Required]
        public string User1Id { get; set; }

        [ForeignKey(nameof(User1Id))] 
        public  UserApp User1 { get; set; }  // Liên kết với UserApp (Người tham gia 1)

        [Required]
        public string User2Id { get; set; }

        [ForeignKey(nameof(User2Id))]
        public  UserApp User2 { get; set; }  // Liên kết với UserApp (Người tham gia 2)

        public DateTime LastMessageTime { get; set; }  // Thời gian của tin nhắn cuối cùng

        public bool IsArchived { get; set; } = false;  // Đánh dấu cuộc trò chuyện đã lưu trữ hay chưa

        public ICollection<Message> Messages { get; set; } = new List<Message>();  // Các tin nhắn trong cuộc trò chuyện
    }
}
