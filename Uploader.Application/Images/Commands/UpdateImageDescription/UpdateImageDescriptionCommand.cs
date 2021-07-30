using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Images.Repositories;
using Uploader.Domain.Entities;

namespace Uploader.Application.Images.Commands.UpdateImageDescription
{
    public class UpdateImageDescriptionCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
    
    public class UpdateImageDescriptionCommandHandler: IRequestHandler<UpdateImageDescriptionCommand>
    {
        private readonly IImageRepository _imageRepository;

        public UpdateImageDescriptionCommandHandler(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<Unit> Handle(UpdateImageDescriptionCommand request, CancellationToken cancellationToken)
        {
            var result = await _imageRepository.UpdateImageDescriptionAsync(request.Id, request.Description, cancellationToken);

            if (result != true)
            {
                throw new NotFoundException(nameof(Image), request.Id);
            }

            return Unit.Value;
        }
    }
}