using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Travel.Application.Common.Interfaces;
using Travel.Domain.Settings;
using Travel.Shared.Files;
using Travel.Shared.Services;

namespace Travel.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureShared(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        serviceCollection.AddTransient<IDateTime, DateTimeService>();
        serviceCollection.AddTransient<IEmailService, EmailService>();
        serviceCollection.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
        return serviceCollection;
    }
}