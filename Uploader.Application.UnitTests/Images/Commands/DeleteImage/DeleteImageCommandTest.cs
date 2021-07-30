using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Images.Commands.DeleteImage;
using Uploader.Application.Images.Repositories;

namespace Uploader.Application.UnitTests.Images.Commands.DeleteImage
{
    public class DeleteImageCommandTest
    {
        private DeleteImageCommandHandler _handler;
        private Mock<IImageRepository> _repository;
        private readonly Guid _testGuid = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IImageRepository>();
            _repository.Setup(x => x.DeleteImageAsync(_testGuid, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _repository.Setup(x => x.DeleteImageAsync(It.IsNotIn(_testGuid), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _handler = new DeleteImageCommandHandler(_repository.Object);
        }

        [Test]
        public async Task Handle_ShouldSoftDeleteImage_WhenGivenValidInput()
        {
            var command = new DeleteImageCommand
            {
                Id = _testGuid
            };
            await _handler.Handle(command, CancellationToken.None);
            _repository.Verify(x => x.DeleteImageAsync(_testGuid, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public void Handle_ShouldThrowError_WhenGivenInvalidId()
        {
            var command = new DeleteImageCommand
            {
                Id = Guid.NewGuid()
            };
            Assert.ThrowsAsync(typeof(NotFoundException), async () =>
            {
                await _handler.Handle(command, CancellationToken.None);
            });
            _repository.Verify(x => x.DeleteImageAsync(command.Id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}