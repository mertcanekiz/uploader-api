using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Uploader.Application.Images.Queries.GetImagesWithPagination;
using Uploader.Application.Images.Repositories;
using Uploader.Domain.Entities;

namespace Uploader.Application.UnitTests.Images.Queries.GetImagesWithPagination
{
    public class GetImagesWithPaginationQueryTest
    {
        private GetImagesWithPaginationQueryHandler _handler;
        private Mock<IImageRepository> _repository;
        private readonly Guid _testGuid = Guid.NewGuid();
        private List<Image> _testImages = new List<Image>();

        [SetUp]
        public void SetUp()
        {
            _testImages = new List<Image>
            {
                new Image()
            };
            _repository = new Mock<IImageRepository>();
            _repository.Setup(x =>
                    x.GetImagesPaginatedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(),
                        It.IsAny<CancellationToken>()))
                .ReturnsAsync((1, _testImages));
            _handler = new GetImagesWithPaginationQueryHandler(_repository.Object);
        }

        [Test]
        public async Task Handler_ShouldReturnPaginatedList_WhenGivenValidInput()
        {
            var query = new GetImagesWithPaginationQuery
            {
                PageNumber = 1,
                PageSize = 10,
                ShowDeleted = false
            };
            var result = await _handler.Handle(query, CancellationToken.None);
            Assert.AreEqual(_testImages.Count, result.TotalCount);
            Assert.AreEqual(_testImages, result.Items);
            Assert.AreEqual(query.PageNumber, result.PageIndex);
            Assert.False(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
        }
    }
}