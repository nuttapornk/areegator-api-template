using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.HealthChecks;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure.Logging;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares.Logging;
using Elastic.Apm.NetCoreAll;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration,builder.Environment.EnvironmentName);
builder.Services.AddComponentService(builder.Configuration, builder.Environment.EnvironmentName);

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
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<RequestHeaderMiddleware>();


//health
app.MapHealthChecks("/health", new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains("startup"), ResponseWriter = HealthCheckResponseWriter.WriteAsync });
app.MapHealthChecks("/health/ready", new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains("ready"), ResponseWriter = HealthCheckResponseWriter.WriteAsync });
app.MapHealthChecks("/health/live", new HealthCheckOptions { Predicate = healthCheck => healthCheck.Tags.Contains("live"), ResponseWriter = HealthCheckResponseWriter.WriteAsync });

app.MapControllers();
app.Run();

namespace _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_
{
    public partial class Program { }
}