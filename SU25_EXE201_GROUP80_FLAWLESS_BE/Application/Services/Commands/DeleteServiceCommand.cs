﻿using Application.Services.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Commands
{
    public class DeleteServiceCommand : IRequest<DeleteServiceResponse>
    {
        [Required]
        public Guid Id { get; set; }
    }
}
