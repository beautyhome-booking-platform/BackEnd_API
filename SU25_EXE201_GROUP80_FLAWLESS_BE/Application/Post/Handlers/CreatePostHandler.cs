using Application.Post.Commands;
using Application.Post.Responses;
using Domain.DTO;
using Domain.Entities;
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
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, CreatePostResponse>
    {
        IUnitOfWork _unitOfWork;
        BlobStorage _blobStorage;
        IClouddinaryStorage _clouddinaryStorage;
        public CreatePostHandler(IUnitOfWork unitOfWork, BlobStorage blobStorage, IClouddinaryStorage clouddinary)
        {
            _unitOfWork = unitOfWork;
            _blobStorage = blobStorage;
            _clouddinaryStorage = clouddinary;
        }
        public async Task<CreatePostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var response = new CreatePostResponse();

            var imageUrl = await _clouddinaryStorage.UploadImageAsync(request.ImageFile);

            var serviceOption = await _unitOfWork.ServiceOptionRepository.GetByIdAsync((Guid) request.ServiceOptionId);

                var user = (await _unitOfWork.UserAppRepository.FindAsync(u => u.Id == request.AuthorId)).FirstOrDefault();

                var post = new Domain.Entities.Post
                {
                    Title = request.Title,
                    Content = request.Content,
                    Author = user,
                    ThumbnailUrl = imageUrl,
                    Tags = request.Tags,
                    ServiceOptionId = request.ServiceOptionId,
                    ServiceOption = serviceOption
                };

                if (post != null)
                {
                    _unitOfWork.PostRepository.AddAsync(post);
                    await _unitOfWork.SaveChangesAsync();
                    response.Post = new PostDTO
                    {
                        Id = post.Id,
                        Title = post.Title,
                        Content = post.Content,
                        AuthorId = post.Author.Id,
                        ThumbnailUrl = post.ThumbnailUrl,
                        Tags = post.Tags,
                        ServiceOptionId = post.ServiceOptionId,
                        AuthorAvatarUrl = user.ImageUrl,
                        AuthorName = user.Name,
                    };
                    response.IsSuccess = true;
                }
            



            return response;
        }
    }
}
