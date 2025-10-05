using Microsoft.Extensions.Configuration;
using Apex.Infrastructure;

namespace Apex.IntegrationTests;

public class OptionsProvider
{
    private readonly IConfigurationRoot _configuration;

    public OptionsProvider()
    {
        _configuration = GetConfigurationRoot();
    }

    public T GetOptions<T>(string sectionName) where T : class, new()
        => _configuration.GetOptions<T>(sectionName);

    private static IConfigurationRoot GetConfigurationRoot()
        => new ConfigurationManager()
            .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();
}