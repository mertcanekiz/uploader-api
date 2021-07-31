using System;
using Uploader.Domain.Common;

namespace Uploader.Domain.Entities
{
    public class User : TimestampedEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}