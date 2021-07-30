using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Images.Repositories;
using Uploader.Domain.Entities;

namespace Uploader.Application.Images.Queries.GetImageById
{
    public class GetImageByIdQuery : IRequest<Image>
    {
        public Guid Id { get; set; }
    }
    
    public class GetImageByIdQueryHandler : IRequestHandler<GetImageByIdQuery, Image>
    {
        private readonly IImageRepository _imageRepository;

        public GetImageByIdQueryHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<Image> Handle(GetImageByIdQuery request, CancellationToken cancellationToken)
        {
            var image = await _imageRepository.GetImageById(request.Id, cancellationToken);

            if (image == null)
            {
                throw new NotFoundException(nameof(Image), request.Id);
            }

            return image;
        }
    }
}