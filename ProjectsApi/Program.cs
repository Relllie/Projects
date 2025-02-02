using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using projects_api.Core.Interfaces;
using projects_api.Core.Middlewares;
using projects_api.Core.Services;
using projects_entity;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Init");

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var ProjectsDbConnection = builder.Configuration["ConnectionStrings:ProjectsDb"];
    builder.Services.AddDbContext<ProjectsContext>(options => options.UseLazyLoadingProxies().UseNpgsql(ProjectsDbConnection), ServiceLifetime.Transient);


    builder.Services.AddControllers();

    builder.Services.AddTransient<ErrorHandlingMiddleware>();

    builder.Services.AddScoped<IProjectService, ProjectService>();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseMiddleware<ErrorHandlingMiddleware>();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    logger.Error(e, "Stopped because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}