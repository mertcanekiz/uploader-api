using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Images.Queries.GetImageById;
using Uploader.Application.Images.Repositories;
using Uploader.Domain.Entities;

namespace Uploader.Application.UnitTests.Images.Queries.GetImageById
{
    public class GetImageByIdQueryTest
    {
        private GetImageByIdQueryHandler _handler;
        private Mock<IImageRepository> _repository;
        private readonly Guid _testGuid = Guid.NewGuid();
        private Image _testImage;

        [SetUp]
        public void SetUp()
        {
            _testImage = new Image
            {
                Id = _testGuid,
                Description = "Test description",
                Url = "http://example.com",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                DeletedAt = null
            };
            _repository = new Mock<IImageRepository>();
            _repository.Setup(x => x.GetImageById(_testGuid, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_testImage);
            _handler = new GetImageByIdQueryHandler(_repository.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnSingleImage_WhenCalledWithValidId()
        {
            var query = new GetImageByIdQuery
            {
                Id = _testGuid
            };
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.AreEqual(_testGuid, result.Id);
        }

        [Test]
        public void Handle_ShouldThrowError_WhenCalledWithInvalidId()
        {
            var query = new GetImageByIdQuery
            {
                Id = Guid.NewGuid(),
            };
            Assert.ThrowsAsync(typeof(NotFoundException), async () =>
            {
                await _handler.Handle(query, CancellationToken.None);
            });
        }
    }
}