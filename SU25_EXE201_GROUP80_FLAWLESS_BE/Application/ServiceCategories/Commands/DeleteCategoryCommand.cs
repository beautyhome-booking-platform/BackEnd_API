using Application.ServiceCategories.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceCategories.Commands
{
    public class DeleteCategoryCommand : IRequest<DeleteCategoryResponse>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
