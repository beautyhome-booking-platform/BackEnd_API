using Application.Post.Commands;
using Application.Post.Responses;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Handlers
{
    public class DeletePostHandler : IRequestHandler<DeletePostCommand, DeletePostResponse>
    {
        IUnitOfWork _unitOfWork;
        public DeletePostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<DeletePostResponse> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var response = new DeletePostResponse();

            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);
            if (post == null || post.IsDeleted)
            {
                response.ErrorMessage = "Post not found";
                return response;
            }

            post.IsDeleted = true;
            _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
            response.IsSuccess = true;
            return response;
        }
    }
}
