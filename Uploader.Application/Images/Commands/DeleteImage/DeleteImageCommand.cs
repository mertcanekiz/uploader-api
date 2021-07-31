using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Images.Repositories;
using Uploader.Domain.Entities;

namespace Uploader.Application.Images.Commands.DeleteImage
{
    public class DeleteImageCommand : IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
    {
        private readonly IImageRepository _imageRepository;

        public DeleteImageCommandHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<Unit> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            var result = await _imageRepository.DeleteImageAsync(request.Id, cancellationToken);

            if (result != true)
            {
                throw new NotFoundException(nameof(Image), request.Id);
            }

            return Unit.Value;
        }
    }
}