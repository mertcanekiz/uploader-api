using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Uploader.Application.Images.Repositories;
using Uploader.Application.Images.Services;
using Uploader.Domain.Entities;

namespace Uploader.Application.Images.Commands.CreateImage
{
    public class CreateImageCommand : IRequest<Guid>
    {
        public IFormFile File { get; set; }
        public string Description { get; set; }
    }

    public class CreateImageCommandHandler : IRequestHandler<CreateImageCommand, Guid>
    {
        private readonly IImageRepository _imageRepository;
        private readonly IS3Service _s3Service;

        public CreateImageCommandHandler(IImageRepository imageRepository, IS3Service s3Service)
        {
            _imageRepository = imageRepository;
            _s3Service = s3Service;
        }

        public async Task<Guid> Handle(CreateImageCommand request, CancellationToken cancellationToken)
        {
            var url = await _s3Service.UploadImage(request.File);

            var image = new Image
            {
                Url = url,
                Description = request.Description
            };

            var createdId = await _imageRepository.CreateImageAsync(image, cancellationToken);
            return createdId;
        }
    }
}