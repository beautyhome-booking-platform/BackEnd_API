using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Seed_Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // TP.HCM
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "City", "District", "Description", "IsActive", "CreateAt", "IsDeleted" },
                values: new object[,]
                {
            { Guid.NewGuid(), "TP.HCM", "Quận 1", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 2", "Quận Thủ Đức cũ", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 3", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 4", "Quận ven sông", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 5", "Quận Chợ Lớn", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 6", "Quận Chợ Lớn", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 7", "Quận Phú Mỹ Hưng", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 8", "Quận ven sông", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 9", "Quận Thủ Đức cũ", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 10", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 11", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận 12", "Quận ngoại thành", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận Bình Thạnh", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận Gò Vấp", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận Phú Nhuận", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận Tân Bình", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận Tân Phú", "Quận trung tâm", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Quận Bình Tân", "Quận ngoại thành", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Huyện Bình Chánh", "Huyện ngoại thành", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Huyện Cần Giờ", "Huyện biển", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Huyện Củ Chi", "Huyện ngoại thành", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Huyện Hóc Môn", "Huyện ngoại thành", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Huyện Nhà Bè", "Huyện ven sông", true, DateTime.Now, false },
            { Guid.NewGuid(), "TP.HCM", "Thành phố Thủ Đức", "Thành phố mới", true, DateTime.Now, false }
                });

            // Hà Nội
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "City", "District", "Description", "IsActive", "CreateAt", "IsDeleted" },
                values: new object[,]
                {
            { Guid.NewGuid(), "Hà Nội", "Quận Ba Đình", "Quận trung tâm",       true, DateTime.Now, false },
            { Guid.NewGuid(), "Hà Nội", "Quận Hoàn Kiếm", "Quận trung tâm",     true, DateTime.Now, false },
            { Guid.NewGuid(), "Hà Nội", "Quận Đống Đa", "Quận trung tâm",       true, DateTime.Now, false },
            { Guid.NewGuid(), "Hà Nội", "Quận Hai Bà Trưng", "Quận trung tâm",  true, DateTime.Now, false },
            { Guid.NewGuid(), "Hà Nội", "Quận Cầu Giấy", "Quận trung tâm",      true, DateTime.Now, false },
            { Guid.NewGuid(), "Hà Nội", "Quận Thanh Xuân", "Quận trung tâm",    true, DateTime.Now, false },
            { Guid.NewGuid(), "Hà Nội", "Quận Hoàng Mai", "Quận trung tâm",     true, DateTime.Now, false },
            { Guid.NewGuid(), "Hà Nội", "Quận Long Biên", "Quận ven sông",      true, DateTime.Now, false },
            { Guid.NewGuid(), "Hà Nội", "Quận Tây Hồ", "Quận ven hồ",           true, DateTime.Now, false }
                });

            // Đà Nẵng
            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "City", "District", "Description", "IsActive", "CreateAt", "IsDeleted" },
                values: new object[,]
                {
            { Guid.NewGuid(), "Đà Nẵng", "Quận Hải Châu", "Quận trung tâm",     true, DateTime.Now, false },
            { Guid.NewGuid(), "Đà Nẵng", "Quận Thanh Khê", "Quận trung tâm",    true, DateTime.Now, false },
            { Guid.NewGuid(), "Đà Nẵng", "Quận Sơn Trà", "Quận ven biển",       true, DateTime.Now, false },
            { Guid.NewGuid(), "Đà Nẵng", "Quận Ngũ Hành Sơn", "Quận ven biển",  true, DateTime.Now, false },
            { Guid.NewGuid(), "Đà Nẵng", "Quận Liên Chiểu", "Quận công nghiệp", true, DateTime.Now, false },
            { Guid.NewGuid(), "Đà Nẵng", "Quận Cẩm Lệ", "Quận nội thành",       true, DateTime.Now, false }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "City",
                keyValues: new object[] { "TP.HCM", "Hà Nội", "Đà Nẵng" });
        }
    }
}
