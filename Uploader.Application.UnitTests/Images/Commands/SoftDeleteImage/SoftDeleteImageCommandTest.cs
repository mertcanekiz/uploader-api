using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Images.Commands.SoftDeleteImage;
using Uploader.Application.Images.Repositories;

namespace Uploader.Application.UnitTests.Images.Commands.SoftDeleteImage
{
    public class SoftDeleteImageCommandTest
    {
        private SoftDeleteImageCommandHandler _handler;
        private Mock<IImageRepository> _repository;
        private readonly Guid _testGuid = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IImageRepository>();
            _repository.Setup(x => x.SoftDeleteImageAsync(_testGuid, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _repository.Setup(x => x.SoftDeleteImageAsync(It.IsNotIn(_testGuid), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _handler = new SoftDeleteImageCommandHandler(_repository.Object);
        }

        [Test]
        public async Task Handle_ShouldSoftDeleteImage_WhenGivenValidInput()
        {
            var command = new SoftDeleteImageCommand
            {
                Id = _testGuid
            };
            await _handler.Handle(command, CancellationToken.None);
            _repository.Verify(x => x.SoftDeleteImageAsync(_testGuid, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowError_WhenGivenInvalidId()
        {
            var command = new SoftDeleteImageCommand
            {
                Id = Guid.NewGuid()
            };
            Assert.ThrowsAsync(typeof(NotFoundException), async () =>
            {
                await _handler.Handle(command, CancellationToken.None);
            });
            _repository.Verify(x => x.SoftDeleteImageAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}