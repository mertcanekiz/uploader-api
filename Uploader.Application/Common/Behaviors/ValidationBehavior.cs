using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using ValidationException = Uploader.Application.Common.Exceptions.ValidationException;

namespace Uploader.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults =
                    await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(x => x.Errors).Where(x => x != null).ToList();
                if (failures.Count != 0)
                    throw new ValidationException(failures);
            }

            return await next();
        }
    }
}