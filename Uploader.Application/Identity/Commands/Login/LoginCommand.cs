using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Uploader.Application.Common.Exceptions;
using Uploader.Application.Common.Interfaces;
using Uploader.Application.Common.Models;

namespace Uploader.Application.Identity.Commands.Login
{
    public class LoginCommand : IRequest<LoginResult>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IIdentityService _identityService;

        public LoginCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var (result, loginResult) = await _identityService.LoginAsync(request.UserName, request.Password);
            
            if (!result.Succeeded)
            {
                throw new UnauthorizedException();
            }

            return loginResult;
        }
    }
}