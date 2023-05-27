using Application.IdentityManagement.User.Shared.Services;

namespace Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddHttpContextAccessor();

        services.AddScoped<IBlockUserService, BlockUserService>();
        services.AddScoped<ICommentService, CommentService>();
        services.AddScoped<IFollowUserService, FollowUserService>();
        services.AddScoped<ILikeService, LikeService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IRetweetService, RetweetService>();
        services.AddScoped<ITweetService, TweetService>();
        services.AddScoped<IHashtagService, HashtagService>();
        services.AddScoped<ITimelineService, TimelineService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ITweetViewModelFactory, TweetViewModelFactory>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }
}
