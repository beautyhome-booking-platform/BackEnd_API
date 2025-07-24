using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Responses
{
    public class CreateFeedbackResponse : BaseResponse
    {
        public FeedbackDTO Feedback { get; set; }
    }
}
