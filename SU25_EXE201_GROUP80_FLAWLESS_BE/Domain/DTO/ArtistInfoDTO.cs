using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ArtistInfoDTO
    {
            public string? IdArtist { get; set; }                // Mã nghệ sĩ (CU-001)
            public string? NameArtist { get; set; }              // Tên nghệ sĩ
            public string? Avatar { get; set; }                  // Đường dẫn ảnh đại diện
            public string? Specialty { get; set; }               // Chuyên môn
            public int? Status { get; set; }                     // 0 = khóa, 1 = hoạt động
            public Gender? Gender { get; set; }                     // -1 = nữ, 0 = không rõ, 1 = nam
            public string? Phone { get; set; }
            public string? Email { get; set; }
            public string? Dob { get; set; }                     // Ngày sinh dạng yyyy/MM/dd
            public string RoleName { get; set; } = "Artist";
            public BankAccountDTO? BankAccount { get; set; }     // Thông tin ngân hàng

            public string? Address { get; set; }
            public string? AreaBook { get; set; }                // Thành phố hoặc quận hoạt động
            public string? Note { get; set; }
            public string? AboutArtist { get; set; }
            public string? TimeJoin { get; set; }                // Ngày tham gia

            public List<ServiceOptionDTO>? Services { get; set; } = new();
            public List<CertificateDTO>? Certificate { get; set; } = new();

            public int? ReviewCount { get; set; }
            public double? Rating { get; set; }                  // Điểm đánh giá trung bình (VD: 4.9)
            public string? Experience { get; set; }              // Ví dụ: "5 years"

            public decimal? TotalIncome { get; set; }
            public int? TotalBooked { get; set; }
            public int? TotalCancel { get; set; }

            public List<Scheduledto>? ScheduleList { get; set; } = new List<Scheduledto>();

    }
    public class Scheduledto
    {
        public CustomerDto? Customer { get; set; }
        public string? Service { get; set; }
        public string? Date { get; set; } // yyyy-MM-dd
        public string? Time { get; set; } // HH:mm
        public string? Duration { get; set; }
        public AvailabilityStatus Status { get; set; }
        public string? AppointmentId { get; set; }
    }

    public class CustomerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
    }
}
