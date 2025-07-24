using Application.Feedback.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Feedback.Commands
{
    public class GetListFeedbackCommand : IRequest<GetListFeedbackResponse>
    {
    }
}
