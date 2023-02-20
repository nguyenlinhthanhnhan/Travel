using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Travel.Data.Contexts;
using Travel.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<TravelDbContext>(options =>
{
    var travelDbConnectionString =
        builder.Configuration.GetSection("ConnectionStrings").Get<Settings>()?.TravelDbConnectionString;
    options.UseSqlServer(travelDbConnectionString);
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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
