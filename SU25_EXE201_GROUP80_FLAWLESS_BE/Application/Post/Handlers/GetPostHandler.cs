using Application.Post.Commands;
using Application.Post.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Handlers
{
    public class GetPostHandler : IRequestHandler<GetPostCommand, GetPostResponse>
    {
        IUnitOfWork _unitOfWork;
        public GetPostHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetPostResponse> Handle(GetPostCommand request, CancellationToken cancellationToken)
        {
            var response = new GetPostResponse();

            var posts = await _unitOfWork.PostRepository.FindAsync(
                            p =>
                        (p.Id == request.Id || request.Id == null) && (!p.IsDeleted) &&
                        (string.IsNullOrEmpty(request.SearchContent) ||
                         p.Title.Contains(request.SearchContent) ||
                         p.Content.Contains(request.SearchContent) ||
                         p.Author.Name.Contains(request.SearchContent) ||
                         p.Tags.Contains(request.SearchContent)) &&
                        (string.IsNullOrEmpty(request.AuthorId) || p.Author.Id == request.AuthorId),
                         query => query.Include(p => p.Author) 
);

            if (posts.Count() > 0)
            {
                var postDTOs = posts.Select(p => new Domain.DTO.PostDTO
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    AuthorId = p.Author.Id,
                    AuthorName = p.Author.Name,
                    Tags = p.Tags,
                    AuthorAvatarUrl = p.Author.ImageUrl,
                    ServiceOptionId = p.ServiceOptionId,
                    ThumbnailUrl = p.ThumbnailUrl,
                }).ToList();
                response.Posts = postDTOs;
                response.IsSuccess = true;
            }
            else
            {
                response.ErrorMessage = "Post not found or not have any posts";
                return response;
            }

            return response;
        }
    }
}
