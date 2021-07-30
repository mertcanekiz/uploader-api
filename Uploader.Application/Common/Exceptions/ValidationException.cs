using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Uploader.Application.Common.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(x => x.PropertyName, x => x.ErrorMessage)
                .ToDictionary(x => x.Key, x => x.ToArray());
        }
        
        public IDictionary<string, string[]> Errors { get; }
    }
}