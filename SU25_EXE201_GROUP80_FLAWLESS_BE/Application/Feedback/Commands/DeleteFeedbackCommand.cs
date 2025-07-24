using Application.Feedback.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Commands
{
    public class DeleteFeedbackCommand : IRequest<DeleteFeedbackResponse>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
