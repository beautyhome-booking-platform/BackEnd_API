using Domain.DTO;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Responses
{
    public class GetListFeedbackResponse : BaseResponse
    {
        public List<FeedbackListDTO> Feedbacks { get; set; } = new List<FeedbackListDTO>();
        public string Message { get; set; }
    }
}
