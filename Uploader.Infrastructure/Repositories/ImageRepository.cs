using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;
using Uploader.Application.Images.Repositories;
using Uploader.Domain.Entities;

namespace Uploader.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly IMongoCollection<Image> _collection;

        public ImageRepository(IMongoClient client)
        {
            _collection = client.GetDatabase("uploader").GetCollection<Image>("images");
        }

        public async Task<Guid> CreateImageAsync(Image image, CancellationToken cancellationToken = default)
        {
            if (image.Id == Guid.Empty)
            {
                image.Id = Guid.NewGuid();
            }
            image.CreatedAt = DateTime.Now;
            image.UpdatedAt = DateTime.Now;
            image.DeletedAt = null;
            await _collection.InsertOneAsync(image, new InsertOneOptions()
            {
                BypassDocumentValidation = false
            }, cancellationToken);
            return image.Id;
        }

        public async Task<List<Image>> GetImagesAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.AsQueryable().ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<(long count, IReadOnlyList<Image> data)> GetImagesPaginatedAsync(int pageNumber, int pageSize, bool showDeleted,
            CancellationToken cancellationToken = default)
        {
            var countFacet = AggregateFacet.Create("count",
                PipelineDefinition<Image, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<Image>()
                }));
            var dataFacet = AggregateFacet.Create("data",
                PipelineDefinition<Image, Image>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Sort(Builders<Image>.Sort.Ascending(x => x.CreatedAt)),
                    PipelineStageDefinitionBuilder.Skip<Image>((pageNumber - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<Image>(pageSize)
                }));

            var filter = Builders<Image>.Filter.Where(x => showDeleted || x.DeletedAt == null);
            var aggregation = await _collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync(cancellationToken: cancellationToken);

            var count = aggregation.First()
                .Facets.First(x => x.Name == "count")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "data")
                .Output<Image>();

            return (count, data);
        }

        public async Task<Image> GetImageById(Guid id, CancellationToken cancellationToken = default)
        {
            var cursor = await _collection.FindAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
            return await cursor.FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }

        public async Task<bool> UpdateImageDescriptionAsync(Guid id, string description, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Image>.Filter.Where(x => x.Id.Equals(id));
            var update = Builders<Image>.Update.Set(x => x.UpdatedAt, DateTime.Now);
            update = update.Set(x => x.Description, description);
            var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            return result.IsAcknowledged && result.ModifiedCount == 1;
        }

        public async Task<bool> SoftDeleteImageAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Image>.Filter.Where(x => x.Id.Equals(id));
            var update = Builders<Image>.Update.Set(x => x.UpdatedAt, DateTime.Now);
            update = update.Set(x => x.DeletedAt, DateTime.Now);
            var result = await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            return result.IsAcknowledged && result.ModifiedCount == 1;
        }

        public async Task<bool> DeleteImageAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
            return result.DeletedCount > 0;
        }
    }
}