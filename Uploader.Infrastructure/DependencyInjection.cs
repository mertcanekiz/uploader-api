using System;
using System.Text;
using Amazon.S3;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using Uploader.Application.Common.Interfaces;
using Uploader.Application.Images.Repositories;
using Uploader.Application.Images.Services;
using Uploader.Infrastructure.Config;
using Uploader.Infrastructure.Identity;
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

            services.AddIdentityMongoDbProvider<ApplicationUser, MongoRole<Guid>, Guid>(identity =>
            {
                identity.Password.RequiredLength = 8;
            }, mongo =>
            {
                mongo.ConnectionString = mongoSettings.ConnectionString + "/uploader";
            }).AddDefaultTokenProviders();
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        // ValidAudience = configuration["JWT:ValidAudience"],
                        // ValidIssuer = configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                    };
                });
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }
    }
}