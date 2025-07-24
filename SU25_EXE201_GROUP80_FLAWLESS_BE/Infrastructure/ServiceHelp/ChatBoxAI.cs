    using Azure.Core;
    using Domain.Entities;
    using Microsoft.Extensions.Configuration;
    using OpenAI;
    using OpenAI.Chat;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading.Tasks;

    namespace Infrastructure.ServiceHelp
    {
        public class ChatBoxAI
        {
            private readonly string _apiKey;
            private readonly HttpClient _httpClient;

            public ChatBoxAI(IConfiguration config)
            {
                _apiKey = config["GeminiAI:ApiKey"];
                _httpClient = new HttpClient();
            }

            public async Task<string> AskAsync(string userMessage)
            {
                string systemPrompt = File.ReadAllText("AppKnowledge.txt");
                string fullPrompt = systemPrompt + ". " + userMessage;

                var contents = new[]
                {
                    new { parts = new[] { new { text = fullPrompt } } }
                };

                var requestBody = new { contents };
                var json = JsonSerializer.Serialize(requestBody);

                var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={_apiKey}";
                var response = await _httpClient.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return "Xin lỗi, hệ thống AI đang bận hoặc hết quota.";

                try
                {
                    using var doc = JsonDocument.Parse(content);
                    var text = doc.RootElement
                        .GetProperty("candidates")[0]
                        .GetProperty("content")
                        .GetProperty("parts")[0]
                        .GetProperty("text")
                        .GetString();
                    return text ?? "Xin lỗi, tôi không thể trả lời lúc này.";
                }
                catch
                {
                    return "Xin lỗi, không lấy được câu trả lời từ AI.";
                }
            }
        }
        }
