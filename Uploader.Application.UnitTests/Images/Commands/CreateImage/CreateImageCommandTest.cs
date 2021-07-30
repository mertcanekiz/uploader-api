using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using NUnit.Framework;
using Uploader.Application.Images.Commands.CreateImage;
using Uploader.Application.Images.Repositories;
using Uploader.Application.Images.Services;
using Uploader.Domain.Entities;

namespace Uploader.Application.UnitTests.Images.Commands.CreateImage
{
    public class CreateImageCommandTest
    {
        private CreateImageCommandHandler _handler;
        private Mock<IImageRepository> _repository;
        private Mock<IS3Service> _s3Service;
        private readonly Guid _testGuid = Guid.NewGuid();
        private readonly string _testUrl = "http://example.com";

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IImageRepository>();
            _s3Service = new Mock<IS3Service>();
            _repository.Setup(x => x.CreateImageAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_testGuid);
            _s3Service.Setup(x => x.UploadImage(It.IsAny<IFormFile>()))
                .ReturnsAsync(_testUrl);
            _handler = new CreateImageCommandHandler(_repository.Object, _s3Service.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnCreatedGuid_WhenGivenValidInput()
        {
            var command = new CreateImageCommand
            {
                Description = "Test description",
                File = new FormFile(Stream.Null, 0, 0, "testFile", "testFile.jpg")
            };
            var result = await _handler.Handle(command, It.IsAny<CancellationToken>());
            _repository.Verify(x => x.CreateImageAsync(It.IsAny<Image>(), It.IsAny<CancellationToken>()), Times.Once);
            _s3Service.Verify(x => x.UploadImage(It.IsAny<IFormFile>()), Times.Once);
            Assert.AreEqual(_testGuid, result);
        }
    }
}