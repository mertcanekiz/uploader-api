using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Uploader.Application.Images.Services
{
    public interface IS3Service
    {
        Task<string> UploadImage(IFormFile file);
    }
}