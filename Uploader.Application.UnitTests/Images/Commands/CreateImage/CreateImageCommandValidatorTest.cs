using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Internal;
using NUnit.Framework;
using Uploader.Application.Images.Commands.CreateImage;

namespace Uploader.Application.UnitTests.Images.Commands.CreateImage
{
    public class CreateImageCommandValidatorTest
    {
        private CreateImageCommandValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CreateImageCommandValidator();
        }
        
        [Test]
        public async Task CreateImageCommandValidator_ShouldBeValid_WhenGivenValidInput()
        {
            var command = new CreateImageCommand
            {
                Description = "Test description",
                File = new FormFile(Stream.Null, 0, 0, "testFile", "testFile.jpg")
            };
            var validationResult = await _validator.ValidateAsync(command);
            Assert.True(validationResult.IsValid);
        }

        [Test]
        public async Task CreateImageCommandValidator_ShouldReturnInvalid_WhenGivenEmptyDescription()
        {
            var command = new CreateImageCommand
            {
                Description = string.Empty,
                File = new FormFile(Stream.Null, 0, 0, "testFile", "testFile.jpg")
            };
            var validationResult = await _validator.ValidateAsync(command);
            Assert.False(validationResult.IsValid);
        }
        
        [Test]
        public async Task CreateImageCommandValidator_ShouldReturnInvalid_WhenGivenTooLongDescription()
        {
            var command = new CreateImageCommand
            {
                Description = TestHelper.GetUniqueString(600),
                File = new FormFile(Stream.Null, 0, 0, "testFile", "testFile.jpg")
            };
            var validationResult = await _validator.ValidateAsync(command);
            Assert.False(validationResult.IsValid);
        }

        [Test]
        public async Task CreateImageCommandValidator_ShouldReturnInvalid_WhenGivenEmptyFile()
        {
            var command = new CreateImageCommand
            {
                Description = string.Empty,
                File = null
            };
            var validationResult = await _validator.ValidateAsync(command);
            Assert.False(validationResult.IsValid);
        }
        
        [Test]
        public async Task CreateImageCommandValidator_ShouldReturnInvalid_WhenGivenInvalidFiletype()
        {
            var command = new CreateImageCommand
            {
                Description = string.Empty,
                File = new FormFile(Stream.Null, 0, 0, "testFile", "testFile.invalid")
            };
            var validationResult = await _validator.ValidateAsync(command);
            Assert.False(validationResult.IsValid);
        }
    }
}