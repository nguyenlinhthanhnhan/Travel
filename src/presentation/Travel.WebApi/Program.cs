using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.MSSqlServer;
using Swashbuckle.AspNetCore.SwaggerGen;
using Travel.Application;
using Travel.Data;
using Travel.Identity;
using Travel.Identity.Helpers;
using Travel.Shared;
using Travel.WebApi;
using Travel.WebApi.Extensions;
using Travel.WebApi.Filters;
using Travel.WebApi.Helpers;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddSerilog();

// Add services to the container.
var travelDbConnectionString =
    builder.Configuration.GetSection("ConnectionStrings").Get<AppSettings>()?.TravelDbConnectionString;
var serilogDbConnectionString =
    builder.Configuration.GetSection("ConnectionStrings").Get<AppSettings>()?.SerilogDbConnectionString;

builder.Services.AddInfrastructureData(travelDbConnectionString);

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructureShared(builder.Configuration);
builder.Services.AddInfrastructureIdentity(builder.Configuration);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ApiExceptionFilter());
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Swagger configs
builder.Services.AddApiVersioningExtension();
builder.Services.AddVersionedApiExplorerExtension();
builder.Services.AddSwaggerGenExtension();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


var app = builder.Build();

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwaggerExtension(provider);
}

app.UseHttpsRedirection();

app.UseMiddleware<JwtMiddleware>();

app.UseAuthorization();

app.MapControllers();

var assemblyName = Assembly.GetExecutingAssembly().GetName();
Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .Enrich.WithMachineName()
    .Enrich.WithProperty("Assembly", $"{assemblyName.Name}")
    .Enrich.WithProperty("Assembly", $"{assemblyName.Version}")
    .WriteTo.MSSqlServer(serilogDbConnectionString, sinkOptions: new MSSqlServerSinkOptions
    {
        TableName = "TravelLog"
    }, restrictedToMinimumLevel: LogEventLevel.Information)
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting host");
    app.Run();
    return 0;
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}