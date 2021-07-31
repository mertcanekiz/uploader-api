using System;
using System.Threading.Tasks;
using Uploader.Application.Common.Models;

namespace Uploader.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUsernameAsync(Guid userId);
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<bool> AuthorizeAsync(Guid userId, string policyName);
        Task<(Result result, Guid userId)> CreateUserAsync(string username, string password);
        Task<(Result result, LoginResult loginResult)> LoginAsync(string username, string password);
        Task<Result> DeleteUserAsync(Guid userId);
    }
}