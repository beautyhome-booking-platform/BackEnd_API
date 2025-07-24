using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.ServiceHelp
{
    public class ParseUserQuestionService
    {
        private readonly ChatBoxAI _chatBoxAI;

        public ParseUserQuestionService(ChatBoxAI chatBoxAI)
        {
            _chatBoxAI = chatBoxAI;
        }

        public async Task<QueryModel> ParseAsync(string userQuestion)
        {
            // Prompt không giới hạn số từ khóa
            var prompt = $"Phân tích câu hỏi sau và trả về đúng 1 object JSON với các trường sau (không được bỏ trường nào, nếu không có giá trị thì để null): - QueryType: 1 hoặc nhiều từ khóa (bằng tiếng Anh) phân cách bởi dấu phẩy, chọn từ: 'artist', 'service', 'feedback', 'location', 'policy', 'event'. - ServiceType: tên dịch vụ makeup nếu có (nếu không có thì để null). - Location: địa điểm nếu có (nếu không có thì để null). - PriceRange: khoảng giá nếu có (nếu không có thì để null). - ArtistName: tên artist nếu có (nếu không có thì để null). - Feedback: true/false nếu câu hỏi liên quan đến feedback, nếu không thì để null. Ví dụ: - Câu hỏi: 'Cho tôi các service của artist Hoang ở Hà Nội có feedback tốt' => {{ \"QueryType\": \"artist, service, location, feedback\", \"ServiceType\": null, \"Location\": \"Hà Nội\", \"PriceRange\": null, \"ArtistName\": \"Hoang\", \"Feedback\": true }} - Câu hỏi: 'Các service ở Hà Nội' => {{ \"QueryType\": \"location, service\", \"ServiceType\": null, \"Location\": \"Hà Nội\", \"PriceRange\": null, \"ArtistName\": null, \"Feedback\": null }} Chỉ trả về JSON đúng format, không giải thích gì thêm. Câu hỏi: {userQuestion}";
            var result = await _chatBoxAI.AskAsync(prompt);

            // Loại bỏ mọi thứ ngoài JSON
            var jsonStart = result.IndexOf('{');
            var jsonEnd = result.LastIndexOf('}');
            if (jsonStart < 0 || jsonEnd <= jsonStart)
                throw new Exception("Không tìm thấy JSON trong kết quả AI trả về: " + result);

            var json = result.Substring(jsonStart, jsonEnd - jsonStart + 1);

            // Làm phẳng chuỗi JSON, loại bỏ mọi xuống dòng, tab, thừa trắng
            json = json.Replace("\r", "")
                       .Replace("\n", "")
                       .Replace("\t", "")
                       .Trim();

            // Nếu AI trả về True/False hoa thì chuẩn hóa lại
            json = json.Replace("True", "true").Replace("False", "false");

            try
            {
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true // Cho phép dấu phẩy ở cuối
                };
                return JsonSerializer.Deserialize<QueryModel>(json, options);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi parse JSON từ AI: " + ex.Message + "\nContent: " + json);
            }
        }
    }
}
