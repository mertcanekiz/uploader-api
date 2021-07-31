using System;
using Uploader.Domain.Common;

namespace Uploader.Domain.Entities
{
    public class Image : TimestampedEntity
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
    }
}