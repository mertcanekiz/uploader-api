using System;

namespace Uploader.Application.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeAttribute : Attribute
    {
        public AuthorizeAttribute()
        {
        }

        public string Roles { get; set; }
        public string Policy { get; set; }
    }
}