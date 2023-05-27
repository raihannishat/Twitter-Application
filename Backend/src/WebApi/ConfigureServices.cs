using Application.Common.Interfaces;
using Application.Tweets.Shared.Interfaces;
using WebApi.Services;

namespace WebApi;

public static class ConfigureServices
{
    public static IServiceCollection AddWebApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddControllers().AddFluentValidation(c =>
              c.RegisterValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));

        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        services.AddEndpointsApiExplorer();

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
               Encoding.ASCII.GetBytes(configuration.GetSection("JwtSettings:Secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RequireExpirationTime = false,
            ClockSkew = TimeSpan.FromSeconds(0),
        };

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddSingleton(tokenValidationParameters);

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
            {
                new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        services.AddCors();
        services.AddOptions();
        services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 102400000;
            });
        services.AddDistributedMemoryCache();
        services.AddSession();


        return services;
    }

    public static void RegisterTweetConsumer(IServiceProvider serviceProvider)
    {
        var tweetConsumer = (ITweetConsumer?)serviceProvider.GetService(typeof(ITweetConsumer));
    }
}