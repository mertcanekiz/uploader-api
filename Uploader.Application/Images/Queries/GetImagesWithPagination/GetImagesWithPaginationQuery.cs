using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uploader.Application.Common.Models;
using Uploader.Application.Images.Repositories;
using Uploader.Domain.Entities;

namespace Uploader.Application.Images.Queries.GetImagesWithPagination
{
    public class GetImagesWithPaginationQuery : IRequest<PaginatedList<Image>>
    {
        public bool ShowDeleted { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    
    public class GetImagesWithPaginationQueryHandler : IRequestHandler<GetImagesWithPaginationQuery, PaginatedList<Image>>
    {
        private readonly IImageRepository _imageRepository;

        public GetImagesWithPaginationQueryHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<PaginatedList<Image>> Handle(GetImagesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var (count, images) = await _imageRepository.GetImagesPaginatedAsync(request.PageNumber, request.PageSize, request.ShowDeleted,
                cancellationToken: cancellationToken);
            return new PaginatedList<Image>(images.ToList(), count, request.PageNumber, request.PageSize);
        }
    }
}