using System;

namespace Uploader.Application.Common.Models
{
    public class LoginResult
    {
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}