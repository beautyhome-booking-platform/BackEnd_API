using Application.UserService.Commands;
using Application.UserService.Response;
using Domain.Entities;
using Infrastructure.ServiceHelp;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.UserService.Handlers
{
    public class AskChatHandler : IRequestHandler<AskChatCommand, AskChatResponse>
    {
        private readonly ChatBoxAI _chatBoxAI;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ParseUserQuestionService _parseUserQuestionService;

        public AskChatHandler(ChatBoxAI chatBoxAI, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor,
                ParseUserQuestionService parseUserQuestionService)
        {
            _chatBoxAI = chatBoxAI;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _parseUserQuestionService = parseUserQuestionService;
        }

        public async Task<AskChatResponse> Handle(AskChatCommand request, CancellationToken cancellationToken)
        {
            var response = new AskChatResponse();

            if (string.IsNullOrEmpty(request.Message))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Message không được để trống!";
                return response;
            }
            var httpContext = _httpContextAccessor.HttpContext;
            var userId = httpContext?.User?.FindFirst("sub")?.Value
                    ?? httpContext?.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                response.IsSuccess = false;
                response.ErrorMessage = "Không xác định được user!";
                return response;
            }

            var historyList = (await _unitOfWork.ChatBoxAiRepository
                .FindAsync(cb => cb.UserId == userId))
                .OrderByDescending(x => x.CreateAt)
                .Take(5)
                .OrderBy(x => x.CreateAt)
                .Select(h => (h.UserMessage, h.AIReply))
                .ToList();

            var parser = new ParseUserQuestionService(_chatBoxAI);
            var entity = await parser.ParseAsync(request.Message);

            var dataGetter = new GetDataByQueryModel(_unitOfWork);
            var contextList = await dataGetter.GetData(entity);
            var promptBuilder = new StringBuilder();

            if (contextList != null && contextList.Any())
            {
                promptBuilder.AppendLine("Dữ liệu liên quan:");
                foreach (var ctx in contextList)
                    promptBuilder.AppendLine(ctx);
            }

            if (historyList != null && historyList.Any())
            {
                promptBuilder.AppendLine("Lịch sử hội thoại:");
                foreach (var (user, ai) in historyList)
                {
                    if (!string.IsNullOrWhiteSpace(user))
                        promptBuilder.AppendLine("User: " + user);
                    if (!string.IsNullOrWhiteSpace(ai))
                        promptBuilder.AppendLine("AI: " + ai);
                }
            }

            promptBuilder.AppendLine("Câu hỏi:");
            promptBuilder.AppendLine(request.Message);

            var aiReply = await _chatBoxAI.AskAsync(
                promptBuilder.ToString()
            );

            var chatLog = new ChatHistory
            {
                UserId = userId,
                UserMessage = request.Message,
                AIReply = aiReply,
            };
            _unitOfWork.ChatBoxAiRepository.AddAsync(chatLog);
            await _unitOfWork.SaveChangesAsync();

            //if (string.IsNullOrWhiteSpace(request.Message))
            //{
            //    response.IsSuccess = false;
            //    response.ErrorMessage = "Tin nhắn không được để trống.";
            //    return response;
            //}

            //var aiReply = await _chatBoxAI.AskAsync(request.Message);

            response.IsSuccess = true;
            response.Reply = aiReply;



            return response;
        }
    }
}
