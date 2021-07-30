using System;

namespace Uploader.Domain.Entities
{
    public class Image
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}