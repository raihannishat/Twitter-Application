Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web api successfully!");
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddApplicationServices();
    builder.Services.AddWebApiServices(builder.Configuration);

    var app = builder.Build();

    app.UseSerilogRequestLogging(configure =>
    {
        configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} {UserId} {UserEmail} responded {StatusCode} in {Elapsed:0.0000}ms";
    });

    app.UseMiddleware<ExceptionMiddleware>();

    if (app.Environment.IsDevelopment())
    {
        //app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseSession();
    // app.MapControllers();
    app.UseRouting();

    app.UseCors(x => x
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithOrigins(builder.Configuration.GetValue<string>("FrontendURL")));

    app.UseAuthentication();

    app.UseAuthorization();

    app.UseMiddleware<LoggerMiddleware>();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers();
        endpoints.MapHub<NotificationHub>("/notificationHub");
    });

    app.MapGet("/ping", () => "pong");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Web api terminated unexpectedly :-(");
}
finally
{
    Log.CloseAndFlush();
}