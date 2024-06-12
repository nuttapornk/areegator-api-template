using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.HealthChecks;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares;
using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Diagnostics;
using System.Reflection;

Assembly assembly = Assembly.GetExecutingAssembly();
FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddComponentService(builder.Configuration, builder.Environment.EnvironmentName);

//config HealthCheck
builder.Services.Configure<HealthCheckResponseOptions>(options =>
{
    options.FileVersion = fileVersionInfo.FileVersion;
});

Log.Logger = ElasticApmLogging.CreateSeriLogger(builder.Configuration);

//builder.Services.AddSwaggerGen();
var app = builder.Build();

// // Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseReDoc();
}

app.UseAllElasticApm(builder.Configuration);

//MiddleWare
app.UseMiddleware<RequestHeaderMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

//health
app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains("startup"), ResponseWriter = HealthCheckResponseWriter.AliveAsync });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains("ready"), ResponseWriter = HealthCheckResponseWriter.WriteAsync });
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains("live"), ResponseWriter = HealthCheckResponseWriter.WriteAsync });

app.UseCors("CORS");

app.MapControllers();
app.Run();

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_
{
    public partial class Program { }
}