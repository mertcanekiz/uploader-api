using System;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Uploader.Application.Images.Repositories;
using Uploader.Application.Images.Services;
using Uploader.Infrastructure.Config;
using Uploader.Infrastructure.Repositories;
using Uploader.Infrastructure.Services;

namespace Uploader.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoSettings = configuration.GetSection("MongoSettings").Get<MongoSettings>();
            services.AddSingleton<IMongoClient, MongoClient>(
                _ => new MongoClient(mongoSettings.ConnectionString));
            services.AddSingleton<IImageRepository, ImageRepository>();

            var s3Settings = configuration.GetSection("S3Settings").Get<S3Settings>();
            Console.WriteLine($"Bucketname: {s3Settings.BucketName}");
            services.AddSingleton(s3Settings);
            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddSingleton<IS3Service, S3Service>();

            return services;
        }
    }
}