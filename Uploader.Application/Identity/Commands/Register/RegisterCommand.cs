using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uploader.Application.Common.Interfaces;
using Uploader.Application.Common.Models;

namespace Uploader.Application.Identity.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResult>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<RegisterResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var (result, userId) = await _identityService.CreateUserAsync(request.UserName, request.Password);

            if (!result.Succeeded)
            {
                throw new Exception(); // TODO
            }

            return new RegisterResult
            {
                UserId = userId
            };
        }
    }
}