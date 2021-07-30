using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NUnit.Framework;
using Uploader.Application.Images.Commands.UpdateImageDescription;

namespace Uploader.Application.UnitTests.Images.Commands.UpdateImageDescription
{
    public class UpdateImageDescriptionCommandValidatorTest
    {
        private UpdateImageDescriptionValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new UpdateImageDescriptionValidator();
        }
        
        [Test]
        public async Task Validate_ShouldReturnTrue_WhenGivenValidInput()
        {
            var command = new UpdateImageDescriptionCommand
            {
                Id = Guid.NewGuid(),
                Description = "Test description"
            };
            var result = await _validator.ValidateAsync(command);
            Assert.True(result.IsValid);
        }

        [Test]
        public async Task Validate_ShouldReturnFalse_WhenGivenEmptyDescription()
        {
            var command = new UpdateImageDescriptionCommand
            {
                Id = Guid.NewGuid(),
                Description = string.Empty
            };
            var result = await _validator.ValidateAsync(command);
            Assert.False(result.IsValid);
        }

        [Test]
        public async Task Validate_ShouldReturnFalse_WhenGivenTooLongDescription()
        {
            var command = new UpdateImageDescriptionCommand
            {
                Id = Guid.NewGuid(),
                Description = TestHelper.GetUniqueString(600)
            };
            var result = await _validator.ValidateAsync(command);
            Assert.False(result.IsValid);
        }
    }
}