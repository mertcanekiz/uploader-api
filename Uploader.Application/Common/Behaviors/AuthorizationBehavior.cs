using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;
using MediatR;
using Uploader.Application.Common.Attributes;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Common.Interfaces;

namespace Uploader.Application.Common.Behaviors
{
    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public AuthorizationBehavior(ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            var authorizeAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>().ToList();
            if (authorizeAttributes.Any())
            {
                if (_currentUserService.UserId == null)
                {
                    throw new UnauthorizedAccessException();
                }

                var authorizeAttributesWithRoles =
                    authorizeAttributes.Where(x => !string.IsNullOrEmpty(x.Roles)).ToList();

                if (authorizeAttributesWithRoles.Any())
                {
                    var authorized = false;

                    foreach (var roles in authorizeAttributesWithRoles.Select(x => x.Roles.Split(',')))
                    {
                        foreach (var role in roles)
                        {
                            var isInRole =
                                await _identityService.IsInRoleAsync((Guid) _currentUserService.UserId, role);
                            if (!isInRole) continue;
                            authorized = true;
                            break;
                        }
                    }

                    if (!authorized)
                    {
                        throw new ForbiddenAccessException();
                    }
                }

                var authorizeAttributesWithPolicies =
                    authorizeAttributes.Where(x => !string.IsNullOrEmpty(x.Policy)).ToList();

                if (authorizeAttributesWithPolicies.Any())
                {
                    foreach (var policy in authorizeAttributesWithPolicies.Select(x => x.Policy))
                    {
                        var authorized =
                            await _identityService.AuthorizeAsync((Guid) _currentUserService.UserId, policy);

                        if (!authorized)
                        {
                            throw new ForbiddenAccessException();
                        }
                    }
                }
            }

            return await next();
        }
    }
}