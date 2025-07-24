using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entities
{
    public class Message : BaseEntity
    {
        [Required]
        public string SenderId { get; set; }

        [ForeignKey(nameof(SenderId))]
        public  UserApp Sender { get; set; }  // Liên kết với UserApp (Người gửi)

        [Required]
        public string ReceiverId { get; set; }

        [ForeignKey(nameof(ReceiverId))]
        public  UserApp Receiver { get; set; }  // Liên kết với UserApp (Người nhận)

        [Required]
        public string Content { get; set; }  // Nội dung tin nhắn

        public bool IsRead { get; set; } = false;  // Trạng thái đã đọc/chưa đọc

        public DateTime SentAt { get; set; } = DateTime.UtcNow;  // Thời gian gửi

        public DateTime? ReadAt { get; set; }  // Thời gian đọc tin nhắn (nếu đã đọc)

        public MessageStatus Status { get; set; } = MessageStatus.Sent;  // Trạng thái tin nhắn: Đã gửi, Đã đọc, Đã xóa

        public Guid ConversationId { get; set; }
        [ForeignKey(nameof(ConversationId))] 
        public  Conversation Conversation { get; set; }
    }
}

