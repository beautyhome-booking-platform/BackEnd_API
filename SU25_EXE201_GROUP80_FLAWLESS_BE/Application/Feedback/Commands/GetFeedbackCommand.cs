using Application.Feedback.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Commands
{
    public class GetFeedbackCommand : IRequest<GetFeedbackResponse>
    {
        public Guid? Id { get; set; }
        public string? UserId { get; set; }
        public string? ArtistId { get; set; }
        public Guid? ServiceOptionId { get; set; }
        public Guid? AppoinmentId { get; set; }
    }
}
