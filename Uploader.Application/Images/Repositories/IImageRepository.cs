using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Uploader.Domain.Entities;

namespace Uploader.Application.Images.Repositories
{
    public interface IImageRepository
    {
        Task<Guid> CreateImageAsync(Image image, CancellationToken cancellationToken = default);
        Task<List<Image>> GetImagesAsync(CancellationToken cancellationToken = default);

        Task<(long count, IReadOnlyList<Image> data)>  GetImagesPaginatedAsync(int pageNumber, int pageSize,
            CancellationToken cancellationToken = default);
        Task<Image> GetImageById(Guid id, CancellationToken cancellationToken = default);

        Task<bool> UpdateImageDescriptionAsync(Guid id, string description,
            CancellationToken cancellationToken = default);
        Task<bool> DeleteImageAsync(Guid id, CancellationToken cancellationToken = default);
    }
}