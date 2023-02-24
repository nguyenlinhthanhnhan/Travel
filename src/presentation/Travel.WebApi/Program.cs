using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Travel.Application;
using Travel.Data;
using Travel.Data.Contexts;
using Travel.Shared;
using Travel.WebApi;
using Travel.WebApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var travelDbConnectionString =
    builder.Configuration.GetSection("ConnectionStrings").Get<Settings>()?.TravelDbConnectionString;
builder.Services.AddInfrastructureData(travelDbConnectionString);

builder.Services.AddApplication();
builder.Services.AddInfrastructureShared(builder.Configuration);

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

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Travel.WebAPI", Version = "v1"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
