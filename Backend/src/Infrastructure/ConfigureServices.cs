using Application.Block.Shared.Interfaces;
using Application.Comments.Shared.Interfaces;
using Application.Likes.Shared.Interfaces;
using Application.Notifications.Shared.Interfaces;
using Application.Photos.Shared.Interfaces;
using Application.Retweets.Shared.Interfaces;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITwitterDbContext, TwitterDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IFollowRepository, FollowRepository>();
        services.AddScoped<ITweetRepository, TweetRepository>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITweetPublisher, TweetPublisher>();
        services.AddScoped<ITweetConsumer, TweetConsumer>();
        //services.AddScoped<INotificationPublisher, NotificationPublisher>();
        //services.AddScoped<INotificationConsumer, NotificationConsumer>();
        //services.AddScoped<ITweetCacheService, TweetCacheService>();
        services.AddScoped<IHomeTimelineRepository, HomeTimelineRepository>();
        services.AddScoped<IUserTimelineRepository, UserTimelineRepository>();
        services.AddScoped<IHashtagRepository, HashtagRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IRetweetRepository, RetweetRepository>();
        services.AddScoped<IBlockRepository, BlockRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<ISearchRepository, SearchRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IPhotoAccessor, PhotosAccessor>();
        services.AddScoped<IRedisTokenService, RedisTokenService>();

        services.AddSingleton<IRabbitMQSettings>(configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>());
        services.AddSingleton<IRedisSettings>(configuration.GetSection(nameof(RedisSettings)).Get<RedisSettings>());
        services.AddSingleton<IMongoDbSettings>(configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>());

        services.Configure<CloudinarySettings>(configuration.GetSection("Cloudinary"));
     
        return services;
    }
}
