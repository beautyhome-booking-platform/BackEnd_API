using Application.Post.Commands;
using Application.Post.Responses;
using Domain.DTO;
using Infrastructure.Storage;
using MediatR;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Handlers
{
    public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, UpdatePostResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClouddinaryStorage _clouddinaryStorage;

        public UpdatePostHandler(IUnitOfWork unitOfWork, IClouddinaryStorage clouddinaryStorage)
        {
            _unitOfWork = unitOfWork;
            _clouddinaryStorage = clouddinaryStorage;
        }
        public async Task<UpdatePostResponse> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var response = new UpdatePostResponse();

            // Tìm post theo Id
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);
            if (post == null || post.IsDeleted)
            {
                response.ErrorMessage = "Post not found";
                response.IsSuccess = false;
                return response;
            }

            // Update từng field nếu truyền lên
            if (!string.IsNullOrEmpty(request.Title))
                post.Title = request.Title;

            if (!string.IsNullOrEmpty(request.Tags))
                post.Tags = request.Tags;

            if (!string.IsNullOrEmpty(request.Content))
                post.Content = request.Content;

            if (request.ServiceOptionId.HasValue)
                post.ServiceOptionId = request.ServiceOptionId.Value;

            // Update ảnh nếu có
            if (request.ImageFile != null)
            {
                var imageUrl = await _clouddinaryStorage.UploadImageAsync(request.ImageFile);
                post.ThumbnailUrl = imageUrl;
            }

            _unitOfWork.PostRepository.Update(post);
            await _unitOfWork.SaveChangesAsync();
           
            response.IsSuccess = true;

            return response;
        }
    }
}
