using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Images.Commands.UpdateImageDescription;
using Uploader.Application.Images.Repositories;

namespace Uploader.Application.UnitTests.Images.Commands.UpdateImageDescription
{
    public class UpdateImageDescriptionCommandHandlerTest
    {
        private UpdateImageDescriptionCommandHandler _handler;
        private Mock<IImageRepository> _repository;
        private readonly Guid _testGuid = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<IImageRepository>();
            _repository.Setup(x =>
                    x.UpdateImageDescriptionAsync(_testGuid, It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _repository.Setup(x =>
                    x.UpdateImageDescriptionAsync(It.IsNotIn(_testGuid), It.IsAny<string>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _handler = new UpdateImageDescriptionCommandHandler(_repository.Object);
        }
        
        [Test]
        public async Task Handle_ShouldUpdateImage_WhenGivenValidInput()
        {
            var command = new UpdateImageDescriptionCommand
            {
                Id = _testGuid,
                Description = "Test description"
            };

            var result = await _handler.Handle(command, It.IsAny<CancellationToken>());
            
            _repository.Verify(x => x.UpdateImageDescriptionAsync(_testGuid, It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task Handle_ShouldThrowError_WhenGivenInvalidId()
        {
            var command = new UpdateImageDescriptionCommand
            {
                Id = Guid.NewGuid(),
                Description = "Test description"
            };

            Assert.ThrowsAsync(typeof(NotFoundException), async () =>
            {
                await _handler.Handle(command, CancellationToken.None);
            });
        }
    }
}