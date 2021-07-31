using System;

namespace Uploader.Domain.Common
{
    public abstract class TimestampedEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}