using Application.Feedback.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Commands
{
    public class UpdateFeedbackCommand : IRequest<UpdateFeedbackResponse>
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; } = 0;
    }
}
