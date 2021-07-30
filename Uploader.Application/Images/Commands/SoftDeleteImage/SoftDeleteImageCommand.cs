using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Images.Repositories;
using Uploader.Domain.Entities;

namespace Uploader.Application.Images.Commands.SoftDeleteImage
{
    public class SoftDeleteImageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class SoftDeleteImageCommandHandler : IRequestHandler<SoftDeleteImageCommand>
    {
        private readonly IImageRepository _imageRepository;

        public SoftDeleteImageCommandHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<Unit> Handle(SoftDeleteImageCommand request, CancellationToken cancellationToken)
        {
            var result = await _imageRepository.SoftDeleteImageAsync(request.Id, cancellationToken);
            if (result != true)
            {
                throw new NotFoundException(nameof(Image), request.Id);
            }

            return Unit.Value;
        }
    }
}