using System;
using AspNetCore.Identity.Mongo.Model;

namespace Uploader.Infrastructure.Identity
{
    public class ApplicationUser : MongoUser<Guid>
    {
        
    }
}