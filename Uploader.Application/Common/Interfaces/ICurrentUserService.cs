using System;

namespace Uploader.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
    }
}