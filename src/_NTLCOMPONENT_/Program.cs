using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Application;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Infrastructure;
using _NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddComponentService(builder.Configuration, builder.Environment.EnvironmentName);
builder.Services.AddControllers();

//builder.Services.AddSwaggerGen();
var app = builder.Build();

//MiddleWare
app.UseMiddleware<RequestHeaderMiddleware>();

// // Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseReDoc();
}

//health
app.UseHealthChecks("/health");
app.MapHealthChecks("/alive", new HealthCheckOptions
{
    //ResponseWriter = HealthCheckAlive.WriteAsync
});

app.MapControllers();
app.Run();
