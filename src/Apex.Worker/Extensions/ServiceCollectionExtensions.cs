using Apex.Application.Client;
using Apex.Worker.Configuration;
using Apex.Worker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Apex.Worker.Extensions;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorkerServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<OpenF1ApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://api.openf1.org/v1/");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromMinutes(5);
            })
            .AddPolicyHandler(HttpClientPolicies.GetRetryPolicy())
            .AddPolicyHandler(HttpClientPolicies.GetCircuitBreakerPolicy());

        services.AddScoped<IOpenF1ApiClient>(provider => provider.GetRequiredService<OpenF1ApiClient>());

        services.AddHostedService<DataIngestionWorker>();

        return services;
    }
}