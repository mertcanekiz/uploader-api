using System.Linq;
using Microsoft.AspNetCore.Identity;
using Uploader.Application.Common.Models;

namespace Uploader.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded ? Result.Success() : Result.Failure(result.Errors.Select(x => x.Description));
        }
    }
}