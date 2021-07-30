using System;
using System.Collections.Generic;
using FluentValidation.Results;
using NUnit.Framework;
using Uploader.Application.Common.Exceptions;

namespace Uploader.Application.UnitTests.Common.Exceptions
{
    public class ValidationExceptionTest
    {
        [Test]
        public void Constructor_ShouldCreateAnEmptyErrorDictionary()
        {
            var actual = new ValidationException().Errors;
            Assert.AreEqual(Array.Empty<string>(), actual.Keys);
        }

        [Test]
        public void Constructor_ShouldCreateASingleElementErrorDictionary_WhenCalledWithSingleValidationFailure()
        {
            var failures = new List<ValidationFailure>
            {
                new ValidationFailure("Description", "must not be empty")
            };
            var actual = new ValidationException(failures).Errors;
            Assert.AreEqual(new string[]{ "Description" }, actual.Keys);
            Assert.AreEqual(new string[]{ "must not be empty" }, actual["Description"]);
        }

        [Test]
        public void Constructor_ShouldCreateMultipleElementDictionary_WhenCalledWithMultipleValidationFailures()
        {
            var failures = new List<ValidationFailure>
            {
                new("Description", "must not be empty"),
                new("Description", "must contain at most 500 characters"),
                new("File", "must not be empty")
            };

            var actual = new ValidationException(failures).Errors;
            
            Assert.AreEqual(new string[]{ "Description", "File" }, actual.Keys);
            Assert.AreEqual(new string[]
            {
                "must not be empty",
                "must contain at most 500 characters"
            }, actual["Description"]);
            Assert.AreEqual(new string[]
            {
                "must not be empty"
            }, actual["File"]);
        }
    }
}